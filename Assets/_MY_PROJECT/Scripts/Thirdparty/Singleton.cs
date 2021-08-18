using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    [SerializeField] private bool dontDestroy = false;

    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = Object.FindObjectOfType<T>();

                if (m_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    m_instance = singleton.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }


    public virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;

            if (dontDestroy)
            {
                transform.parent = null;
                DontDestroyOnLoad(this.gameObject);
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    
    
    
    
    
}
