using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgColorItem : MonoBehaviour
{
    private UnityEngine.UI.Toggle myToggle;
    private UnityEngine.UI.Image img;
    void Awake()
    {
        myToggle = GetComponent<UnityEngine.UI.Toggle>();
        img = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
    }

    //衣服数据
    private Colors color;
    /// <summary>
    /// 初始化Item
    /// </summary>
    /// <param name="data">衣服数据</param>
    public void INIT(Colors color, bool isOn)
    {
        myToggle.isOn = isOn;
        this.color = color;
        img.color = color.camera;
        myToggle.onValueChanged.AddListener(OnToggleChange);
    }

    private void OnToggleChange(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.OnPlay(AudioManager.AudioType.按键音);
            GameManager.Instance.CosBG(color);
        }
    }
}
