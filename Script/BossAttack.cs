using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("ボス")] public Boss boss;
    [Header("最大半径")] public float maxRadius;
    [Header("最小半径")] public float minRadius;
    [HideInInspector] public int attackNumber = 0;

    private Rigidbody2D rb = null;
    private bool isSmall = false;
    private float radius = 0.0f;
    private float kakudo = 0.0f;
    private string playerTag = "Player";
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("設定が足りません");
            Destroy(this.gameObject);
        }
        radius = minRadius;
        kakudo = 45f * (float)attackNumber;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //回転半径が時間ごとに変化する
        if(isSmall)
        {
            radius -= 0.05f;
        }
        else
        {
            radius += 0.05f;
        }

        if(radius < minRadius && isSmall)
        {
            isSmall = false;
        }
        else if(radius > maxRadius && !isSmall)
        {
            isSmall = true;
        }

        //回転角度の設定
        kakudo += 1f;
        if(kakudo >= 360f)
        {
            kakudo = 0f;
        }

        //位置の設定
        float locX = radius * Mathf.Cos(kakudo * Mathf.Deg2Rad);
        float locY = radius * Mathf.Sin(kakudo * Mathf.Deg2Rad);
        transform.position = boss.transform.position + new Vector3(locX, locY, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.tag == playerTag)
        {
            Destroy(this.gameObject);
        }
	}
}
