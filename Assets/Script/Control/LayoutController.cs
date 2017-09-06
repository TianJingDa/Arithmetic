using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        layoutAssetDict.Add(LayoutID.Vertical, new string[] { "Layout/Vertical/Default", "" });
        layoutAssetDict.Add(LayoutID.Horizontal, new string[] { "Layout/Horizontal/Default", "" });
    }
    public RectTransform[] GetLayoutData(LayoutID curLayout, HandednessID curHandedness)
    {
        RectTransform[] transforms = null;
        return transforms;
    }
}
