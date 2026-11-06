using UnityEngine;

// Put this on the SHIELD PREFAB (collider with Is Trigger ticked).
public class Shield : MonoBehaviour
{
    [SerializeField] float spinSpeed = 90f;

    void Update() { transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f); }

    void OnTriggerEnter(Collider other)
    {
        PlayerShield shield = other.GetComponent<PlayerShield>();
        if (shield == null) return;

        shield.Activate();
        Destroy(gameObject);
    }
}