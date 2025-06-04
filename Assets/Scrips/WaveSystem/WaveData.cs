using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public GameObject enemyPrefab;
    public PatternMoveBase pattern;
    public Vector2[] formationOffsets;
}