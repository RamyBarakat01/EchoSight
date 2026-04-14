using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWaveManager : MonoBehaviour
{
    [Header("Bat Prefab")]
    public GameObject batPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public float timeBetweenSpawns = 0.5f;
    public float timeBetweenWaves = 2f;

    [Header("Wave Counts")]
    public int wave1Count = 2;
    public int wave2Count = 3;
    public int wave3Count = 5;

    private bool fightStarted = false;
    private bool fightFinished = false;

    private List<GameObject> aliveBats = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!fightStarted && collision.CompareTag("Player"))
        {
            fightStarted = true;
            StartCoroutine(StartWaves());
        }
    }

    IEnumerator StartWaves()
    {
        yield return StartCoroutine(SpawnWave(wave1Count));
        yield return StartCoroutine(WaitUntilWaveCleared());

        yield return new WaitForSeconds(timeBetweenWaves);

        yield return StartCoroutine(SpawnWave(wave2Count));
        yield return StartCoroutine(WaitUntilWaveCleared());

        yield return new WaitForSeconds(timeBetweenWaves);

        yield return StartCoroutine(SpawnWave(wave3Count));
        yield return StartCoroutine(WaitUntilWaveCleared());

        fightFinished = true;
        Debug.Log("Arena fight completed!");
    }

    IEnumerator SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnBat();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    void SpawnBat()
    {
        if (batPrefab == null || spawnPoints.Length == 0)
            return;

        Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newBat = Instantiate(batPrefab, chosenSpawnPoint.position, Quaternion.identity);

        aliveBats.Add(newBat);
    }

    IEnumerator WaitUntilWaveCleared()
    {
        while (true)
        {
            aliveBats.RemoveAll(bat => bat == null);

            if (aliveBats.Count == 0)
                yield break;

            yield return null;
        }
    }

    public int GetTotalPlannedEnemies()
    {
        return wave1Count + wave2Count + wave3Count;
    }

    public bool IsFightFinished()
    {
        return fightFinished;
    }
}