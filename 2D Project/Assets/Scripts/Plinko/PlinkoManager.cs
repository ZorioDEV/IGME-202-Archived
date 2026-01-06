using UnityEngine;

/// <summary>
/// Manages the data of highscore and scores of each game.
/// </summary>
public class PlinkoManager : MonoBehaviour
{
    public static PlinkoManager Instance { get; private set; }

    private int currentScore = 0;
    private int highScore = 0;

    /// <summary>
    /// READ & WRITE property for the user's current score.
    /// </summary>
    public int CurrentScore
    {
        get
        {
            return currentScore;
        }
        private set
        {
            currentScore = value;

            // sets new score to highscore if higher
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }
        }
    }

    /// <summary>
    /// READ-only property of the user's highscore.
    /// </summary>
    public int HighScore
    {
        get
        {
            return highScore;
        }
    }

    /// <summary>
    /// Loads PlayerPrefs & creates an instance of the PlinkoManager.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // *** METHODS ***
    /// <summary>
    /// Loads the highscore from PlayerPrefs.
    /// </summary>
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    /// <summary>
    /// Adds the specific amount of points to the current score.
    /// </summary>
    /// <param name="points">Amount of points added.</param>
    public void AddScore(int points)
    {
        CurrentScore += points;
    }

    /// <summary>
    /// Resets the score back to zero.
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
