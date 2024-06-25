using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("スピード")] public float speed;
    [Header("最大移動距離")] public float maxDistance;
    [HideInInspector] public int direct;

    private Rigidbody2D rb = null;
    private Vector3 defaultPos;
    private string playerTag = "Player";
    private string groundTag = "Ground";
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("設定が足りません");
            Destroy(this.gameObject);
        }
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float d = Vector3.Distance(transform.position, defaultPos);

        //最大移動距離を超えている
        if (d > maxDistance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            rb.velocity = new Vector2(direct * speed, 0);
        }
    }

    /// <summary>
    /// 敵の向きに合わせて攻撃方向を設定
    /// </summary>
    public void Revolution(bool rightTleftF)
    {
        if(rightTleftF)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.tag == playerTag || collision.tag == groundTag)
        {
            Destroy(this.gameObject);
        }
	}
}
