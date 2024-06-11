using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("踏みつけ判定の高さの割合(%)")] public float stepOnRate;
    [Header("攻撃間隔")] public float fireInterval;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("動く床の上にいるか")] public bool isOnMove;
    [Header("敵の当たり判定")] public EnemyCollisionCheck checkCollision;
    [Header("接触判定")] public EnemyTriggerCheck checkTrigger;
    [Header("攻撃オブジェクト")] public GameObject attackObj;
    [Header("やられた時に鳴らすSE")] public AudioClip downSE;
    [Header("攻撃する時に鳴らすSE")] public AudioClip attackSE;
    #endregion

    #region//プライベート変数    
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private BoxCollider2D[] boxcol = new BoxCollider2D[2];
    private SpriteRenderer sr = null;
    private ObjectCollision[] oc = new ObjectCollision[2];
    private MoveObject moveObj = null;
    private GameObject[] GObjs = new GameObject[100];
    private EnemyAttack[] attack = new EnemyAttack[100];
    private int xVector = 0;
    private int attackNumber = 0;
    private float walkTime = 0.0f;
    private bool isDown = false;
    private bool rightTleftF = false;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxcol = GetComponentsInChildren<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        oc = GetComponentsInChildren<ObjectCollision>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
        
        if(!oc[0].playerAttack)
        {
            if(sr.isVisible || nonVisibleAct)
            {
                walkTime += Time.deltaTime;

                //速度を求める
                float xSpeed = GetXSpeed();

                //通常の状態
                if(currentState.IsName("enemy_walk"))
                {
                    //攻撃する
                    if(walkTime >= fireInterval * Time.deltaTime)
                    {
                        anim.SetTrigger("attack");
                        walkTime = 0.0f;
                        GManager.instance.PlaySE(attackSE);
                    }
                }
                
                //移動速度を設定
                if(moveObj != null)
                {
                    rb.velocity = new Vector2(xSpeed, 0) + moveObj.GetVelocity();
                }
                else if(isOnMove)
                {
                    rb.velocity = new Vector2(xSpeed, -gravity - 5);
                }
                else
                {
                    rb.velocity = new Vector2(xSpeed, -gravity);
                }
            }
            else
            {
                walkTime = 0.0f;
                rb.Sleep();
            }
        }
        else
        {
            //ダウン時
            if(!isDown)
            {
                anim.Play("enemy_down");
                rb.velocity = new Vector2(0, -gravity);
                isDown = true;
                boxcol[0].enabled = false;
                boxcol[1].enabled = false;
                if(GManager.instance != null)
                {
                    GManager.instance.PlaySE(downSE);
                    GManager.instance.score += myScore;
                }
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        if(checkTrigger.isOn)
        {
            rightTleftF = !rightTleftF;
        }
        xVector = -1;
        if(rightTleftF)
        {
            xVector = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        return xVector * speed;
    }

    /// <summary>
    /// 攻撃時の処理
    /// </summary>
    private void Attack()
    {
        GObjs[attackNumber] = Instantiate(attackObj);
        GObjs[attackNumber].transform.position = attackObj.transform.position;
        GObjs[attackNumber].SetActive(true);
        attack[attackNumber] = GObjs[attackNumber].GetComponent<EnemyAttack>();
        attack[attackNumber].direct = xVector;
        attack[attackNumber].Revolution(rightTleftF);
        attackNumber++;
    }

    /// <summary>
    /// 動く床を踏んだ時の処理
    /// </summary>
    public void MoveFloorCollision(bool isMoveFloor)
    {
        if(isMoveFloor)
        {
            float stepOnHeight = boxcol[0].size.y * (stepOnRate / 100f);
            float judgePos = transform.position.y + stepOnHeight;

            foreach(ContactPoint2D p in checkCollision.other.contacts)
            {
                if(p.point.y < judgePos)
                {
                    moveObj = checkCollision.other.gameObject.GetComponent<MoveObject>();
                }
            }
        }
        else
        {
            moveObj = null;
        }
    }
}
