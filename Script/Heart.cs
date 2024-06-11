using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    private Text heartText = null;
    private int oldHearteNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        heartText = GetComponent<Text>();
        if(GManager.instance != null)
        {
            heartText.text = "× " + GManager.instance.heartNum;
        }
        else
        {
            Debug.Log("ゲームマネージャーがありません");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(oldHearteNum != GManager.instance.heartNum)
        {
            heartText.text = "× " + GManager.instance.heartNum;
            oldHearteNum = GManager.instance.heartNum;
        }
    }
}
