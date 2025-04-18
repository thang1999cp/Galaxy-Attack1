using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovementType
{
    Straight,
    ZigZag,
    SineWave
}


public class EnemyManager : MonoBehaviour
{
    private float speedEnemy = 5f;
    public EnemyMovementType movementType;
    private float zigzagFrequency = 5f;
    private float zigzagMagnitude = 0.5f;
    private float startX;

    private void Start()
    {
        startX = transform.position.x;
    }
    private void Update()
    {
        switch (movementType)
        {
            case EnemyMovementType.Straight:
                transform.Translate(Vector3.down * speedEnemy * Time.deltaTime);
                break;

            case EnemyMovementType.ZigZag:
                float zigzagX = Mathf.Sin(Time.time * zigzagFrequency) * zigzagMagnitude;
                transform.Translate(new Vector3(zigzagX, -1, 0) * speedEnemy * Time.deltaTime);
                break;

            case EnemyMovementType.SineWave:
                float sineX = Mathf.Sin(Time.time * 2f) * 1.5f;
                transform.position += new Vector3(sineX * Time.deltaTime, -speedEnemy * Time.deltaTime, 0);
                break;
        }

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    } 
}
