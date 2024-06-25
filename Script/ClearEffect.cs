using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    [Header("拡大縮小のアニメーションカーブ")] public AnimationCurve curve;
    [Header("ステージコントローラー")] public StageCtrl ctrl;

    private bool comp = false;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!comp)
        {
            //クリア演出
            if(timer < 1.0f)
            {
                transform.localScale = Vector3.one * curve.Evaluate(timer);             
                timer += Time.deltaTime;
            }
            //クリアしたら次のステージへ
            else
            {
                transform.localScale = Vector3.one;
                ctrl.ChangeScene(GManager.instance.stageNum + 1);
                comp = true;
            }
        }
    }
}
