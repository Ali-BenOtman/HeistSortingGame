// UIManager.cs
// Controls HUD elements - timer, score, multiplier - and the Game Over panel.
// NEW: shows/hides PauseButton at the right times - visible during actual
// play, hidden on Game Over or a deliberate quit back to the Main Menu.

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public GameObject pauseButton; // NEW

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
        if (pauseButton != null) pauseButton.SetActive(true); // NEW
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nScore: " + scoreSystem.GetScore();
        if (pauseButton != null) pauseButton.SetActive(false); // NEW
    }

    // NEW - wire to GameStateManager's On Quit To Menu () event.
    public void OnQuitToMenu()
    {
        if (pauseButton != null) pauseButton.SetActive(false);
    }
}