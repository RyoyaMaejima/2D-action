using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("フェード")] public FadeImage fade;
    [Header("ゲームスタート時に鳴らすSE")] public AudioClip startSE;

    private bool firstPush = false;
    private bool goNextScene = false;

    /// <summary>
    /// スタートボタンを押されたら呼ばれる
    /// </summary>
    public void PressStart()
    {
        if (!firstPush)
        {
            if(!firstPush)
            {
                GManager.instance.PlaySE(startSE);
                fade.StartFadeOut();
                firstPush = true;
            }
        }
    }

    private void Update()
    {
        if(!goNextScene && fade.IsFadeOutComplete())
        {
            GManager.instance.RetryGame();
            GManager.instance.stageNum = 1;
            SceneManager.LoadScene("stage1");
            goNextScene = true;
        }
    }
}
