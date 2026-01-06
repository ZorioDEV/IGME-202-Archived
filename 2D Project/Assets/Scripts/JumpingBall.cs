using UnityEngine;

public class JumpingBall : MonoBehaviour
{
    public Rigidbody2D rb2D;

    public Vector3 direction;
    public float jumpForce;     // 500f

    public void OnCollisionEnter2D(Collision2D collision)
    {
        rb2D.AddForce(direction * jumpForce);
    }
}
