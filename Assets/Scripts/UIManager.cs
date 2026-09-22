// UIManager.cs
// Controls HUD elements - timer, score, multiplier - and the Game Over panel.
// Main Menu is now handled separately by MainMenuManager; the old shared
// startButton is gone, replaced by MainMenuPanel's PlayButton (first launch)
// and GameOverPanel's own dedicated PlayAgainButton (instant retry).

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;

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
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nScore: " + scoreSystem.GetScore();
    }
}