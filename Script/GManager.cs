using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [Header("スコア")] public int score;
    [Header("現在のステージ")] public int stageNum;
    [Header("現在の復帰位置")] public int continueNum;
    [Header("現在のHP")] public int hp;
    [Header("デフォルトのHP")] public int defaultHp;
    [Header("現在の残機")]public int heartNum;
    [Header("デフォルトの残機")]public int defaultHeartNum;
    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isStageClear = false;
    [HideInInspector] public bool isTimeAdvance = false;
    [HideInInspector] public float gameTime = 0.0f;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(isTimeAdvance)
        {
            gameTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// HPを１つ増やす
    /// </summary>
    public void addHP()
    {
        if(hp < defaultHp)
        {
            ++hp;
        }
    }

    /// <summary>
    /// HPを１つ減らす
    /// </summary>
    public void SubHP()
    {
        if(hp > 0)
        {
            --hp;
        }
    }

    /// <summary>
    /// 残機を１つ増やす
    /// </summary>
    public void AddHeartNum()
    {
        if(heartNum < 99)
        {
            ++heartNum;
        }
    }

    /// <summary>
    /// 残機を１つ減らす
    /// </summary>
    public void SubHeartNum()
    {
        if(heartNum > 0)
        {
            --heartNum;
        }
        else
        {
            isGameOver = true;
        }
    }

    /// <summary>
    /// 最初から始める時の処理
    /// </summary>
    public void RetryGame()
    {
        isGameOver = false;
        hp = defaultHp;
        heartNum = defaultHeartNum;
        score = 0;
        stageNum = 0;
        continueNum = 0;
        isTimeAdvance = true;
        gameTime = 0.0f;
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }
}
