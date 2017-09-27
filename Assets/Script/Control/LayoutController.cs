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
        layoutAssetDict = new Dictionary<LayoutID, string[]>();
        InitLayoutData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static LayoutController Instance
    {
        get { return instance ?? (instance = new LayoutController()); }
    }
    #endregion
    private Dictionary<LayoutID, string[]> layoutAssetDict;
    private void InitLayoutData()
    {
        layoutAssetDict.Add(LayoutID.Vertical, new string[] { Application.dataPath + "/Resources/Layout/Vertical/Default.txt", "" });
        layoutAssetDict.Add(LayoutID.Horizontal, new string[] { Application.dataPath + "/Resources/Layout/Horizontal/Default.txt", "" });
    }
    public Dictionary<string, MyRectTransform> GetLayoutData(LayoutID curLayout, HandednessID curHandedness)
    {
        string fullName = layoutAssetDict[curLayout][0];
        Dictionary<string, MyRectTransform> rectTransforms = (Dictionary<string, MyRectTransform>)IOHelper.GetData(fullName, typeof(Dictionary<string, MyRectTransform>));
        return rectTransforms;
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
