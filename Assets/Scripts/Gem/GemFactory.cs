using UnityEngine;

public class GemFactory : MonoBehaviour
{
    public static GemFactory Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public GameObject CreateGem(GemType type, Vector3 position, Transform parent = null)
    {
        GameObject prefabToSpawn = null;

        switch (type)
        {
            case GemType.Normal:
                prefabToSpawn = ObjectPool.instance.normalGemPrefab;
                break;
            case GemType.Rare:
                prefabToSpawn = ObjectPool.instance.rareGemPrefab;
                break;
        }

        if (prefabToSpawn != null)
        {
            return ObjectPool.instance.GetObject(prefabToSpawn, position, Quaternion.identity, parent);
        }

        return null;
    }
}