using UnityEngine;

public enum GemType
{
    Normal,
    Rare
}


public abstract class GemController : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        TakeDame();
    //    }
    //}

    protected int scoreValue;

    public virtual void TakeDame()
    {
        if (CollectGem.Instance != null)
        {
            CollectGem.Instance.CreateFlyingGem(transform.position, scoreValue);
        }

        ObjectPool.instance.ReturnObject(gameObject);
    }
}
