using UnityEngine;

/// <summary>
/// Rotates the peg in a circle and loops.
/// </summary>
public class RotatePeg : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Speed of the rotation.")]
    public float speedOfRotation;   // 50f
    [Tooltip("Total amount of rotation thats been applied to the GO.")]
    public float totalRotation;


    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    /// <summary>
    /// Rotates the GO with a Quaternion.
    /// </summary>
    public void Rotate()
    {
        // increases the total rotation by the rotation speed
        totalRotation += speedOfRotation * Time.deltaTime;

        // update the transform's rotation with a quaternion
        Quaternion newRotation = Quaternion.Euler(0, 0, totalRotation);
        gameObject.transform.rotation = newRotation;
    }
}
