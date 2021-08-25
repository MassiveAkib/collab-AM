using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject [] images;
    // Start is called before the first frame update
    void Start()
    {
        var i = images.Length-1;
        Debug.Log(i);
        while (i > 0)
        {
            images[i].gameObject.SetActive(false);
            i--;
        }
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    private IEnumerator FadeIn()
    {
        var i = images.Length-1;
        while (i > 0)
        {
            images[i].gameObject.SetActive(true);
            i--;
            yield return new WaitForSeconds(6);
            images[i].gameObject.SetActive(false);
        }
       
    }
    
}
