// MainMenuManager.cs
// Shows/hides the Main Menu panel and keeps its High Score text current.
// Visible by default at boot (Idle state); hidden once a game starts.

using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("References")]
    public GameObject mainMenuPanel;
    public TextMeshProUGUI highScoreText;
    public ScoreSystem scoreSystem;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        UpdateHighScoreText();
    }

    // Wire this to GameStateManager's onGameStart event, alongside UIManager.OnGameStart.
    public void HideMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }

    void UpdateHighScoreText()
    {
        if (highScoreText != null && scoreSystem != null)
            highScoreText.text = "HIGH SCORE: " + scoreSystem.GetHighScore();
    }
}