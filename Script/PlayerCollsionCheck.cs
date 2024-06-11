using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollsionCheck : MonoBehaviour
{
    public Player player;
    [HideInInspector] public bool isHitArea = false;
    [HideInInspector] public bool isDeadArea = false;
    [HideInInspector] public Collision2D other = null;
    private string enemyTag = "Enemy";
    private string bossTag = "Boss";
    private string deadAreaTag = "DeadArea";
    private string hitAreaTag = "HitArea";
    private string moveFloorTag = "MoveFloor";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == enemyTag)
        {
            other = collision;
            player.EnemyCollision();
        }
        else if(collision.collider.tag == bossTag)
        {
            isHitArea = true;
        }
        else if(collision.collider.tag == moveFloorTag)
        {
            other = collision;
            player.MoveFloorCollision(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag)
        {
            player.MoveFloorCollision(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == deadAreaTag)
        {
            isDeadArea = true;
        }
        else if(collision.tag == hitAreaTag)
        {
            isHitArea = true;
        }
    }

    public void ResetFlag()
    {
        isDeadArea = false;
        isHitArea = false;
    }
}
