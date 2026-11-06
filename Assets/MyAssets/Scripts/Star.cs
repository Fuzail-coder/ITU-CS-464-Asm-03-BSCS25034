using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.Win();
    }

    void Update()
    {
        transform.Rotate(0f, 90f * Time.deltaTime, 0f);
    }
}