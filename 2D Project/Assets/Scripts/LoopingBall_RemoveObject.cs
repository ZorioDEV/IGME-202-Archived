using UnityEngine;

public class LoopingBall_RemoveObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // destroys this instance of the ball prefab when it touches any other collider
        Destroy(gameObject);
    }
}
