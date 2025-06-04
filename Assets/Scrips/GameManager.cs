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
        public Vector2[] formationOffsets;

        public int enemiesPerRow = 2;

        public float patternHorizontalSpacing = 2f;

        public float patternVerticalSpacing = 2f;

        public bool centerFormation = true;
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
        }
        if (!spawning && aliveEnemies == 0 && currentWave >= waves.Length)
        {
            ShowWinUI();
        }
    }

    private Vector2 CalculatePatternOffset(int enemyIndex, EnemyData enemyData)
    {
        int row = enemyIndex / enemyData.enemiesPerRow;
        int col = enemyIndex % enemyData.enemiesPerRow;

        float x = col * enemyData.patternHorizontalSpacing;
        float y = -row * enemyData.patternVerticalSpacing;

        if (enemyData.centerFormation)
        {
            int totalEnemies = enemyData.formationOffsets.Length;
            int totalRows = Mathf.CeilToInt((float)totalEnemies / enemyData.enemiesPerRow);
            int enemiesInCurrentRow = (row == totalRows - 1)
                ? (totalEnemies - row * enemyData.enemiesPerRow)
                : enemyData.enemiesPerRow;

            float totalWidth = (enemiesInCurrentRow - 1) * enemyData.patternHorizontalSpacing;
            float totalHeight = (totalRows - 1) * enemyData.patternVerticalSpacing;

            x -= totalWidth * 0.5f;
            y += totalHeight * 0.5f;
        }

        return new Vector2(x, y);
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
            if (enemyData.enemyPrefab == null || enemyData.pattern == null)
            {
                Debug.LogError("Enemy prefab hoặc pattern bị thiếu!");
                continue;
            }

            for (int i = 0; i < enemyData.formationOffsets.Length; i++)
            {
                GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity);
                EnemyManager movement = enemy.GetComponent<EnemyManager>();

                if (movement == null)
                {
                    Debug.LogError("Enemy prefab thiếu component EnemyManager!");
                    continue;
                }

                Vector2 patternOffset = (enemyData.pattern is DropByRowsPattern)   ? Vector2.zero   : CalculatePatternOffset(i, enemyData);

                movement.pattern = enemyData.pattern;
                movement.formationAnchor = formationAnchor;
                movement.formationOffset = enemyData.formationOffsets[i];
                movement.patternOffset = patternOffset;

                aliveEnemies++;
                yield return new WaitForSeconds(0.2f);
            }
        }

        currentWave++;
        yield return new WaitForSeconds(delayBetweenWaves);
        spawning = false;
    }

    private void ShowWinUI()
    {
        Debug.Log("Bạn đã chiến thắng!");
        UIManager.instance.winUI.SetActive(true);
    }

}
