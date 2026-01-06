using UnityEngine;

/// <summary>
/// Get rid of this GameObject after a number of seconds
/// </summary>
public class BlowMeUp : MonoBehaviour
{
    // *** FIELDS ***           // Values for testing:
    [Header("Values")]
    [SerializeField]
    private float secondsToLive; // 0.5f

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, secondsToLive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
