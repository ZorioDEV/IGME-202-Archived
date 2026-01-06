using System;
using UnityEngine;

public class FishMovementWithInputManager : MonoBehaviour
{
    // *** FEILDS ***
    /// <summary>
    /// Object that this script moves with WASD or arrow keys
    /// </summary>
    public GameObject fish;

    /// <summary>
    /// Unit-based movement per frame for the fish
    /// </summary>
    public float fishSpeed;     // 5f
    public Vector3 fishDirection;

    public float speedOfRotation;   // 1f
    public float totalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// Update is called once per frame
    void Update()
    {
        // Start by moving fish independently on X or Y axis using per-frame movement.
        MoveFish();

        //// Rotate the fish
        //RotateFish();
    }

    /// <summary>
    /// Rotates the fish with a Quaternion
    /// </summary>
    public void RotateFish()
    {
        // Increases the total rotation by the rotation speed
        totalRotation += speedOfRotation * Time.deltaTime;

        // Update the transform's rotation with a quaternion calculated by Unity
        Quaternion newRotation = Quaternion.Euler(0, 0, totalRotation);
        fish.transform.rotation = newRotation;
    }

    /// <summary>
    /// Use Unity's Input Manager system to move the fish in 4 cardinal directions
    ///    with WASD.
    /// </summary>
    public void MoveFish()
    {
        // Grab the fish's current position as a locally-declared struct
        // such that it is modifiable.
        Vector3 fishPosition = fish.transform.position;

        // Move the jellyfish!
        // When the A key is pressed, move fish to the left
        if (Input.GetKey(KeyCode.A))				// Left
        {
            // Prove to ourselves that the key presses are working!
            UnityEngine.Debug.Log("Pressing A");

            // Move the object a tiny unit left
            fishPosition.x -= fishSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))                       // Right
        {
            // Prove to ourselves that the key presses are working!
            UnityEngine.Debug.Log("Pressing D");

            // Move the object a tiny unit right
            fishPosition.x += fishSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))                       // Up
        {
            // Prove to ourselves that the key presses are working!
            UnityEngine.Debug.Log("Pressing W");

            // Move the object a tiny unit up
            fishPosition.y += fishSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))                       // Down
        {
            // Prove to ourselves that the key presses are working!
            UnityEngine.Debug.Log("Pressing S");

            // Move the object a tiny unit down
            fishPosition.y -= fishSpeed * Time.deltaTime;
        }

        // Transport the fish to that position
        fish.transform.position = fishPosition;
    }

}
