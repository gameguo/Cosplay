using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string defaultCloth = "7";
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        OnInit();
    }


    //衣服列表
    public List<Transform> cloths;

    //默认衣服列表
    public List<ClothData> defaultCloths;

    //精灵列表
    public List<Sprite> allSprites;

    public List<Colors> colors;

    //内衣裤
    public GameObject girlCloth;

    public MeshRenderer plane;

    /// <summary>
    /// 换背景
    /// </summary>
    /// <param name="color"></param>
    public void CosBG(Colors color)
    {
        plane.material.color = color.plane;
        Camera.main.backgroundColor = color.camera;
    }

    /// <summary>
    /// 换装
    /// </summary>
    /// <param name="data">换装数据</param>
    public void CosPlay(ClothData data)
    {
        if (defaultCloths == null)
        {
            return;
        }
        if (data == null)
        {
            return;
        }
        if (data == lastClothData)
        {
            return;
        }
        
        if (data.name == "all")
        {
            AllClothHandle(data);
        }
        else if (data.name == "wai")
        {
            WaiClothHandle(data);
        }
        else if (data.name == "nei")
        {
            NeiClothHandle(data);
        }
        else if (data.name == "qun")
        {
            QunClothHandle(data);
        }
        else if (data.name == "fa")
        {
            FaClothHandle(data);
        }
        else if (data.name == "xie")
        {
            XieClothHandle(data);
        }
        else if (data.name == "cang")
        {
            CangClothHandle(data);
        }
        lastClothData = data;
    }

    private ClothData lastClothData = null;
    //所有衣服列表操作
    private void AllClothHandle(ClothData data)
    {
        //关闭所有衣服
        AllClose(data.neiyi == null);

        if(data.neiyi)
            data.neiyi.SetActive(true);
        if (data.waiyi)
            data.waiyi.SetActive(true);
        if (data.qunzi)
            data.qunzi.SetActive(true);
        currentCloth.neiyi = data.neiyi;
        currentCloth.waiyi = data.waiyi;
        currentCloth.qunzi = data.qunzi;
    }

    //外衣服列表操作
    private void WaiClothHandle(ClothData data)
    {
        //关闭所有衣服
        AllClose(currentCloth.neiyi == null);
        if (data.waiyi)
            data.waiyi.SetActive(true);
        if (currentCloth.neiyi)
            currentCloth.neiyi.SetActive(true);
        if (currentCloth.qunzi)
            currentCloth.qunzi.SetActive(true);

        currentCloth.waiyi = data.waiyi;
    }

    //内衣服列表操作
    private void NeiClothHandle(ClothData data)
    {
        //关闭所有衣服
        AllClose(data.neiyi == null);
        if (currentCloth.waiyi)
            currentCloth.waiyi.SetActive(true);
        if (data.neiyi)
            data.neiyi.SetActive(true);
        if (currentCloth.qunzi)
            currentCloth.qunzi.SetActive(true);

        currentCloth.neiyi = data.neiyi;
    }

    //裙子衣服列表操作
    private void QunClothHandle(ClothData data)
    {
        //关闭所有衣服
        AllClose(currentCloth.neiyi == null);
        if (currentCloth.waiyi)
            currentCloth.waiyi.SetActive(true);
        if (currentCloth.neiyi)
            currentCloth.neiyi.SetActive(true);
        if (data.qunzi)
            data.qunzi.SetActive(true);

        currentCloth.qunzi = data.qunzi;
    }

    //发衣服列表操作
    private void FaClothHandle(ClothData data)
    {

    }

    //鞋衣服列表操作
    private void XieClothHandle(ClothData data)
    {

    }

    //收藏列表操作
    private void CangClothHandle(ClothData data)
    {

    }
    private ClothData currentCloth;
    //关闭所有衣服
    private void AllClose(bool isOpen)
    {
        //打开内衣
        girlCloth.SetActive(isOpen);
        for (int i = 0; i < defaultCloths.Count; i++)
        {
            if (defaultCloths[i].neiyi != null) {  defaultCloths[i].neiyi.SetActive(false); }
            if (defaultCloths[i].waiyi != null) {  defaultCloths[i].waiyi.SetActive(false); }
            if (defaultCloths[i].qunzi != null) {  defaultCloths[i].qunzi.SetActive(false); }
        }
    }


    //初始化默认列表
    private void OnInit()
    {
        if (cloths == null)
        {
            return;
        }
        currentCloth = new ClothData();
        defaultCloths = new List<ClothData>();
        for (int i = 0; i < cloths.Count; i++)
        {
            ClothData data = new ClothData();
            for (int j = 0; j < cloths[i].childCount; j++)
            {
                data.name = "all";
                data.index = (i + 1).ToString();
                if (cloths[i].GetChild(j).name == "neiyi")
                {
                    data.neiyi = cloths[i].GetChild(j).gameObject;
                    if (data.neiyi.activeSelf)
                    {
                        currentCloth.neiyi = data.neiyi;
                        defaultCloth = (i + 1).ToString();
                    }
                }
                else if (cloths[i].GetChild(j).name == "waiyi")
                {
                    data.waiyi = cloths[i].GetChild(j).gameObject;
                    if (data.waiyi.activeSelf)
                    {
                        currentCloth.waiyi = data.waiyi;
                    }
                }
                else if (cloths[i].GetChild(j).name == "qunzi")
                {
                    data.qunzi = cloths[i].GetChild(j).gameObject;
                    if (data.qunzi.activeSelf)
                    {
                        currentCloth.qunzi = data.qunzi;
                    }
                }
            }
            defaultCloths.Add(data);
        }
    }

    /// <summary>
    /// 获取精灵
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns></returns>
    public Sprite GetSprite(string id, string n)
    {
        if (n == "all")
        {
            for (int i = 0; i < allSprites.Count; i++)
            {
                if (allSprites[i].name == id)
                {
                    //Debug.Log(allSprites[i].name);
                    return allSprites[i];
                }
            }
            return null;
        }
        else if (n == "wai")
        {
            for (int i = 0; i < allSprites.Count; i++)
            {
                if (allSprites[i].name == id)
                {
                    //Debug.Log(allSprites[i].name);
                    return allSprites[i];
                }
            }
            return null;
        }
        else if (n == "nei")
        {
            for (int i = 0; i < allSprites.Count; i++)
            {
                if (allSprites[i].name == id)
                {
                    //Debug.Log(allSprites[i].name);
                    return allSprites[i];
                }
            }
            return null;
        }
        else if (n == "qun")
        {
            for (int i = 0; i < allSprites.Count; i++)
            {
                if (allSprites[i].name == id)
                {
                    //Debug.Log(allSprites[i].name);
                    return allSprites[i];
                }
            }
            return null;
        }
        else if (n == "fa")
        {
            return null;
        }
        else if (n == "xie")
        {
            return null;
        }
        else if (n == "cang")
        {
            return null;
        }
        else
        {
            return null;
        }
    }


    private Dictionary<string, List<ClothData>> clothList = new Dictionary<string, List<ClothData>>();

    /// <summary>
    /// 获取衣服列表
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>返回列表</returns>
    public List<ClothData> GetClothsList(string id)
    {
        if(id == "all")
        {
            return defaultCloths;
        }
        else if (id == "cang")
        {
            //if (!clothList.ContainsKey(id))
            //{
            //    clothList.Add(id, new List<ClothData>());
            //    SetList("shoucang", clothList[id]);
            //}
            return null;
        }
        else
        {
            if (!clothList.ContainsKey(id))
            {
                clothList.Add(id, new List<ClothData>());
                SetList(id, clothList[id]);
            }
            return clothList[id];
        }
    }
    //设置列表
    private void SetList(string n, List<ClothData> datas)
    {
        if(defaultCloths == null)
        {
            return;
        }
        string id = GetName(n);
        for (int i = 0; i < defaultCloths.Count; i++)
        {
            if(defaultCloths[i].neiyi != null && defaultCloths[i].neiyi.name == id)
            {
                ClothData d = new ClothData();
                d.index = defaultCloths[i].index;
                d.name = n;
                d.neiyi = defaultCloths[i].neiyi;
                datas.Add(d);
                continue;
            }
            else if (defaultCloths[i].waiyi != null && defaultCloths[i].waiyi.name == id)
            {
                ClothData d = new ClothData();
                d.index = defaultCloths[i].index;
                d.name = n;
                d.waiyi = defaultCloths[i].waiyi;
                datas.Add(d);
                continue;
            }
            else if (defaultCloths[i].qunzi != null && defaultCloths[i].qunzi.name == id)
            {
                ClothData d = new ClothData();
                d.index = defaultCloths[i].index;
                d.name = n;
                d.qunzi = defaultCloths[i].qunzi;
                datas.Add(d);
                continue;
            }
        }
    }

    private string GetName(string id)
    {
        if (id == "wai")
        {
            return "waiyi";
        }
        else if (id == "nei")
        {
            return "neiyi";
        }
        else if (id == "qun")
        {
            return "qunzi";
        }
        else if (id == "fa")
        {
            return "toufa";
        }
        else if (id == "xie")
        {
            return "xiezi";
        }
        else
        {
            return "";
        }
    }

}

[System.Serializable]
public struct Colors
{
    public Color plane;
    public Color camera;
}

public class ClothData
{
    public string index;
    public string name;
    public GameObject neiyi;
    public GameObject waiyi;
    public GameObject qunzi;
}
