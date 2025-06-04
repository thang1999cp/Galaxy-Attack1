using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int HP = 10;
    public int coinValue = 5;

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
            UIManager.instance.Coin += coinValue;
            UIManager.instance.UpdateCoinUI();
        }
    }

}
