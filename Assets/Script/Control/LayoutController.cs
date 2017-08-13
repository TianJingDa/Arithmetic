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
        layoutDict = new Dictionary<GuiFrameID, LayoutID>();
        InitLayoutData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static LayoutController Instance
    {
        get { return instance ?? (instance = new LayoutController()); }
    }
    #endregion
    private Dictionary<GuiFrameID, LayoutID> layoutDict;
    private void InitLayoutData()
    {
        layoutDict.Add(GuiFrameID.ExamFrame_H, LayoutID.Horizontal);
        layoutDict.Add(GuiFrameID.ExamFrame_V, LayoutID.Vertical);
    }
}
