using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationMoveBase : ScriptableObject
{
    public abstract Vector2 GetFormationOffset(Vector2 basePos, float time);
}