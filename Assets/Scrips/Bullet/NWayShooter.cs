using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NWayShooter : MonoBehaviour, IShooter
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public int bulletCount = 10;
    public float bulletSpeed = 5f;

    [Header("Shot Pattern")]
    public int wayCount = 5;
    [Range(0f, 360f)] public float centerAngle = 180f;
    [Range(0f, 360f)] public float betweenAngle = 10f;
    public float delayBetweenLines = 0.1f;

    private bool isShooting = false;

    public void Shoot()
    {
        if (!isShooting && bulletPrefab != null)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        if (bulletCount <= 0 || bulletSpeed <= 0f || wayCount <= 0)
        {
            Debug.LogWarning("Cannot shot because bulletCount or bulletSpeed or wayCount is not set.");
            yield break;
        }

        isShooting = true;
        int wayIndex = 0;

        for (int i = 0; i < bulletCount; i++)
        {
            if (wayIndex >= wayCount)
            {
                wayIndex = 0;
                if (delayBetweenLines > 0f)
                {
                    yield return new WaitForSeconds(delayBetweenLines);
                }
            }

            float totalSpread = (wayCount - 1) * betweenAngle;
            float startAngle = centerAngle - totalSpread / 2f;
            float angle = startAngle + wayIndex * betweenAngle;

            FireBullet(angle);
            wayIndex++;
        }

        isShooting = false;
    }
    private float GetShiftedAngle(int index, float baseAngle, float betweenAngle)
    {
        float halfWayCount = (wayCount - 1) / 2f;
        return baseAngle + (index - halfWayCount) * betweenAngle;
    }

    private void FireBullet(float angle)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}