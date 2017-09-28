using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LayoutController : Controller 
{
    #region C#单例
    private static LayoutController instance = null;
    private LayoutController()
    {
        base.id = ControllerID.LayoutController;
        layoutAssetDict = new Dictionary<LayoutID, List<Dictionary<string, MyRectTransform>>>();
        InitLayoutData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static LayoutController Instance
    {
        get { return instance ?? (instance = new LayoutController()); }
    }
    #endregion
    private Dictionary<LayoutID, List<Dictionary<string, MyRectTransform>>> layoutAssetDict;
    private void InitLayoutData()
    {
        Dictionary<string, MyRectTransform> vertical_Right = (Dictionary<string, MyRectTransform>)IOHelper.GetData(Application.dataPath + "/Resources/Layout/Vertical/Right.txt", typeof(Dictionary<string, MyRectTransform>));
        Dictionary<string, MyRectTransform> vertical_Left = (Dictionary<string, MyRectTransform>)IOHelper.GetData(Application.dataPath + "/Resources/Layout/Vertical/Left.txt", typeof(Dictionary<string, MyRectTransform>));
        //Dictionary<string, MyRectTransform> horizontal_Right = (Dictionary<string, MyRectTransform>)IOHelper.GetData(Application.dataPath + "/Resources/Layout/Horizontal/Right.txt", typeof(Dictionary<string, MyRectTransform>));
        //Dictionary<string, MyRectTransform> horizontal_Left = (Dictionary<string, MyRectTransform>)IOHelper.GetData(Application.dataPath + "/Resources/Layout/Horizontal/Left.txt", typeof(Dictionary<string, MyRectTransform>));
        layoutAssetDict.Add(LayoutID.Vertical, new List<Dictionary<string, MyRectTransform>> { vertical_Right, vertical_Left });
        //layoutAssetDict.Add(LayoutID.Horizontal, new List<Dictionary<string, MyRectTransform>> { horizontal_Right, horizontal_Left });
    }
    public Dictionary<string, MyRectTransform> GetLayoutData(LayoutID curLayout, HandednessID curHandedness)
    {
        return layoutAssetDict[curLayout][(int)curHandedness];
    }
}
[Serializable]
public class MyRectTransform
{
    public MyVector2 pivot;
    public MyVector2 anchorMax;
    public MyVector2 anchorMin;
    public MyVector2 offsetMax;
    public MyVector2 offsetMin;
    public MyVector3 localEulerAngles;
}
[Serializable]
public class MyVector2
{
    public float x;
    public float y;
    public MyVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
[Serializable]
public class MyVector3
{
    public float x;
    public float y;
    public float z;
    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
