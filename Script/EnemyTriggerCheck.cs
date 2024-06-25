using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerCheck : MonoBehaviour
{
    /// <summary>
	/// 判定内にプレイヤーがいる
	/// </summary>
    [HideInInspector] public bool isOn = false;

    private string groundTag = "Ground";
    private string enemyTag = "Enemy";
    private string reflectTag = "Reflect";

    //壁や敵との接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == groundTag || collision.tag == enemyTag || collision.tag == reflectTag)
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == groundTag || collision.tag == enemyTag || collision.tag == reflectTag)
        {
            isOn = false;
        }
    }
}
