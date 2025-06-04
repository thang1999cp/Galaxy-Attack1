using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGunController : MonoBehaviour
{
    [Header("Shot Controllers theo cấp độ")]
    public List<ShotController> shotControllers;

    private int CurrentPowerIndex => Mathf.Clamp(GameContext.power - 1, 0, shotControllers.Count - 1);

    private void Start()
    {
        StartShot();
    }

    public void StartShot()
    {
        if (shotControllers.Count == 0) return;
        int index = CurrentPowerIndex;
        //Debug.Log($"Starting shot with index: {index}, ShotController: {shotControllers[index].name}");
        shotControllers[CurrentPowerIndex].StartShooting();
    }

    public void StopShot()
    {
        if (shotControllers.Count == 0) return;

        shotControllers[CurrentPowerIndex].StopShooting();
    }

    public void PowerUp()
    {
        if (GameContext.power >= shotControllers.Count)
            return;

        StopShot();
        GameContext.power++;
        StartShot();
    }

    public void PowerDown()
    {
        if (GameContext.power <= 1)
            return;

        StopShot();
        GameContext.power--;
        StartShot();
    }

    public void ShotFinishedRoutine()
    {
        UnityEngine.Debug.Log("Shot Finish Routine");
    }
}
public static class GameContext
{
    public static int power = 1;
}