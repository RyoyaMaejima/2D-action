using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public bool isEnemy = false;
    [HideInInspector] public ObjectCollision o = null;

    private string enemyTag = "Enemy";

    //攻撃が敵に当たったか判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == enemyTag)
        {
            isEnemy = true;
            o = collision.gameObject.GetComponent<ObjectCollision>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == enemyTag)
        {
            isEnemy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == enemyTag)
        {
            isEnemy = false;
        }
    }
}
