using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionCheck : MonoBehaviour
{
    public Enemy enemy;
    [HideInInspector] public Collision2D other = null;
    private string moveFloorTag = "MoveFloor";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag)
        {
            other = collision;
            enemy.MoveFloorCollision(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag)
        {
            enemy.MoveFloorCollision(false);
        }
    }
}
