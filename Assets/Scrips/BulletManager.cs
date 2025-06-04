
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private int _planeId;
    public int planeId
    {
        get => _planeId;
        set
        {
            _planeId = value;
            SetPowerFromPlaneManager();
        }
    }

    [SerializeField]private int power;
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
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(power);
                }
                DespawnBullet();
            }
            
        }
    }

	public virtual void DespawnBullet()
	{
		this.isTriggerEnemy = false;
        Destroy(gameObject);
	}

    private void SetPowerFromPlaneManager()
    {
        Plane plane = PlaneManager.Instance.GetPlaneById(planeId);
        if (plane != null)
        {
            power = plane.power;
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy máy bay với ID: {planeId}");
        }
    }
}
