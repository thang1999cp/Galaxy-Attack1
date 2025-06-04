using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemyPrefab;
        public PatternMoveBase pattern;
        //public FormationMoveBase formationMovement;
        public Vector2[] formationOffsets;

    }

    [System.Serializable]
    public class WaveData
    {
        public EnemyData[] enemies;
    }

    public Transform formationAnchor;
    public Transform spawnPoint;
    public WaveData[] waves;
    public float delayBetweenWaves = 2f;

    private static int aliveEnemies = 0;
    private int currentWave = 0;
    private bool spawning = false;
    private bool waveIsOn = false;

    private void Start()
    {
        //StartCoroutine(nameof(ShowWaveNotification));
    }

    public static void OnEnemyDestroyed()
    {
        aliveEnemies--;
    }

    void Update()
    {
        if (!spawning && aliveEnemies == 0 && currentWave < waves.Length)
        {
            StartCoroutine(SpawnWave());
            waveIsOn = false;
        }
    }

    IEnumerator SpawnWave()
    {
        spawning = true;

        WaveData wave = waves[currentWave];


        UIManager.instance.ShowWaveUI(currentWave + 1, waves.Length);
        yield return new WaitForSeconds(2);
        UIManager.instance.HideWaveUI();


        foreach (EnemyData enemyData in wave.enemies)
        {
            if (enemyData.enemyPrefab == null)
            {
                Debug.LogError("Enemy prefab is missing!");
                continue;
            }

            if (enemyData.pattern == null)
            {
                Debug.LogError("Enemy pattern is missing!");
                continue;
            }

            foreach (Vector2 offset in enemyData.formationOffsets)
            {
                GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity);

                EnemyManager movement = enemy.GetComponent<EnemyManager>();

                if (movement == null)
                {
                    Debug.LogError("Enemy prefab is missing EnemyMovementController!");
                    continue;
                }

                movement.pattern = enemyData.pattern;
                movement.formationAnchor = formationAnchor;
                movement.formationOffset = offset;

                aliveEnemies++;
                yield return new WaitForSeconds(0.2f);
            }
        }

        currentWave++;
        yield return new WaitForSeconds(delayBetweenWaves);
        spawning = false;
    }

    //IEnumerator ShowWaveNotification()
    //{
    //    UIManager.instance.ShowWaveUI(currentWave + 1, waves.Length);
    //    yield return new WaitForSeconds(2);
    //    UIManager.instance.HideWaveUI();
    //    StartCoroutine(nameof(SpawnWave));
    //    waveIsOn = true;
    //}
}
