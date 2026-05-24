// CanvasTrigger.cs - Va en el objeto con el Trigger Collider
using UnityEngine;

public class CanvasTrigger : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private string playerTag = "Player";

    void Start()
    {
        if (canvas != null) canvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && canvas != null)
            canvas.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && canvas != null)
            canvas.SetActive(false);
    }
}