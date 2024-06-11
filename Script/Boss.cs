using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("攻撃間隔")] public float fireInterval;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
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
    private GameObject[] GObjs = new GameObject[8];
    private BossAttack[] attack = new BossAttack[8];
    private int xVector = 0;
    private int damageNumber = 0;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private float walkTime = 0.0f;
    private bool isAttack = false;
    private bool isContinue = false;
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

    private void Update()
    {
        if(isContinue)
        {
            //明滅　ついている時に戻る
            if(blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えているとき
            else if(blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            //明滅　ついているとき
            else
            {
                sr.enabled = true;
            }
            //1秒たったら明滅終わり
            if(continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;
                oc[0].playerAttack = false;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        if(!isContinue && !isDown)
        {
            if(sr.isVisible || nonVisibleAct)
            {
                walkTime += Time.deltaTime;

                //速度を求める
                float xSpeed = GetXSpeed();

                //攻撃する
                if(!isAttack)
                {
                    Attack();
                    isAttack = true;
                }

                //攻撃音を鳴らす
                if(walkTime >= fireInterval * Time.deltaTime)
                {
                    walkTime = 0.0f;
                    GManager.instance.PlaySE(attackSE);
                }

                //移動速度を設定
                rb.velocity = new Vector2(xSpeed, 0);
            }
            else
            {
                rb.Sleep();
            }
        }

        //コンティニュー時
        if(isContinue)
        {
            rb.velocity = Vector2.zero;
        }

        if(oc[0].playerAttack)
        {
            //ダメージ時
            ReceiveDamage();

            //ダウン時
            if(damageNumber >= 3)
            {
                if(!isDown)
                {
                    rb.velocity = new Vector2(0, -gravity);
                    boxcol[0].enabled = false;
                    boxcol[1].enabled = false;
                    isDown = true;
                    if(GManager.instance != null)
                    {
                        GManager.instance.PlaySE(downSE);
                        GManager.instance.score += myScore;
                    }
                    Destroy(gameObject, 5f);
                }
                else
                {
                    transform.Rotate(new Vector3(0, 0, 5));
                }
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
            transform.localScale = new Vector3(7, 7, 1);
        }
        else
        {
            transform.localScale = new Vector3(-7, 7, 1);
        }
        return xVector * speed;
    }

    /// <summary>
    /// 攻撃時の処理
    /// </summary>
    private void Attack()
    {
        for(int i = 0; i < 8; i++)
        {
            GObjs[i] = Instantiate(attackObj);
            GObjs[i].transform.position = attackObj.transform.position;
            GObjs[i].SetActive(true);
            attack[i] = GObjs[i].GetComponent<BossAttack>();
            float radius = attack[i].minRadius;
            float locX = radius * Mathf.Cos(45f * (float)i * Mathf.Deg2Rad);
            float locY = radius * Mathf.Sin(45f * (float)i * Mathf.Deg2Rad);
            GObjs[i].transform.position += new Vector3(locX, locY, 0);
            attack[i].attackNumber = i;
        }
    }

    /// <summary>
    /// 攻撃オブジェクトの削除
    /// </summary>
    private void DestoryAttack()
    {
        for(int i = 0; i < 8; i++)
        {
            Destroy(GObjs[i]);
        }
        isAttack = false;
    }

    /// <summary>
    /// やられた時の処理
    /// </summary>
    private void ReceiveDamage()
    {
        if(isDown || isContinue)
        {
            return;
        }
        else
        {
            if(damageNumber < 2)
            {
                damageNumber++;
                isContinue = true;
            }
            else if(damageNumber < 3)
            {
                damageNumber++;
            }
            DestoryAttack();
        }
    }

    public bool IsDown()
    {
        return isDown;
    }
}
