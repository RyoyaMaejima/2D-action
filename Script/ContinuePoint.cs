using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePoint : MonoBehaviour
{
    [Header("コンティニュー番号")] public int continueNum;
    [Header("コンティニュー取得音")] public AudioClip getContinueSE;
    [Header("プレイヤー判定")] public PlayerTriggerCheck trigger;
    [Header("スピード")] public float speed = 3.0f;
    [Header("動く幅")] public float moveDis = 3.0f;

    private bool on = false;
    private float kakudo = 0.0f;
    private Vector3 defaultPos;

    // Start is called before the first frame update
    void Start()
    {
        if (trigger == null)
        {
            Destroy(this);
        }
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger.isOn && !on)
        {
            GManager.instance.PlaySE(getContinueSE);
            GManager.instance.continueNum = continueNum;
            on = true;
        }

        if(on)
        {
            //コンティニューポイントを通った時の演出
            if (kakudo < 180.0f)
            {
                transform.position = defaultPos + Vector3.up * moveDis * Mathf.Sin(kakudo * Mathf.Deg2Rad);

                if (kakudo > 90.0f)
                {
                    transform.localScale = Vector3.one * (1 - ((kakudo - 90.0f) / 90.0f));
                }
                kakudo += 180.0f * Time.deltaTime * speed;
            }
            else
            {
                gameObject.SetActive(false);
                on = false;
            }
        }
    }
}
