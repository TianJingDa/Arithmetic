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
        layoutAssetDict = new Dictionary<OrientationID, string[]>();
        InitLayoutData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static LayoutController Instance
    {
        get { return instance ?? (instance = new LayoutController()); }
    }
    #endregion
    private Dictionary<OrientationID, string[]> layoutAssetDict;
    private void InitLayoutData()
    {
        layoutAssetDict.Add(OrientationID.Vertical, new string[] { "Layout/Vertical/Default", "Layout/Vertical/Custom " });
        layoutAssetDict.Add(OrientationID.Horizontal, new string[] { "Layout/Horizontal/Default", "Layout/Horizontal/Custom " });
    }
    public RectTransform[] GetLayoutData(OrientationID curLayout)
    {
        RectTransform[] transforms = null;
        return transforms;
    }
}
