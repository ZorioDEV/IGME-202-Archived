using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Main Manager for Fruit Ninja gameloop, UI, & audio.
/// </summary>
public class FruitNinjaManager : MonoBehaviour
{
    public static FruitNinjaManager Instance;

    [Header("Settings")]
    public int startLives;      // 3 - three total lives/health before game over (-1 life when slices bomb)
    public int missedFruitLimit;       // 3 - three total missed fruit before game over

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text missedText;
    public GameObject gameOverPanel;

    [Header("Health")]
    public Image healthDisplay;
    public Sprite[] healthSprites;      // 4 total stages - 3, 2, 1, & 0 lives

    [Header("Audio")]
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip bombThrowSound;
    public AudioClip bombExplodeSound;
    public AudioClip fruitThrowSound;
    public AudioClip[] fruitSplatSounds;
    public AudioClip gameOverSound;
    public AudioClip gameStartSound;

    private int currentScore;
    private int currentLives;
    private int missedCount;
    private bool isGameOver = false;

    /// <summary>
    /// Ensures that there is only one FruitNinjaManager in the scene.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts the beginning of the game & restarts all feilds to default.
    /// </summary>
    void Start()
    {
        // resets to defualt values
        currentScore = 0;
        currentLives = startLives;
        missedCount = 0;
        isGameOver = false;

        // updates UI & ensures gameover panel isn't showing
        UpdateUI();
        gameOverPanel.SetActive(false);

        // plays starting sounds
        PlayGameStartSound();
        PlayBackgroundMusic();
    }

    /// <summary>
    /// Checks for any key press to restart when game is over (restarts by reloading the scene).
    /// </summary>
    void Update()
    {
        // when game is over, allows for any key to restart
        if (isGameOver && Input.anyKeyDown)
        {
            SceneManager.LoadScene("Fruit Ninja");
        }
    }

    /// <summary>
    /// Adds points to the total score.
    /// </summary>
    /// <param name="points">The amount of points to be added.</param>
    public void AddScore(int points)
    {
        if (isGameOver) return;

        currentScore += points;

        // updates ALL UI
        UpdateUI();
    }

    /// <summary>
    /// Handles taking damage/health.
    /// </summary>
    /// <param name="damage">The amount of damage being done.</param>
    public void TakeDamage(int damage)
    {
        if (isGameOver) return;

        currentLives -= damage;

        // when lives are zero, it ends the game
        if (currentLives <= 0)
        {
            currentLives = 0;
            GameOver();
        }

        // updates ALL UI
        UpdateUI();
    }

    /// <summary>
    /// Handles the amount of missed fruit.
    /// </summary>
    public void FruitMissed()
    {
        if (isGameOver) return;

        missedCount++;

        // when the amount of missed fruit reaches it's limit, it ends the game
        if (missedCount >= missedFruitLimit)
        {
            GameOver();
        }

        // updates ALL UI
        UpdateUI();
    }

    /// <summary>
    /// Handles the end of the game.
    /// </summary>
    void GameOver()
    {
        // changes bool to true, letting other scripts know the game is over
        isGameOver = true;

        // activates the gameover panel
        gameOverPanel.SetActive(true);

        // plays game over sound effect & ends music
        PlayGameOverSound();
        StopBackgroundMusic();

        // gets the SpawnManager in the scene & stops the spawning
        SpawnManager spawnManager = FindFirstObjectByType<SpawnManager>();
        spawnManager.StopSpawning();
    }

    /// <summary>
    /// Handles the displaying of ALL the game UI.
    /// </summary>
    void UpdateUI()
    {
        // updates the current score
        scoreText.text = $"Score: {currentScore}";

        // updates the current missed fruit
        missedText.text = $"Missed: {missedCount}/{missedFruitLimit}";

        // updates health display to the correct amount of lives left
        healthDisplay.sprite = healthSprites[currentLives];
    }

    /// <summary>
    /// Allows other scripts to check if the game is over or not.
    /// </summary>
    /// <returns>The opposite of the bool isGameOver.</returns>
    public bool IsGameActive()
    {
        return !isGameOver;
    }

    #region --- AUDIO METHODS ---
    private void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void PlayBombThrowSound()
    {
        PlaySFX(bombThrowSound);
    }

    public void PlayBombExplodeSound()
    {
        PlaySFX(bombExplodeSound);
    }

    public void PlayFruitThrowSound()
    {
        PlaySFX(fruitThrowSound);
    }

    public void PlayRandomFruitSplatSound()
    {
        int randomIndex = Random.Range(0, fruitSplatSounds.Length);     // randomly selects 1 of the 4 splatter sound effects
        PlaySFX(fruitSplatSounds[randomIndex]);
    }

    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
    }

    public void PlayGameStartSound()
    {
        PlaySFX(gameStartSound);
    }

    public void PlayBackgroundMusic()
    {
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        musicAudioSource.Stop();
    }
    #endregion
}