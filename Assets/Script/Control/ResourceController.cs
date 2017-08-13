using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceController : Controller
{
    #region 单例
    private static ResourceController Instance
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

    private Dictionary<GuiFrameID, string> guiAssetDict;//key：GuiFrameID，value：资源路径
    private Dictionary<GuiFrameID, Object> resourceDict;//key：GuiFrameID，value：资源
    protected override void InitController()
    {
        base.id = ControllerID.ResourceController;
        guiAssetDict = new Dictionary<GuiFrameID, string>();
        resourceDict = new Dictionary<GuiFrameID, Object>();
        InitReourceData();
    }
    /// <summary>
    /// 注册所有资源地址
    /// </summary>
    private void InitReourceData()
    {
        guiAssetDict.Add(GuiFrameID.StartFrame, "UI/StartFrame");
        guiAssetDict.Add(GuiFrameID.StatisticsFrame, "UI/StatisticsFrame");
        guiAssetDict.Add(GuiFrameID.CategoryFrame, "UI/CategoryFrame");
        guiAssetDict.Add(GuiFrameID.SetUpFrame, "UI/SetUpFrame");
        guiAssetDict.Add(GuiFrameID.ExamFrame_V, "UI/ExamFrame_V");
        guiAssetDict.Add(GuiFrameID.ExamFrame_H, "UI/ExamFrame_H");
        guiAssetDict.Add(GuiFrameID.SettlementFrame, "UI/SettlementFrame");
    }

    /// <summary>
    /// 获取资源实例
    /// </summary>
    /// <param name="id">GuiFrameID</param>
    /// <returns>资源实例</returns>
    public Object GetResource(GuiFrameID id)
    {
        Object resouce = null;
        if (resourceDict.ContainsKey(id))
        {
            resouce = resourceDict[id];
        }
        else
        {
            resouce = Resources.Load(guiAssetDict[id]);
            resourceDict.Add(id, resouce);
        }
        return resouce;
    }
}
