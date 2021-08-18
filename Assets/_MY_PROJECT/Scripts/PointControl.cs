using UnityEngine;

public class PointControl : MonoBehaviour
{
    public GameObject[] points;
    GameObject point;
    void Start()
    {
        
        point = points[Random.Range(0,4)];

        point.SetActive(true);
    }

}
