using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothItem : MonoBehaviour
{
    private UnityEngine.UI.Toggle myToggle;
    private UnityEngine.UI.Image img;
    void Awake()
    {
        myToggle = GetComponent<UnityEngine.UI.Toggle>();
        img = transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>();
    }

    //衣服数据
    private ClothData data;
    /// <summary>
    /// 初始化Item
    /// </summary>
    /// <param name="data">衣服数据</param>
    public void INIT(ClothData data, Sprite sprite, bool isOn)
    {
        if (sprite != null)
        {
            img.sprite = sprite;
            img.gameObject.SetActive(true);
        }
        myToggle.isOn = isOn;
        if(data == null)
        {
            this.data = new ClothData();
            this.data.name = transform.parent.parent.name;
        }
        else
        {
            this.data = data;
        }
        myToggle.onValueChanged.AddListener(OnToggleChange);
    }

    private void OnToggleChange(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.OnPlay(AudioManager.AudioType.按键音);
            GameManager.Instance.CosPlay(data);
        }
    }
}
