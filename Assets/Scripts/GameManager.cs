using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Enemy Tracking")]
    public int totalEnemies;
    public int enemiesRemaining;

    [Header("Score")]
    public int killScore = 0;
    public int finalScore = 0;

    [Header("Time")]
    public float elapsedTime = 0f;
    public bool timerRunning = true;

    [Header("State")]
    public bool levelCompleted = false;
    public bool playerDied = false;
    public bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        EnemyHealth[] existingEnemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
        ArenaWaveManager[] waveManagers = FindObjectsByType<ArenaWaveManager>(FindObjectsSortMode.None);

        totalEnemies = existingEnemies.Length;

        foreach (ArenaWaveManager waveManager in waveManagers)
        {
            totalEnemies += waveManager.GetTotalPlannedEnemies();
        }

        enemiesRemaining = totalEnemies;

        Debug.Log("Level started. Total enemies including future waves: " + totalEnemies);
    }

    void Update()
    {
        if (!gameEnded && timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void EnemyDefeated(int points)
    {
        if (gameEnded) return;

        killScore += points;
        enemiesRemaining--;

        Debug.Log("Enemy defeated! Kill Score: " + killScore);
        Debug.Log("Enemies remaining: " + enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        if (gameEnded) return;

        levelCompleted = true;
        gameEnded = true;
        timerRunning = false;

        int timeBonus = Mathf.Max(0, 1000 - Mathf.RoundToInt(elapsedTime * 10));
        finalScore = killScore + timeBonus;

        Debug.Log("LEVEL COMPLETE!");
        Debug.Log("Time: " + elapsedTime.ToString("F2") + " seconds");
        Debug.Log("Time Bonus: " + timeBonus);
        Debug.Log("Final Score: " + finalScore);
    }

    public void PlayerDied()
    {
        if (gameEnded) return;

        playerDied = true;
        gameEnded = true;
        timerRunning = false;

        int timeBonus = 0;
        finalScore = killScore + timeBonus;

        Debug.Log("GAME OVER!");
        Debug.Log("Time: " + elapsedTime.ToString("F2") + " seconds");
        Debug.Log("Final Score: " + finalScore);
    }

    public int GetKillScore()
    {
        return killScore;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public int GetFinalScore()
    {
        return finalScore;
    }
}