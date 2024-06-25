using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    [Header("BGM")] public AudioSource audioSource;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource != null)
        {
            //BGMの音量を調整
            audioSource.volume = slider.value;
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }
}
