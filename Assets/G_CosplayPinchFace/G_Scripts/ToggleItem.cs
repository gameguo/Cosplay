using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleItem : MonoBehaviour
{
    private UnityEngine.UI.Toggle myToggle;

    public Transform groupParent;
    void Start()
    {
        myToggle = GetComponent<UnityEngine.UI.Toggle>();
        myToggle.onValueChanged.AddListener(OnToggleChange);
    }
    private void OnToggleChange(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.OnPlay(AudioManager.AudioType.按键音);
            OpenThisPage();
        }
    }
    //打开自己对应的页面
    private void OpenThisPage()
    {
        if (groupParent == null)
        {
            return;
        }
        for (int i = 0; i < groupParent.childCount; i++)
        {
            groupParent.GetChild(i).gameObject.SetActive(gameObject.name == groupParent.GetChild(i).gameObject.name);
        }
    }
}
