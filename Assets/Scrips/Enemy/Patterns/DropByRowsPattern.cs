using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Movement/DropByRows")]
public class DropByRowsPattern : PatternMoveBase
{
    public float dropInterval = 0.5f;    
    public float fallSpeed = 3f;    

    public override Vector2 GetNextPosition(Vector2 startPos, float time)
    {
        float dropDelay = startPos.y; 

        if (time < dropDelay)
        {
            return startPos;
        }

        float fallTime = time - dropDelay;
        float newY = startPos.y - fallSpeed * fallTime;

        return new Vector2(startPos.x, newY);
    }
}
