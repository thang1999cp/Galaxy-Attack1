using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Movement/Circle")]
public class CirclePattern : PatternMoveBase
{
    public float radius = 2f;
    public float angularSpeed = 90f;
    public float verticalSpeed = 1f;

    public override Vector2 GetNextPosition(Vector2 startPos, float time)
    {
        float angle = time * angularSpeed;
        float radian = angle * Mathf.Deg2Rad;

        float x = startPos.x + Mathf.Cos(radian) * radius;
        float y = startPos.y + Mathf.Sin(radian) * radius - verticalSpeed * time;

        return new Vector2(x, y);
    }
}