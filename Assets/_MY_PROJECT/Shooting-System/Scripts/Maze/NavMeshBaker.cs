using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface[] navMeshSurfaces;

    void Awake()
    {
        for (int i = 0; i < navMeshSurfaces.Length; i++)
        {
            if (navMeshSurfaces[i].gameObject.activeSelf)
            {
                navMeshSurfaces[i].BuildNavMesh();
            }
        }
    }
}
