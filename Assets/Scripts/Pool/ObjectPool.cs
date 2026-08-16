using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private int poolSize = 10;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    [Header("Pool Configurations")]
    public GameObject normalGemPrefab;
    public GameObject rareGemPrefab;
    public GameObject gemUIPrefab;
    public GameObject confettiPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (normalGemPrefab != null) InitializeNewPool(normalGemPrefab);
        if (rareGemPrefab != null) InitializeNewPool(rareGemPrefab);
        if (gemUIPrefab != null) InitializeNewPool(gemUIPrefab);
        if (confettiPrefab != null) InitializeNewPool(confettiPrefab);
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            InitializeNewPool(prefab);
        }

        if (poolDictionary[prefab].Count == 0)
            CreateNewObject(prefab);

        GameObject objectToGet = poolDictionary[prefab].Dequeue();

        objectToGet.SetActive(true);
        objectToGet.transform.SetParent(null, false);

        return objectToGet;
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject objectToGet = GetObject(prefab);
        objectToGet.transform.position = position;
        objectToGet.transform.rotation = rotation;

        if (parent != null)
        {
            objectToGet.transform.SetParent(parent, false);
        }

        return objectToGet;
    }

    public void ReturnObject(GameObject objectToReturn, float delay = .001f) => StartCoroutine(DelayReturn(delay, objectToReturn));

    private IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool(objectToReturn);
    }

    private void ReturnToPool(GameObject objectToReturn)
    {
        GameObject originalPrefab = objectToReturn.GetComponent<PooledObject>().originalPrefab;

        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(transform, false);

        poolDictionary[originalPrefab].Enqueue(objectToReturn);
    }

    private void InitializeNewPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;
        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
    }
}