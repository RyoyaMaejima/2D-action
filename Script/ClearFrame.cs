using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFrame : MonoBehaviour
{
   [Header("ボス")] public Boss boss;

    private SpriteRenderer sr = null;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(boss.IsDown())
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
            if(continueTime > 5.0f)
            {
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;
                //明滅終わったら消える
                Destroy(this.gameObject);
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }
}
