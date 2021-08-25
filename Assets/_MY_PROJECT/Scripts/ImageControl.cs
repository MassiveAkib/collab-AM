using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageControl : MonoBehaviour
{
    public GameObject[] images;
    GameObject _image;
    void Start()
    {

        _image = images[Random.Range(0, 3)];

        _image.SetActive(true);
    }
}
