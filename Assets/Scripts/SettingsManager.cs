// SettingsManager.cs
// Opens/closes the Settings panel, connects the mute toggle to AudioManager,
// and does a REAL full pause via Time.timeScale = 0 - this freezes everything
// using Time.deltaTime or WaitForSeconds automatically (TimerSystem, ScoreSystem's
// decay, PowerUpManager's spawn timer, any active power-up coroutine) in one move.
// SwipeDetector/TapDetector are disabled separately, since they read raw input
// rather than time-based logic and wouldn't stop on their own.

using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    public GameObject settingsPanel;
    public Toggle muteToggle;
    public SwipeDetector swipeDetector;
    public TapDetector tapDetector;

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        if (muteToggle != null && AudioManager.Instance != null)
            muteToggle.isOn = AudioManager.Instance.IsMuted();

        Time.timeScale = 0f;

        if (swipeDetector != null) swipeDetector.enabled = false;
        if (tapDetector != null) tapDetector.enabled = false;
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Time.timeScale = 1f;

        if (swipeDetector != null) swipeDetector.enabled = true;
        if (tapDetector != null) tapDetector.enabled = true;
    }

    public void OnMuteToggleChanged(bool isOn)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMuted(isOn);
    }
}