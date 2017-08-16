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
        layoutAssetDict.Add(LayoutID.Vertical, new string[] { "Layout/Vertical/Default", "Layout/Vertical/Custom " });
        layoutAssetDict.Add(LayoutID.Horizontal, new string[] { "Layout/Horizontal/Default", "Layout/Horizontal/Custom " });
    }
    public RectTransform[] GetLayoutData(LayoutID curLayout)
    {
        RectTransform[] transforms = null;
        return transforms;
    }
}
