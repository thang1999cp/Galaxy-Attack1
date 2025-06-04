using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternMoveBase : ScriptableObject
{
    public abstract Vector2 GetNextPosition(Vector2 startPos, float time);
}
