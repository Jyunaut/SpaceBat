public static class GlobalEvents
{
    public delegate void LevelStart();
    public static event LevelStart OnLevelStart;
    public static void StartLevel() => OnLevelStart?.Invoke();

    public delegate void ReachedEndOfLevel();
    public static event ReachedEndOfLevel OnReachedEndOfLevel;
    public static void EndOfLevelReached() => OnReachedEndOfLevel?.Invoke();

    public delegate void LevelComplete();
    public static event LevelComplete OnLevelComplete;
    public static void EndLevel() => OnLevelComplete?.Invoke();
}