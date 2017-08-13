using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutController : Controller 
{
    #region 单例
    private static LayoutController Instance
    {
        get;
        set;
    }
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            InitController();
            SendMessage("RegisterController", Instance, SendMessageOptions.RequireReceiver);
        }
    }
    #endregion
    private Dictionary<GuiFrameID, LayoutID> layoutDict;
    protected override void InitController()
    {
        base.id = ControllerID.LayoutController;
        layoutDict = new Dictionary<GuiFrameID, LayoutID>();
        InitLayoutData();
    }
    private void InitLayoutData()
    {
        layoutDict.Add(GuiFrameID.ExamFrame_H, LayoutID.Horizontal);
        layoutDict.Add(GuiFrameID.ExamFrame_V, LayoutID.Vertical);
    }
}
