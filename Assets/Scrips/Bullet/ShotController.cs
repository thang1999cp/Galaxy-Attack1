using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShotController : MonoBehaviour
{
    [System.Serializable]
    public class ShotInfo
    {
        public GameObject shooterObject;
        public float delayAfterShot = 0.5f;
    }

    public List<ShotInfo> shotList = new List<ShotInfo>();
    public bool startOnAwake = true;
    public float startDelay = 1f;
    public bool loop = true;
    public bool randomOrder = false;
    public UnityEvent onFinished;

    private bool isShooting;
    private List<ShotInfo> workingList = new List<ShotInfo>();

    void Start()
    {
        if (startOnAwake)
        {
            StartCoroutine(StartRoutineAfterDelay());
        }
    }

    private IEnumerator StartRoutineAfterDelay()
    {
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);
        StartShooting();
    }

    public void StartShooting()
    {
        if (isShooting || shotList.Count == 0) return;

        workingList.Clear();
        workingList.AddRange(shotList);
        isShooting = true;
        StartCoroutine(ShootRoutine());
    }

    public void StopShooting()
    {
        StopAllCoroutines();
        isShooting = false;
    }

    private IEnumerator ShootRoutine()
    {
        int index = 0;

        while (isShooting)
        {
            if (workingList.Count == 0)
            {
                if (loop)
                {
                    workingList.AddRange(shotList);
                    index = 0;
                }
                else
                {
                    break;
                }
            }

            if (workingList.Count == 0) continue;

            if (randomOrder)
            {
                index = Random.Range(0, workingList.Count);
            }
            else
            {
                if (index >= workingList.Count)
                {
                    if (loop)
                    {
                        index = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var shotInfo = workingList[index];

            if (shotInfo.shooterObject != null)
            {
                IShooter shooter = shotInfo.shooterObject.GetComponent<IShooter>();
                if (shooter != null)
                {
                    shooter.Shoot();
                }
            }

            if (randomOrder)
            {
                workingList.RemoveAt(index);
            }
            else
            {
                index++;
            }

            yield return new WaitForSeconds(shotInfo.delayAfterShot);
        }

        isShooting = false;
        onFinished.Invoke();
    }
}