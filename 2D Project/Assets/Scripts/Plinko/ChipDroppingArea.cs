using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Button area where you can spawn in chips to drop.
/// </summary>
public class ChipDroppingArea : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Reference to the Chip prefab")]
    [SerializeField] private GameObject chipPrefab;
    [Tooltip("Max amount of chips allowed per game.")]
    [SerializeField] private int maxChips = 5;

    [Header("UI")]
    [Tooltip("TMP Text reference UI.")]
    [SerializeField] private TMP_Text chipsText;
    [Tooltip("Format of chips remaining UI display.")]
    [SerializeField] private string displayFormat = "Chips: {0}/{1}";

    private int chipsRemaining;
    private Camera mainCamera;
    private Button dropButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        mainCamera = Camera.main;
        dropButton = GetComponent<Button>();

        // sets up the button click event
        dropButton.onClick.AddListener(OnDropButtonClicked);

        ResetChips();
        UpdateChipDisplay();
    }

    /// <summary>
    /// Checks if the player used all chips & if there's no more Chip prefabs within the scene.
    /// </summary>
    private void Update()
    {
        Chip[] chipsInScene = FindObjectsOfType<Chip>();

        if (chipsRemaining <= 0 && chipsInScene.Length == 0)
        {
            PlinkoManager.Instance.ResetScore();

            ResetChips();
        }
    }

    // *** METHODS ***
    /// <summary>
    /// Occurs whent the player clips the button, converting the mouse 
    /// position into a spawn point of the new Chip prefab.
    /// </summary>
    private void OnDropButtonClicked()
    {
        if (chipsRemaining <= 0) return;

        // gets current mouse position
        Vector3 mousePosition = Input.mousePosition;

        // converts screen position to world position
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;

        // instantiates the chip with correct position
        Instantiate(chipPrefab, worldPosition, Quaternion.identity);

        // updates chip count and UI
        chipsRemaining--;
        UpdateChipDisplay();
    }

    /// <summary>
    /// Updates the chips remaining text UI.
    /// </summary>
    private void UpdateChipDisplay()
    {
        chipsText.text = string.Format(
            displayFormat, 
            chipsRemaining, 
            maxChips
        );
    }

    /// <summary>
    /// Resets the chip count back to max chips.
    /// </summary>
    private void ResetChips()
    {
        chipsRemaining = maxChips;
        UpdateChipDisplay();
    }
}
