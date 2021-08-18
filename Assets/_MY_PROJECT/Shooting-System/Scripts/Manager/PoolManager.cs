using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public static Dictionary<string, object> pool = new Dictionary<string, object>();
    public static Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    public static void CreatePool<T>(GameObject prefab, Transform parent, int count = 5)
    {
        Queue<T> q = new Queue<T>();
        for (int i = 0; i < count; i++)
        {
            GameObject g = GameObject.Instantiate(prefab, parent);

            T t = g.GetComponent<T>();
            g.SetActive(false);
            q.Enqueue(t);
        }

        string key = typeof(T).ToString();
        pool.Add(key, q);
        prefabDictionary.Add(key, prefab);
    }

    public static T GetItem<T>() where T : MonoBehaviour
    {
        string key = typeof(T).ToString();
        T item = null;

        if (pool.ContainsKey(key))
        {
            Queue<T> q = (Queue<T>)pool[key];
            T firstItem = q.Peek();

            if (firstItem.gameObject.activeSelf)
            {
                GameObject prefab = prefabDictionary[key];
                GameObject g = GameObject.Instantiate(prefab, firstItem.transform.parent);
                item = g.GetComponent<T>();
            }
            else
            {
                item = q.Dequeue();
                item.gameObject.SetActive(true);
            }

            q.Enqueue(item);
        }

        return item;
    }
}
