using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 90f;
    [SerializeField] int value = 1;
    void Start()
    {
        Debug.Log("Coin spawned: " + name);
    }
    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Coin collected");
        GameManager.Instance.AddScore(value);
        Destroy(gameObject);
    }
}