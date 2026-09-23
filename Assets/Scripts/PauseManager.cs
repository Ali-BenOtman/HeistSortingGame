// PauseManager.cs
// Owns the actual pause mechanism (Time.timeScale + input blocking) for the
// whole game now. SettingsManager no longer touches any of this - it can be
// opened from here without conflicting over who controls the pause state.

using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseButton;
    public GameObject pausePanel;
    public SwipeDetector swipeDetector;
    public TapDetector tapDetector;
    public GameStateManager gameStateManager;

    public void OpenPause()
    {
        if (pausePanel != null) pausePanel.SetActive(true);

        Time.timeScale = 0f;
        if (swipeDetector != null) swipeDetector.enabled = false;
        if (tapDetector != null) tapDetector.enabled = false;
    }

    // Wire to ResumeButton's On Click ().
    public void ClosePause()
    {
        if (pausePanel != null) pausePanel.SetActive(false);

        Time.timeScale = 1f;
        if (swipeDetector != null) swipeDetector.enabled = true;
        if (tapDetector != null) tapDetector.enabled = true;
    }

    // Wire to QuitButton's On Click ().
    public void QuitToMenu()
    {
        if (pausePanel != null) pausePanel.SetActive(false);

        Time.timeScale = 1f;
        if (swipeDetector != null) swipeDetector.enabled = true;
        if (tapDetector != null) tapDetector.enabled = true;

        if (gameStateManager != null)
            gameStateManager.QuitToMenu();
    }
}