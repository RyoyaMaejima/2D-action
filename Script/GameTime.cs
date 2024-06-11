using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    private Text timeText = null;
    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<Text>();
        if(GManager.instance != null)
        {
            timeText.text = "Time: " + Math.Floor(GManager.instance.gameTime) + "s";
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
        timeText.text = "Time: " + Math.Floor(GManager.instance.gameTime) + "s";
    }
}
