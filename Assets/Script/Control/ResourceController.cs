using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceCtrl : Controller
{
    #region 单例
    private static ResourceCtrl Instance
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
            GameManager.Instance.RegisterController(base.id, Instance);
        }
    }
    #endregion

    private Dictionary<GuiFrameID, string> guiAssetDict;//key：GuiFrameID，value：资源路径
    private Dictionary<GuiFrameID, Object> resourceDict;//key：GuiFrameID，value：资源
    protected override void InitController()
    {
        base.id = ControllerID.ResourceCtrl;
        guiAssetDict = new Dictionary<GuiFrameID, string>();
        resourceDict = new Dictionary<GuiFrameID, Object>();
        RegisterAsset();
    }
    /// <summary>
    /// 注册所有资源地址
    /// </summary>
    private void RegisterAsset()
    {
        guiAssetDict.Add(GuiFrameID.StartFrame, "StartFrame");
        guiAssetDict.Add(GuiFrameID.StatisticsFrame, "StatisticsFrame");
        guiAssetDict.Add(GuiFrameID.CategoryFrame, "CategoryFrame");
        guiAssetDict.Add(GuiFrameID.SetUpFrame, "SetUpFrame");
        guiAssetDict.Add(GuiFrameID.AnswerFrame, "AnswerFrame");
        guiAssetDict.Add(GuiFrameID.SettlementFrame, "SettlementFrame");
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
