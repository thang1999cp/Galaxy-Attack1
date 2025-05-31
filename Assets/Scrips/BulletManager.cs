
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{


    public float power;

    public bool needSetPower = true;

    private bool isTriggerEnemy;


    private void Update()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
        {
            DespawnBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("Enemy"))
        {
            if (!isTriggerEnemy)
            {
                isTriggerEnemy = true;
                TakeDamageEnemy(collision.gameObject);
            }
            
        }
    }

	public virtual void TakeDamageEnemy(GameObject objEnemy)
	{
		DespawnBullet();
	}


	public virtual void DespawnBullet()
	{
		this.isTriggerEnemy = false;
        Destroy(gameObject);
	}

}
