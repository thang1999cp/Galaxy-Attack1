using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefabs;

    public float leftPos;
    public float rightPos;
    public float hightPos;

    private int waveNumber = 0;

    void Start()
    {
        StartCoroutine(WaveSpawner());
    }

    IEnumerator WaveSpawner()
    {
        while (true)
        {
            waveNumber++;
            //Debug.Log("Wave " + waveNumber);

            for (int i = 0; i < 5 + waveNumber * 2; i++)
            {
                SpawnEnemyByWave(waveNumber);
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(3f);
        }
    }

    void SpawnEnemyByWave(int wave)
    {
        Vector3 spawnPos = new Vector3(Random.Range(leftPos, rightPos), hightPos);
        GameObject enemy = Instantiate(enemyPrefabs, spawnPos, Quaternion.identity);

        EnemyManager controller = enemy.GetComponent<EnemyManager>();

        if (wave == 1)
            controller.movementType = EnemyMovementType.Straight;
        else if (wave == 2)
            controller.movementType = EnemyMovementType.ZigZag;
        else
            controller.movementType = (Random.value > 0.5f) ? EnemyMovementType.SineWave : EnemyMovementType.ZigZag;
    }
}
