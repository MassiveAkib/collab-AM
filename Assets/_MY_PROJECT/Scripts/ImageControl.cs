using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageControl : MonoBehaviour
{
    public GameObject[] images;
    GameObject image;
    void Start()
    {

        image = images[Random.Range(0, 3)];

        image.SetActive(true);
    }
}
