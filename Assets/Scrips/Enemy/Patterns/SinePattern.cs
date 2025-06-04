using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Movement/Sine")]
public class SinePattern : PatternMoveBase
{
    public float speed = 2f;
    public float frequency = 2f;
    public float amplitude = 1f;

    public override Vector2 GetNextPosition(Vector2 startPos, float time)
    {
        float maxFall = 9f;
        float yOffset = Mathf.Min(speed * time, maxFall);
        return new Vector2(
            startPos.x + Mathf.Sin(time * frequency) * amplitude,
            startPos.y - yOffset
        );
    }
}
