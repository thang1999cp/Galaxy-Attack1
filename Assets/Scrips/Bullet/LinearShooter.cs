using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearShooter : MonoBehaviour, IShooter
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public int bulletCount = 5;
    public float bulletSpeed = 5f;

    [Header("Linear Shot Settings")]
    [Range(0f, 360f)]
    public float angle = 180f;
    public float betweenDelay = 0.1f;

    private bool isShooting = false;

    public void Shoot()
    {
        if (!isShooting && bulletPrefab != null)
        {
            StartCoroutine(ShotCoroutine());
        }
    }

    private IEnumerator ShotCoroutine()
    {
        if (bulletCount <= 0 || bulletSpeed <= 0f)
        {
            Debug.LogWarning("Cannot shot because bulletCount or bulletSpeed is not set.");
            yield break;
        }

        if (isShooting)
        {
            yield break;
        }

        isShooting = true;

        for (int i = 0; i < bulletCount; i++)
        {
            if (i > 0 && betweenDelay > 0f)
            {
                yield return new WaitForSeconds(betweenDelay);
            }

            FireBullet(angle);
        }

        isShooting = false;
    }

    private void FireBullet(float shootAngle)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = Quaternion.Euler(0, 0, shootAngle) * Vector2.up;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}
