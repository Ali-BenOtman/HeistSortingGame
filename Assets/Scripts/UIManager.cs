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

    [Tooltip("NEW - needed so the timer display can read the real, current cap " +
             "instead of a hardcoded number. Assign the same GameManager object " +
             "that holds your TimerSystem component.")]
    public TimerSystem timerSystem;

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
        // CHANGED: was normalizedTime * 15f (hardcoded, wrong the moment the real
        // cap isn't 15). Now reads the actual current cap from TimerSystem directly,
        // so the displayed number stays accurate even as DifficultyManager shrinks it.
        if (timerSystem == null)
        {
            // Fallback so this can't null-ref if the field isn't assigned yet -
            // just falls back to the old (inaccurate) behavior rather than crashing.
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
        startButton.SetActive(false);
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nScore: " + scoreSystem.GetScore();
        startButton.SetActive(true);
    }
}