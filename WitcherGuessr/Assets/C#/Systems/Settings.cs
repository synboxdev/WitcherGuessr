public static class Settings
{
    private static int ImageViewSensitivity;
    private static int MapZoomSensitivity;
    private static int GuessAttempts;

    private static bool LocationLooping;
    private static bool EnableHints;
    private static bool LocationPreloading;

    static Settings() => InitializeSettings();

    public static void InitializeSettings()
    {
        ImageViewSensitivity = 3;
        MapZoomSensitivity = 4;
        GuessAttempts = 5;
        LocationLooping = false;
        EnableHints = false;
        LocationPreloading = true;
    }

    public static void SetImageViewSensitivity(float value) => ImageViewSensitivity = (int)value;
    public static int GetImageViewSensitivity => ImageViewSensitivity;

    public static void SetMapZoomSensitivity(float value) => MapZoomSensitivity = (int)value;
    public static int GetMapZoomSensitivity => MapZoomSensitivity;

    public static void SetGuessAttempts(float value) => GuessAttempts = (int)value;
    public static int GetGuessAttempts => GuessAttempts;

    public static void SetLocationLoopingToggle() => LocationLooping = !LocationLooping;

    public static bool GetLocationLoopingToggle => LocationLooping;

    public static void SetEnableHintsToggle() => EnableHints = !EnableHints;
    public static bool GetEnableHintsToggle => EnableHints;

    public static void SetEnableLocationPreloading() => LocationPreloading = !LocationPreloading;
    public static bool GetEnableLocationPreloading => LocationPreloading;
}