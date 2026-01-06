using System;
using UnityEngine;

public class TriangleMover : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Speed of the object.")]
    public float moveSpeed;     // 0.001f

    // Update is called once per frame
    void Update()
    {
        MoveForward();
    }

    /// <summary>
    /// Locally moves the GO's position up 1 on the Y-cord, making it go forward in the direction its facing.
    /// </summary>
    private void MoveForward()
    {
        gameObject.transform.Translate(new Vector3(0, 1, 0) * moveSpeed);
    }
}
