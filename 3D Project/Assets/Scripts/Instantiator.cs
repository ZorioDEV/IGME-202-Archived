using UnityEngine;

public class Instantiator : MonoBehaviour
{
    // *** FIELDS ***           // Values for testing:
    [Header("Values")]
    [SerializeField]
    private int number;         // 500

    [Header("References")]
    public GameObject boxPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ////number = 0;
        //Debug.Log("Number is " + number);

        for (int i = 0; i < 100; i++)
        {
            float xPosition = Random.Range(-3f, 4f);
            float yPosition = Random.Range(1f, 4f);
            float zPosition = Random.Range(-10f, 11f);

            Instantiate(
                boxPrefab,                                      // Which prefab?
                new Vector3(xPosition, yPosition, zPosition),   // Where?
                Quaternion.identity                             // Rotation?
            ); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
