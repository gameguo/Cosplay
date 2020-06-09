using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgColor : MonoBehaviour
{
    private bgColorItem item;
    private void Awake()
    {
        item = transform.GetChild(0).GetChild(0).GetComponent<bgColorItem>();
    }

    private void Start()
    {
        //获取数据初始化
        INIT(GameManager.Instance.colors);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="datas">数据</param>
    private void INIT(List<Colors> datas)
    {
        if (datas == null)
        {
            return;
        }
        for (int i = 1; i < datas.Count; i++)
        {
            bgColorItem c = Instantiate(item.gameObject, item.transform.parent).GetComponent<bgColorItem>();
            c.INIT(datas[i], false);
        }
        item.INIT(datas[0], true);
    }
}
