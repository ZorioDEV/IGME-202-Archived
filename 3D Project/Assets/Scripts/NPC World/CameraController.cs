using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;
    public TMP_Text cameraInstructions;
    public PlayerMovement playerMovement;
    private int currentCameraIndex = 0;

    void Start()
    {
        // initializes the first camera as active & disables all others
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = (i == 0);
        }

        // sets player movement ability based on the active camera & updates on-screen instructions
        playerMovement.SetCanMove(currentCameraIndex == 0);
        UpdateInstructions();
    }

    void Update()
    {
        // detects camera switch input & cycles to the next camera
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            cameras[currentCameraIndex].enabled = false;

            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

            cameras[currentCameraIndex].enabled = true;

            playerMovement.SetCanMove(currentCameraIndex == 0);
            UpdateInstructions();
        }
    }

    void UpdateInstructions()
    {
        cameraInstructions.text = $"Camera {currentCameraIndex + 1}/{cameras.Length} (Press SPACE to switch)" +
            $"\nSweep (Right mouse button)";
    }
}
