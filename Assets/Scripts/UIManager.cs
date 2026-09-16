// UIManager.cs
// Controls all UI elements - timer, score, multiplier, start and game over screens

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
    public GameObject startButton;

    [Header("References")]
    public ScoreSystem scoreSystem;

    void Start()
    {
        gameOverPanel.SetActive(false);
        timerText.text = "TIME: 0";
        scoreText.text = "SCORE: 0";
        multiplierText.text = "x1";
        startButton.SetActive(true);
    }

    public void UpdateTimer(float normalizedTime)
    {
        timerText.text = "TIME: " + Mathf.CeilToInt(normalizedTime * 15f);
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
        startButton.SetActive(false);
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nScore: " + scoreSystem.GetScore();
        startButton.SetActive(true);
    }
}