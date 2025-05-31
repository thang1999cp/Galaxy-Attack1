using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlayUser : MonoBehaviour
{
    public GameObject bulletSpawm;
    public Transform spawnPoint;
    public float bulletSpeed = 10f;
    public int numRowsBullet = 1;
    public float rowSpacing = 0.1f;

    Rigidbody2D rb;

    private void Start()
    {
        StartCoroutine(nameof(SpawnBullet));
    }
    private void OnMouseDrag()
    {
        Vector3 mouPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouPos.z = 0;
        transform.position = mouPos;
    }


    private void ShotBullet()
    {
        for (int i = 0; i < numRowsBullet; i++)
        {
            Vector3 offset = new Vector3((i - (numRowsBullet - 1) / 2f) * rowSpacing, 0, 0);


            GameObject bullet = Instantiate(bulletSpawm, spawnPoint.position + offset, spawnPoint.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = transform.up * bulletSpeed;
            }
        }
    }


    IEnumerator SpawnBullet()
    {
        while (true)
        {
            ShotBullet();
            yield return new WaitForSecondsRealtime(2f);
        }
    }
}
