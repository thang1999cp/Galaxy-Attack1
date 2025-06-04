using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PatternMoveBase pattern;
    public FormationMoveBase formationMovement;
    public float patternDuration = 3f;
    public bool joinFormationAfterPattern = true;
    public Transform formationAnchor;
    public Vector2 formationOffset;

    private Vector2 startPos;
    private float timer;
    private bool inFormation = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!inFormation && timer < patternDuration)
        {
            Vector2 pos = pattern.GetNextPosition(startPos, timer);
            transform.position = pos;
        }
        else if (joinFormationAfterPattern && formationAnchor != null)
        {
            inFormation = true;
            Vector2 baseTarget = (Vector2)formationAnchor.position + formationOffset;

            if (formationMovement != null)
            {
                transform.position = formationMovement.GetFormationOffset(baseTarget, timer);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, baseTarget, Time.deltaTime * 2f);
            }
        }
    }

    void OnDestroy()
    {
        GameManager.OnEnemyDestroyed();
    }
}
