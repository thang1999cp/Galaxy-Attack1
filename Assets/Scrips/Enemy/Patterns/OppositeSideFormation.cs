using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Formation/OppositeSideToSide")]
public class OppositeSideFormation : FormationMoveBase
{
    public float amplitude = 0.5f;
    public float frequency = 2f;
    public bool reversePhase = false;

    public override Vector2 GetFormationOffset(Vector2 basePos, float time)
    {
        float phase = reversePhase ? Mathf.PI : 0f;
        float offsetX = Mathf.Sin(time * frequency + phase) * amplitude;
        return basePos + new Vector2(offsetX, 0f);
    }
}
