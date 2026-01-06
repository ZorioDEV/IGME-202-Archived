using UnityEngine;

public class FollowBall : MonoBehaviour
{
    [Header("References")]
    [Tooltip("GameObject that follows the camera.")]
    public GameObject objectToFollow;

    // Update is called once per frame
    void Update()
    {
        // makes the position of the camera (the attached GO) equal the position of objectToFollow
        gameObject.transform.position = objectToFollow.transform.position;
    }
}
