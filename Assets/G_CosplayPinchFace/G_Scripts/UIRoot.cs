using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{
    //复位按钮
    public Button reset;
    //展开按钮
    public Button openButton;
    //背景
    public RectTransform bg;
    private void Start()
    {
        if (CameraControl.Instance != null)
        {
            reset.onClick.AddListener(OnAudioButton);
            reset.onClick.AddListener(CameraControl.Instance.OnReset);
        }
        openButton.onClick.AddListener(OnAudioButton);
        openButton.onClick.AddListener(OpenClick);

        audioCtrl.onValueChanged.AddListener(AudioCtrl);
    }


    #region 打开关闭UI面板
    private bool isOpen = false;
    private void OpenClick()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(OPEN_ASYNC());
    } 

    IEnumerator OPEN_ASYNC()
    {
        float endY;
        if (isOpen) endY = 0;
        else endY = -bg.sizeDelta.y;
        while (true)
        {
            if(Mathf.Abs(endY - bg.anchoredPosition.y) <= 0.1f)
            {
                bg.anchoredPosition = new Vector2(bg.anchoredPosition.x, endY);
                break;
            }
            else
            {
                float y = Mathf.Lerp(bg.anchoredPosition.y, endY, Time.deltaTime * 10f);
                bg.anchoredPosition = new Vector2(bg.anchoredPosition.x, y);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion


    #region 声音控制
    public Toggle audioCtrl;

    private void AudioCtrl(bool isOn)
    {
        OnAudioButton();
        AudioManager.Instance.IsBgAudioEnable = !isOn;
    }

    public void OnAudioButton()
    {
        AudioManager.Instance.OnPlay(AudioManager.AudioType.按键音);
    } 
    #endregion




}
