using UnityEngine;

/// <summary>
/// Attaches this to a "manager" GO to control the Y movement of other GOs
/// </summary>
public class RaiseController : MonoBehaviour
{
    // *** FIELDS ***           // Values for testing:
    [Header("Values")]
    [SerializeField]
    private float speed;        // 0.005f
    [SerializeField]
    private float secondsToLive; // 1.0f

    [Header("References")]
    //public GameObject box1;
    //public GameObject box2;

    public GameObject[] boxes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i] != null)
                Destroy(boxes[i], secondsToLive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // increase y value of transform.position by speed
        //box1.transform.position += new Vector3(0, speed, 0);
        //box2.transform.position += new Vector3(0, speed, 0);

        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i] != null)
                boxes[i].transform.position += new Vector3(0, speed, 0);
        }
    }
}
