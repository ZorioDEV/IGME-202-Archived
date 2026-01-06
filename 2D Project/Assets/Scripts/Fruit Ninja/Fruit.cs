using UnityEngine;

/// <summary>
/// Fruit variant (isBeneficial = true) of SliceObject.
/// </summary>
public class Fruit : SliceObject
{
    [Header("Settings")]
    public int points = 10;
    public Sprite slicedSprite;

    /// <summary>
    /// Handles when the Bomb is being sliced.
    /// </summary>
    public override void OnSliced()
    {
        if (isBeingDestroyed) return;
        isBeingDestroyed = true;

        // adds to points, changes to split sprite, & plays sound effect
        FruitNinjaManager.Instance.AddScore(points);
        spriteRenderer.sprite = slicedSprite;
        FruitNinjaManager.Instance.PlayRandomFruitSplatSound();

        // starts to be destroyed
        StartCoroutine(Destruction());
    }
}