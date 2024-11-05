using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void ResetToDefault() => Settings.InitializeSettings();

    public void SetImageViewSensitivity(float value) => Settings.SetImageViewSensitivity(value);
    public int GetImageViewSensitivity() => Settings.GetImageViewSensitivity;

    public void SetMapZoomSensitivity(float value) => Settings.SetMapZoomSensitivity(value);
    public int GetMapZoomSensitivity() => Settings.GetMapZoomSensitivity;

    public void SetGuessAttempts(float value) => Settings.SetGuessAttempts(value);
    public int GetGuessAttempts() => Settings.GetGuessAttempts;

    public void SetLocationLoopingToggle() => Settings.SetLocationLoopingToggle();
    public bool GetLocationLoopingToggle() => Settings.GetLocationLoopingToggle;

    public void SetEnableHintsToggle() => Settings.SetEnableHintsToggle();
    public bool GetEnableHintsToggle() => Settings.GetEnableHintsToggle;

    public void SetEnableLocationPreloading() => Settings.SetEnableLocationPreloading();
    public bool GetEnableLocationPreloading() => Settings.GetEnableLocationPreloading;
}