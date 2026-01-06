using TMPro;
using UnityEngine;

/// <summary>
/// Displays the current score and highscore to a text UI.
/// </summary>
public class DisplayScores : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("TMP Text reference UI.")]
    [SerializeField] private TMP_Text scoreText;
    [Tooltip("Format of score & highscore UI display.")]
    [SerializeField] private string displayFormat = "Score: {0}\nHighscore: {1}";

    /// <summary>
    /// Updates the text UI with the current score and highscore of the games.
    /// </summary>
    private void Update()
    {
        scoreText.text = string.Format(
            displayFormat, 
            PlinkoManager.Instance.CurrentScore, 
            PlinkoManager.Instance.HighScore
        );
    }
}
