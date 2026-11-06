using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Coin")]
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int maxCoins = 3;          // coins visible at once
    [SerializeField] float coinHeight = 0.5f;
    [SerializeField] float coinRadius = 0.3f;   // clearance needed around the coin

    [Header("Spawn area (yellow box in the Scene view)")]
    [SerializeField] Vector3 areaSize = new Vector3(9f, 0.1f, 9f);

    [Header("Walls (ONLY the Wall layer - not the floor, not Default)")]
    [SerializeField] LayerMask wallMask;

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] float minDistanceFromPlayer = 1.5f;

    readonly List<GameObject> coins = new List<GameObject>();
    float nextWarn;

    void Start()
    {
        if (coinPrefab == null) Debug.LogError("Spawner: Coin Prefab is not assigned!", this);
        if (GameManager.Instance == null) Debug.LogError("Spawner: no GameManager found in the scene!", this);
        if (wallMask.value == 0) Debug.LogWarning("Spawner: Wall Mask is Nothing, so coins CAN spawn inside walls. Set it to the Wall layer.", this);
    }

    void Update()
    {
        if (coinPrefab == null || GameManager.Instance == null) return;
        coins.RemoveAll(c => c == null);

        // never spawn more coins than are still needed to open the door
        int remaining = GameManager.Instance.TargetScore - GameManager.Instance.Score;
        int wanted = Mathf.Min(maxCoins, remaining);

        while (coins.Count < wanted)
        {
            if (!TryGetPoint(out Vector3 pos))
            {
                if (Time.time > nextWarn)
                {
                    nextWarn = Time.time + 3f;
                    Debug.LogWarning("Spawner: no free spot found. Check that (1) Wall Mask is only the Wall layer and the floor is NOT on it, " +
                                     "(2) the yellow box overlaps the maze floor, (3) coin height/radius aren't touching the floor.", this);
                }
                break;
            }
            coins.Add(Instantiate(coinPrefab, pos, Quaternion.identity));
        }
    }

    bool TryGetPoint(out Vector3 pos)
    {
        Vector3 c = transform.position;
        for (int i = 0; i < 60; i++)
        {
            pos = new Vector3(
                c.x + Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                c.y + coinHeight,
                c.z + Random.Range(-areaSize.z / 2f, areaSize.z / 2f));

            // reject points inside / touching a wall (only if a wall mask is set)
            if (wallMask.value != 0 &&
                Physics.CheckSphere(pos, coinRadius, wallMask, QueryTriggerInteraction.Ignore)) continue;

            if (player != null)
            {
                Vector3 flat = pos - player.position; flat.y = 0f;
                if (flat.magnitude < minDistanceFromPlayer) continue;
            }
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.up * coinHeight;
        Gizmos.color = new Color(1f, 0.92f, 0.1f, 0.15f);
        Gizmos.DrawCube(center, areaSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, areaSize);
    }
}