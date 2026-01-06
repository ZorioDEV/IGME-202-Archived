using UnityEngine;

public class RainbowColorCycler : MonoBehaviour
{
    [Header("References")]
    [Tooltip("SpriteRenderer that changes colors.")]
    public SpriteRenderer targetSpriteRenderer;

    [Header("Settings")]
    [Tooltip("Speed of color cycling.")]
    public float colorChangeSpeed;      // 0.1f

    [Tooltip("HSV color hue value (note: 0 to 1).")]
    private float hue = 0f;

    void Update()
    {
        // increase the hue every frame
        hue += colorChangeSpeed * Time.deltaTime;

        // loops the hue, if over 1f -> 100%
        if (hue >= 1f)
        {
            hue = 0f;
        }

        // converts HSV to RGB
        Color rainbowColor = Color.HSVToRGB(
            hue,    // hue
            1f,     // saturation
            1f      // vibrance
        );

        // applies the color to the targetSpriteRenderer
        targetSpriteRenderer.color = rainbowColor;
    }
}
