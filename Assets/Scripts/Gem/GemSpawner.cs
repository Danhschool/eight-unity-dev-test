using System.Collections;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [Header("Setting Gem")]
    //[SerializeField] private GameObject gemPrefab;
    [SerializeField] private int initialGemCount = 10;
    [SerializeField] private Transform gemContainer;

    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxGemsOnMap = 20;

    [Header("Map Area (X and Z Axis)")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minZ = -20f;
    [SerializeField] private float maxZ = 20f;

    [Header("Raycast Settings")]
    [SerializeField] private float raycastHeight = 50f;
    [SerializeField] private float heightOffset = 0.5f;

    [Header("Layer Detection")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask obstacleMask; 

    [SerializeField] private int maxAttempts = 30;


    private void Start()
    {
        for (int i = 0; i < initialGemCount; i++)
        {
            SpawnSingleGem();
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true) 
        {
            yield return new WaitForSeconds(spawnInterval);

            if (transform.childCount < maxGemsOnMap)
            {
                SpawnSingleGem();
            }
        }
    }

    private void SpawnSingleGem()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            Vector3 rayOrigin = new Vector3(randomX, raycastHeight, randomZ);

            LayerMask combinedMask = groundMask | obstacleMask;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, Mathf.Infinity, combinedMask))
            {
                if ((groundMask.value & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    Vector3 spawnPosition = hit.point + Vector3.up * heightOffset;

                    //Instantiate(gemPrefab, spawnPosition, Quaternion.identity, gemContainer);
                    GemType randomType = (Random.Range(0f, 100f) < 67f) ? GemType.Normal : GemType.Rare;
                    GameObject newGem = GemFactory.Instance.CreateGem(randomType, spawnPosition);
                    return;
                }

            }
        }

        Debug.LogWarning("Cant Spawn Gem.");
    }
}