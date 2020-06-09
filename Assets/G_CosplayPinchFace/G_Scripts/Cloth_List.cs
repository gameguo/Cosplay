using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth_List : MonoBehaviour
{
    private ClothItem item;
    private void Awake()
    {
        item = transform.GetChild(0).GetChild(0).GetComponent<ClothItem>();
    }

    private void Start()
    {
        //获取数据初始化
        INIT(GameManager.Instance.GetClothsList(gameObject.name));
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="datas">数据</param>
    private void INIT(List<ClothData> datas)
    {
        if (datas == null)
        {
            return;
        }
        for (int i = 0; i < datas.Count; i++)
        {
            ClothItem cloth = Instantiate(item.gameObject, item.transform.parent).GetComponent<ClothItem>();
            Sprite sprite = GameManager.Instance.GetSprite(datas[i].index, datas[i].name);
            bool isOn = datas[i].index == GameManager.Instance.defaultCloth;
            cloth.INIT(datas[i], sprite, isOn);
        }
        item.INIT(null, null, datas.Count == 0);
    }
}
