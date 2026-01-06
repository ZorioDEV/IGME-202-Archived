using UnityEngine;

/// <summary>
/// "Raise" an object (increase its y value)
/// </summary>
public class RaiseMe : MonoBehaviour
{
    // *** FIELDS ***           // Values for testing:
    [Header("Values")]
    [SerializeField]
    private float speed;        // 0.005f

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // increase y value of transform.position by speed
        
        transform.position += new Vector3(0, speed, 0);

        // OR, alternatively:
        //Vector3 positionCopy = transform.position;
        //positionCopy.y += speed;
        //transform.position = positionCopy;

    }
}
