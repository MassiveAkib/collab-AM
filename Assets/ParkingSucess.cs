using UnityEngine;
using UnityEngine.SceneManagement;

public class ParkingSucess : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Cinematic_PlaneHijack");
        }

        
    }
}
