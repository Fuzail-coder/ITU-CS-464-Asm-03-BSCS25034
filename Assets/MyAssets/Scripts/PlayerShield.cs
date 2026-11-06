using UnityEngine;

// Put this on the PLAYER. Holds a one-hit shield.
public class PlayerShield : MonoBehaviour
{
    [SerializeField] GameObject shieldVisual;   // optional bubble (child of the player), starts disabled
    [SerializeField] float graceTime = 0.5f;    // stops one crusher/hazard touch from hitting twice

    float graceUntil;
    public bool Active { get; private set; }

    void Start() { if (shieldVisual) shieldVisual.SetActive(false); }

    public void Activate()
    {
        Active = true;
        if (shieldVisual) shieldVisual.SetActive(true);
    }

    // Returns true if the hit was blocked (damage should be ignored)
    public bool TryAbsorb()
    {
        if (Active)
        {
            Active = false;                      // shield is used up
            graceUntil = Time.time + graceTime;
            if (shieldVisual) shieldVisual.SetActive(false);
            return true;
        }
        return Time.time < graceUntil;           // brief protection right after it breaks
    }
}