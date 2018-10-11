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
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
    }
    public static LayoutController Instance
    {
        get { return instance ?? (instance = new LayoutController()); }
    }
    #endregion
    private Dictionary<LayoutID, List<Dictionary<string, MyRectTransform>>> layoutAssetDict;
    private void InitLayoutData()
    {
        string data = CommonTool.GetData("Layout/Vertical/Right");
        LayoutDataWrapper wrapper = JsonUtility.FromJson<LayoutDataWrapper>(data);
        Dictionary<string, MyRectTransform> vertical_Right = ConvertToDict(wrapper);

        data = CommonTool.GetData("Layout/Vertical/Left");
        wrapper = JsonUtility.FromJson<LayoutDataWrapper>(data);
        Dictionary<string, MyRectTransform> vertical_Left = ConvertToDict(wrapper);

        data = CommonTool.GetData("Layout/Horizontal/Right");
        wrapper = JsonUtility.FromJson<LayoutDataWrapper>(data);
        Dictionary<string, MyRectTransform> horizontal_Right = ConvertToDict(wrapper);

        data = CommonTool.GetData("Layout/Horizontal/Left");
        wrapper = JsonUtility.FromJson<LayoutDataWrapper>(data);
        Dictionary<string, MyRectTransform> horizontal_Left = ConvertToDict(wrapper);

        layoutAssetDict.Add(LayoutID.Vertical, new List<Dictionary<string, MyRectTransform>> { vertical_Right, vertical_Left });
        layoutAssetDict.Add(LayoutID.Horizontal, new List<Dictionary<string, MyRectTransform>> { horizontal_Right, horizontal_Left });
    }

    private Dictionary<string, MyRectTransform> ConvertToDict(LayoutDataWrapper wrapper)
    {
        Dictionary<string, MyRectTransform> dict = new Dictionary<string, MyRectTransform>();

        for(int i = 0; i < Mathf.Min(wrapper.names.Count, wrapper.transforms.Count); i++)
        {
            dict[wrapper.names[i]] = wrapper.transforms[i];
        }

        return dict;
    }

    public Dictionary<string, MyRectTransform> GetLayoutData(LayoutID curLayout, HandednessID curHandedness)
    {
        return layoutAssetDict[curLayout][(int)curHandedness];
    }
}

[Serializable]
public class LayoutDataWrapper
{
    public List<string> names;
    public List<MyRectTransform> transforms;

    public LayoutDataWrapper()
    {
        names = new List<string>();
        transforms = new List<MyRectTransform>();
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
