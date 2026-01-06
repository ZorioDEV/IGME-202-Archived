using UnityEngine;

/// <summary>
/// Destroys ALL GO's that collide with the boundary.
/// </summary>
public class DestroyBoundary : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        SliceObject sliceObject = other.GetComponent<SliceObject>();

        // checks if GO is sliceObject & a valid fruit (isBeneficial = true)
        if (sliceObject != null 
            && sliceObject.isBeneficial 
            && !sliceObject.isBeingDestroyed
           )
        {
            FruitNinjaManager.Instance.FruitMissed();
        }

        // destroys GO
        Destroy(other.gameObject);
    }
}