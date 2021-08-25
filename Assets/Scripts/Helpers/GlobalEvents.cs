public struct GlobalEvents
{
#region Player Events
    public delegate void PlayerSpawn();
    public static event PlayerSpawn OnPlayerSpawn;
    public static void PlayerSpawns() => OnPlayerSpawn?.Invoke();

    public delegate void PlayerDead();
    public static event PlayerDead OnPlayerDead;
    public static void PlayerDies() => OnPlayerDead?.Invoke();
#endregion
#region Level Events
    public delegate void LevelStart();
    public static event LevelStart OnLevelStart;
    public static void StartLevel() => OnLevelStart?.Invoke();

    public delegate void ReachedEndOfLevel();
    public static event ReachedEndOfLevel OnReachedEndOfLevel;
    public static void EndOfLevelReached() => OnReachedEndOfLevel?.Invoke();

    public delegate void LevelComplete();
    public static event LevelComplete OnLevelComplete;
    public static void CompleteLevel() => OnLevelComplete?.Invoke();
#endregion
}