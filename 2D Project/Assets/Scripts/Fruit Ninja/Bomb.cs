using UnityEngine;

/// <summary>
/// Bomb variant (isBeneficial = false) of SliceObject.
/// </summary>
public class Bomb : SliceObject
{
    [Header("Settings")]
    public int damage = 1;
    
    /// <summary>
    /// Handles when the Bomb is being sliced.
    /// </summary>
    public override void OnSliced()
    {
        if (isBeingDestroyed) return;
        isBeingDestroyed = true;

        // adds to damage (removes 1 health) & plays sound effect
        FruitNinjaManager.Instance.TakeDamage(damage);
        FruitNinjaManager.Instance.PlayBombExplodeSound();

        // starts to be destroyed
        StartCoroutine(Destruction());
    }
}