using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプする高さ")] public float jumpHeight;
    [Header("ジャンプする長さ")] public float jumpLimitTime;
    [Header("踏みつけ判定の高さの割合(%)")] public float stepOnRate;
    [Header("攻撃してから敵がやられるまでのタイムラグ")] public float attackTimeLag;
    [Header("プレイヤーの当たり判定")] public PlayerCollsionCheck checkCollision;
    [Header("攻撃範囲")]public PlayerAttack attack;
    [Header("接地判定")] public GroundCheck ground;
    [Header("天井判定")] public GroundCheck head;
    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    [Header("ジャンプする時に鳴らすSE")] public AudioClip jumpSE;
    [Header("ダメージ時にならすSE")] public AudioClip hurtSE;
    [Header("やられた時に鳴らすSE")] public AudioClip downSE;
    [Header("攻撃する時に鳴らすSE")] public AudioClip attackSE;
    [Header("前転する時に鳴らすSE")] public AudioClip rollSE;
    #endregion

    #region//プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private BoxCollider2D[] boxcol = new BoxCollider2D[4];
    private SpriteRenderer sr = null;
    private MoveObject moveObj = null;
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isDeadArea = false;
    private bool isHitArea = false;
    private bool isAttackEnemy = false;
    private bool isRun = false;
    private bool isAttack = false;
    private bool isRoll = false;
    private bool isDown = false;
    private bool isHurt = false;
    private bool isOtherJump = false;
    private bool isContinue = false;
    private bool nonDownAnim = false;
    private bool isClearMotion = false;
    private bool canJump = false;
    private bool canRool = false;
    private bool canAttack = false;
    private float jumpPos = 0.0f;
    private float otherJumpHeight = 0.0f;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private float jumpTime = 0.0f;
    private float dashTime = 0.0f;
    private float attackTime = 0.0f;
    private float beforeKey;
    private float offsetX;
    private float offsetY;
    private float sizeX;
    private float sizeY;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxcol = GetComponentsInChildren<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
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
        if(!isDown && !GManager.instance.isGameOver && !GManager.instance.isStageClear)
        {
            //各種当たり判定を得る
            isDeadArea = checkCollision.isDeadArea;
            isHitArea = checkCollision.isHitArea;
            isAttackEnemy = attack.isEnemy;
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //アニメーションによるコライダーのサイズ変更   
            if(isGround)
            {
                if(!isRoll)
                {
                    offsetX = -0.04f;
                    offsetY = 0.7f;
                    sizeX = 0.86f;
                    sizeY = 1.3f;
                    boxcol[0].offset = new Vector3(offsetX, offsetY, 0);
                    boxcol[0].size = new Vector3(sizeX, sizeY, 0);
                }
                else
                {
                    offsetX = -0.04f;
                    offsetY = 0.45f;
                    sizeX = 0.86f;
                    sizeY = 0.74f;
                    boxcol[0].offset = new Vector3(offsetX, offsetY, 0);
                    boxcol[0].size = new Vector3(sizeX, sizeY, 0);
                }
            }
            else
            {
                offsetX = 0.08f;
                offsetY = 0.73f;
                sizeX = 0.6f;
                sizeY = 1.21f;
                boxcol[0].offset = new Vector3(offsetX, offsetY, 0);
                boxcol[0].size = new Vector3(sizeX, sizeY, 0);
            }

            //各種座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();
            
            //攻撃
            Attack();

            //前転
            Roll();

            //ダメージ
            Hurt();

            //ダメージエリアとの当たり判定
            AreaCollision();

            //アニメーションを適用
            SetAnimation();

            //移動速度を設定
            if(moveObj != null)
            {
                rb.velocity = new Vector2(xSpeed, ySpeed) + moveObj.GetVelocity();
            }
            else
            {
                rb.velocity = new Vector2(xSpeed, ySpeed);
            }
        }
        else
        {
            //クリア時
            if(!isClearMotion && GManager.instance.isStageClear)
            {
                anim.Play("player_clear");
                isClearMotion = true;
            }
            rb.velocity = new Vector2(0, -gravity);
        }
    }

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;
        
        //何かを踏んだ際のジャンプ
        if(isOtherJump)
        {
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;
            
            if(canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        //地面にいるとき
        else if(isGround)
        {
            if(verticalKey > 0)
            {
                if(canJump && !isAttack && !isRoll && !isHurt)
                {
                    if(!isJump)
                    {
                        GManager.instance.PlaySE(jumpSE);
                    }
                    ySpeed = jumpSpeed;
                    jumpPos = transform.position.y; //ジャンプした位置を記録する
                    isJump = true;
                    canJump = false;
                    jumpTime = 0.0f;
                }
            }
            else
            {
                isJump = false;
                canJump = true;
            }
        }
        //ジャンプ中
        else if(isJump)
        {
            //上方向キーを押しているか
            bool pushUpKey = verticalKey > 0;
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;
            
            if(pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }
        
        if(isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        return ySpeed;
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;
        
        if(!isAttack && !isHurt)
        {
            if(horizontalKey > 0)
            {
                transform.localScale = new Vector3(2, 2, 1);
                isRun = true;
                dashTime += Time.deltaTime;
                xSpeed = speed;
            }
            else if(horizontalKey < 0)
            {
                transform.localScale = new Vector3(-2, 2, 1);
                isRun = true;
                dashTime += Time.deltaTime;
                xSpeed = -speed;    
            }
            else
            {
                isRun = false;
                xSpeed = 0.0f;
                dashTime = 0.0f;
            }
            
            //前回の入力からダッシュの反転を判断して速度を変える
            if(horizontalKey > 0 && beforeKey < 0)
            {
                dashTime = 0.0f;
            }
            else if(horizontalKey < 0 && beforeKey > 0)
            {
                dashTime = 0.0f;
            }
            
            beforeKey = horizontalKey;
            xSpeed *= dashCurve.Evaluate(dashTime);
            beforeKey = horizontalKey;
        }
        return xSpeed;
    }

    /// <summary>
    /// 攻撃時の処理
    /// </summary>
    private void Attack()
    {
        float jumpKey = Input.GetAxis("Jump");

        if(!isRoll)
        {
            if(!IsAttackAnim())
            {
                if(isAttack)
                {
                    isAttack = false;
                }
                else if(jumpKey > 0)
                {
                    if(canAttack)
                    {
                        GManager.instance.PlaySE(attackSE);
                        isAttack = true;
                        canAttack = false;
                    }
                }
                else
                {
                    canAttack = true;
                }
                attackTime = 0.0f;
            }

            if(isAttack)
            {
                attackTime += Time.deltaTime;
                if(isAttackEnemy)
                {
                    ObjectCollision o = attack.o;                    
                    if(o != null)
                    {
                        if(attackTime >= attackTimeLag * Time.deltaTime)
                        {
                            o.playerAttack = true;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 前転時の処理
    /// </summary>
    private void Roll()
    {
        float verticalKey = Input.GetAxis("Vertical");

        if(isGround && !isAttack)
        {
            if(!IsRollAnim())
            {
                if(isRoll)
                {
                    isRoll = false;
                }
                else if(verticalKey < 0)
                {
                    if(canRool)
                    {
                        GManager.instance.PlaySE(rollSE);
                        isRoll = true;
                        canRool = false;
                    }
                }
                else
                {
                    canRool = true;
                }
            }
        }
    }

    /// <summary>
    /// ダメージ時の処理
    /// </summary>
    private void Hurt()
    {
        if(isHurt && !GManager.instance.isGameOver)
        {
            if(IsHurtAnimEnd())
            {
                isHurt = false;
                anim.Play("player_stand");
            }
        }
    }

    /// <summary>
    /// アニメーションを設定する
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
        anim.SetBool("attack", isAttack);
        anim.SetBool("roll", isRoll);
    }

    /// <summary>
    /// 攻撃中か
    /// </summary>
    private bool IsAttackAnim()
    {
        if(isAttack && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if(currentState.IsName("player_attack"))
            {
                if(currentState.normalizedTime < 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 前転中か
    /// </summary>
    private bool IsRollAnim()
    {
        if(isRoll && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if(currentState.IsName("player_roll"))
            {
                if(currentState.normalizedTime < 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    /// <summary>
    /// ダウンアニメーションが完了しているかどうか
    /// </summary>
    private bool IsDownAnimEnd()
    {
        if(isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if(currentState.IsName("player_down"))
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// ダメージアニメーションが完了しているかどうか
    /// </summary>
    private bool IsHurtAnimEnd()
    {
        if(isHurt && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if(currentState.IsName("player_hurt"))
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// コンティニュー待機状態か
    /// </summary>
    public bool IsContinueWaiting()
    {
        if(GManager.instance.isGameOver)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        }
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer()
    {
        GManager.instance.hp = GManager.instance.defaultHp;
        isDown = false;
        isHurt = false;
        anim.Play("player_stand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;
        nonDownAnim = false;
    }

    /// <summary>
    /// やられた時の処理
    /// </summary>
    private void ReceiveDamage(bool isAnim)
    {
        if(isDown || isHurt|| GManager.instance.isStageClear)
        {
            return;
        }
        else
        {
            if(isAnim)
            {
                if(!isContinue)
                {
                    GManager.instance.SubHP();
                    if(GManager.instance.hp > 0)
                    {
                        anim.Play("player_hurt");
                        isHurt = true;
                        GManager.instance.PlaySE(hurtSE);
                    }
                    else
                    {
                        anim.Play("player_down");
                        isDown = true;
                        GManager.instance.SubHeartNum();
                        GManager.instance.PlaySE(downSE);
                    }
                }
            }
            else
            {
                nonDownAnim = true;
                isDown = true;
                GManager.instance.SubHeartNum();
                GManager.instance.PlaySE(downSE);
            }
        }
    }
    
    /// <summary>
    /// 敵とぶつかったときの処理
    /// </summary>
    public void EnemyCollision() 
    {   
        //踏みつけ判定になる高さ
        float stepOnHeight = boxcol[0].size.y * (stepOnRate / 100f);
        //踏みつけ判定のワールド座標
        float judgePos = transform.position.y + stepOnHeight;
            
        foreach(ContactPoint2D p in checkCollision.other.contacts)
        {
            if(p.point.y < judgePos)
            {
                ObjectCollision o = checkCollision.other.collider.gameObject.GetComponent<ObjectCollision>();
                if(o != null)
                {
                    otherJumpHeight = o.boundHeight; //踏んづけたものから跳ねる高さを取得する
                    o.playerAttack = true; //踏んづけたものに対して踏んづけた事を通知する
                    jumpPos = transform.position.y; //ジャンプした位置を記録する
                    isOtherJump = true;
                    isJump = false;
                    jumpTime = 0.0f;
                }
                else
                {
                    Debug.Log("ObjectCollisionが付いてないよ!");
                }
            }
            else
            {
                ReceiveDamage(true);                   
                break;
            }
        }
    }

    /// <summary>
    /// 動く床を踏んだ時の処理
    /// </summary>
    public void MoveFloorCollision(bool isMoveFloor)
    {
        if(isMoveFloor)
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = boxcol[0].size.y * (stepOnRate / 100f);
            //踏みつけ判定のワールド座標
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

    /// <summary>
    /// ダメージエリアにぶつかったときの処理
    /// </summary>
    private void AreaCollision()
    {
        if(isDeadArea)
        {
            ReceiveDamage(false);
            checkCollision.ResetFlag();
        }
        else if(isHitArea)
        {
            ReceiveDamage(true);
            checkCollision.ResetFlag();
        }
    }
}