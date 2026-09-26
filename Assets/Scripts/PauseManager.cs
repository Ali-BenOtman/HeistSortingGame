// PauseManager.cs
// Owns the actual pause mechanism (Time.timeScale + input blocking) for the
// whole game. SettingsManager doesn't touch any of this - it can be opened
// from here without conflicting over who controls the pause state.
//
// NEW: OnApplicationPause() automatically triggers a real pause whenever the
// app loses focus mid-run (call, notification, home button, screen lock) -
// without this, the timer would just keep draining in the background while
// the player isn't even looking at the screen. Deliberately does NOT auto-
// resume when focus returns - the player has to tap Resume themselves.

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

    // NEW - Unity calls this automatically on mobile whenever the app loses
    // or regains focus. pauseStatus is true when losing focus.
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && gameStateManager != null && gameStateManager.IsPlaying())
        {
            OpenPause();
        }
        // Deliberately no action when pauseStatus is false (regaining focus) -
        // the player resumes manually via ResumeButton, never automatically.
    }
}