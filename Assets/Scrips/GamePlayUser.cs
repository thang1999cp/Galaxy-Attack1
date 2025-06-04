using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlayUser : MonoBehaviour
{
    private void OnMouseDrag()
    {
        Vector3 mouPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouPos.z = 0;
        transform.position = mouPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            UIManager.instance.ShowOnlyPanel(UIManager.instance.loseUI);
        }
    }


}
