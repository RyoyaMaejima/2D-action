using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    private Image hpGage = null;
    private int oldHp = 0;

    // Start is called before the first frame update
    void Start()
    {
        hpGage = GetComponent<Image>();
        oldHp = GManager.instance.defaultHp;
        if(GManager.instance != null)
        {
            hpGage.color = new Color(1, 1, 1, 1);
            hpGage.fillAmount = 1;
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
        if(oldHp != GManager.instance.hp)
        {
            hpGage.fillAmount = GManager.instance.hp / (float)GManager.instance.defaultHp;
            oldHp = GManager.instance.hp;
        }
    }
}
