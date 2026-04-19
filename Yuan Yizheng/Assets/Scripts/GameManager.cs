using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance;

    // UI references (assigned via drag in Unity)
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject winPanel;
    public GameObject losePanel;

    // Game settings (configured in Unity)
    public float gameTime = 60f;
    public int targetScore = 50;

    // Game state
    private float currentTime;
    private bool isGameOver = false;

    void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Initialize
        currentTime = gameTime;
        isGameOver = false;

        // Hide win/lose panels
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        // Countdown
        currentTime -= Time.deltaTime;

        // Update UI
        UpdateTimerUI();

        // Time's up, lose the game
        if (currentTime <= 0)
        {
            currentTime = 0;
            GameLose();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Display remaining time
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Called externally: check if target score is reached
    public void CheckWinCondition(int currentScore)
    {
        if (isGameOver) return;

        // Update score display
        UpdateScoreUI(currentScore);

        if (currentScore >= targetScore)
        {
            GameWin();
        }
    }

    // Update score UI
    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score + " / " + targetScore;
        }
    }

    // Win
    void GameWin()
    {
        isGameOver = true;
        if (winPanel != null)
            winPanel.SetActive(true);
    }

    // Lose
    void GameLose()
    {
        isGameOver = true;
        if (losePanel != null)
            losePanel.SetActive(true);
    }
}
