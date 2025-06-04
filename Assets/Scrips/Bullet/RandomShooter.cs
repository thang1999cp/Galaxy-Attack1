using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShooter : MonoBehaviour, IShooter
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public int bulletCount = 10;
    public float bulletSpeed = 5f;

    [Header("Random Shot Settings")]
    [Range(0f, 360f)]
    public float randomCenterAngle = 180f;

    [Range(0f, 360f)]
    public float randomRangeSize = 360f;

    public float randomSpeedMin = 1f;
    public float randomSpeedMax = 3f;

    public float randomDelayMin = 0.01f;
    public float randomDelayMax = 0.1f;

    public bool evenlyDistribute = true;

    private bool isShooting = false;

    [Header("Plane Settings")]
    [SerializeField] private int planeId;

    public void Shoot()
    {
        if (!isShooting && bulletPrefab != null)
        {
            StartCoroutine(ShotCoroutine());
        }
    }

    private IEnumerator ShotCoroutine()
    {
        if (bulletCount <= 0 || randomSpeedMin <= 0f || randomSpeedMax <= 0f)
        {
            Debug.LogWarning("Cannot shot because bulletCount or randomSpeedMin or randomSpeedMax is not set.");
            yield break;
        }

        if (isShooting)
        {
            yield break;
        }

        isShooting = true;

        List<int> numList = new List<int>(bulletCount);
        for (int i = 0; i < bulletCount; i++)
        {
            numList.Add(i);
        }

        while (numList.Count > 0)
        {
            int randomIndex = Random.Range(0, numList.Count);
            int bulletIndex = numList[randomIndex];

            float shootAngle = CalculateShootAngle(bulletIndex);
            float currentBulletSpeed = Random.Range(randomSpeedMin, randomSpeedMax);

            FireBullet(shootAngle, currentBulletSpeed);

            numList.RemoveAt(randomIndex);

            if (numList.Count > 0 && randomDelayMin >= 0f && randomDelayMax > 0f)
            {
                float waitTime = Random.Range(randomDelayMin, randomDelayMax);
                yield return new WaitForSeconds(waitTime);
            }
        }

        isShooting = false;
    }

    private float CalculateShootAngle(int bulletIndex)
    {
        float minAngle = randomCenterAngle - randomRangeSize / 2f;
        float maxAngle = randomCenterAngle + randomRangeSize / 2f;
        float angle;

        if (evenlyDistribute)
        {
            float segmentSize = Mathf.Floor((float)bulletCount / 4f);
            float segmentIndex = Mathf.Floor((float)bulletIndex / segmentSize);
            float anglePerSegment = Mathf.Abs(maxAngle - minAngle) / 4f;

            float segmentMinAngle = minAngle + anglePerSegment * segmentIndex;
            float segmentMaxAngle = minAngle + anglePerSegment * (segmentIndex + 1f);

            angle = Random.Range(segmentMinAngle, segmentMaxAngle);
        }
        else
        {

            angle = Random.Range(minAngle, maxAngle);
        }

        return angle;
    }

    private void FireBullet(float shootAngle, float speed)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        BulletManager bulletManager = bullet.GetComponent<BulletManager>();
        if (bulletManager != null)
        {
            bulletManager.planeId = planeId;
        }

        Vector2 direction = Quaternion.Euler(0, 0, shootAngle) * Vector2.up;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }
}
