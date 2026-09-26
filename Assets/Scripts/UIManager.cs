// UIManager.cs
// Controls HUD elements - timer, score, multiplier - and the Game Over panel.
//
// CHANGED: OnGameOver() now shows the high score alongside the run's score,
// with a "NEW HIGH SCORE!" callout when earned. This relies on
// ScoreSystem.StopPlaying() having already run and updated the high score
// BEFORE this fires - GameStateManager's On Game Over () event list must
// have ScoreSystem.StopPlaying listed ABOVE UIManager.OnGameOver.

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public GameObject pauseButton;

    [Header("Screens")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    [Header("References")]
    public ScoreSystem scoreSystem;
    public TimerSystem timerSystem;

    void Start()
    {
        gameOverPanel.SetActive(false);
        timerText.text = "TIME: 0";
        scoreText.text = "SCORE: 0";
        multiplierText.text = "x1";
    }

    public void UpdateTimer(float normalizedTime)
    {
        if (timerSystem == null)
        {
            timerText.text = "TIME: " + Mathf.CeilToInt(normalizedTime * 15f);
            return;
        }

        float realSeconds = normalizedTime * timerSystem.GetCurrentMaxCap();
        timerText.text = "TIME: " + Mathf.CeilToInt(realSeconds);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "SCORE: " + score;
    }

    public void UpdateMultiplier(int multiplier)
    {
        multiplierText.text = "x" + multiplier;
    }

    public void OnGameStart()
    {
        gameOverPanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);

        int finalScore = scoreSystem.GetScore();
        int highScore = scoreSystem.GetHighScore();

        // Requires ScoreSystem.StopPlaying() to have already run - see note above.
        string highScoreLine = (finalScore >= highScore)
            ? "NEW HIGH SCORE!"
            : "High Score: " + highScore;

        gameOverText.text = "GAME OVER\nScore: " + finalScore + "\n" + highScoreLine;

        if (pauseButton != null) pauseButton.SetActive(false);
    }

    public void OnQuitToMenu()
    {
        if (pauseButton != null) pauseButton.SetActive(false);
    }
}