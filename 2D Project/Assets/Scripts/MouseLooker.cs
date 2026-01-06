using System;
using UnityEngine;

public class MouseLooker : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Vector of the current mouse's position.")]
    public Vector3 mousePosition;

    // Update is called once per frame
    void Update()
    {
        LookAt();
    }

    /// <summary>
    /// Makes the rotation of the GO the point towards the mouse's position.
    /// </summary>
    private void LookAt()
    {
        // gets mouse position
        mousePosition = Input.mousePosition;

        // converts screen point to world point
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f; // defualts z-value to 0 ALWAYS

        // calculates direction from triangle to mouse position
        Vector3 direction = worldPosition - gameObject.transform.position;

        // calculates the angle & converts to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // rotates triangle 90 degrees, making the triangle point correctly towards the mouse
        angle -= 90f;

        // applies the rotation
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
