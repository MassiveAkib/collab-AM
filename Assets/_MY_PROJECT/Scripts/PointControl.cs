using UnityEngine;

public class PointControl : MonoBehaviour
{
    public GameObject[] points;
    private GameObject _point;
    void Start()
    {
        
        _point = points[Random.Range(0,4)];

        _point.SetActive(true);
    }

}
