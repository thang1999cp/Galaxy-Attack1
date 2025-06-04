using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Movement/ZigZag")]
public class ZigZagPattern : PatternMoveBase
{
    public float speed = 2f;
    public float frequency = 4f;
    public float amplitude = 2f;

    public override Vector2 GetNextPosition(Vector2 startPos, float time)
    {
        float x = startPos.x + Mathf.Sin(time * frequency) * amplitude;
        float y = startPos.y - speed * time;
        return new Vector2(x, y);
    }
}
