using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Formation/SideToSide")]
public class SideToSideFormation : FormationMoveBase
{
    public float amplitude = 0.5f;
    public float frequency = 2f;

    public override Vector2 GetFormationOffset(Vector2 basePos, float time)
    {
        float offsetX = Mathf.Sin(time * frequency) * amplitude;
        return basePos + new Vector2(offsetX, 0f);
    }
}