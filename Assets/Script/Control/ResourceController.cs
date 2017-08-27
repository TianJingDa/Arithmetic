﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceController : Controller
{
    #region C#单例
    private static ResourceController instance = null;
    private ResourceController()
    {
        base.id = ControllerID.ResourceController;
        guiAssetDict = new Dictionary<GuiFrameID, string>();
        resourceDict = new Dictionary<GuiFrameID, Object>();
        prefabItemDict = new Dictionary<string, string>();
        InitReourceData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static ResourceController Instance
    {
        get { return instance ?? (instance = new ResourceController()); }
    }
    #endregion
    private Dictionary<GuiFrameID, string> guiAssetDict;//key：GuiFrameID，value：资源路径
    private Dictionary<GuiFrameID, Object> resourceDict;//key：GuiFrameID，value：资源
    private Dictionary<string, string> prefabItemDict;//key：Item名称，value：资源路径
    /// <summary>
    /// 注册所有资源地址
    /// </summary>
    private void InitReourceData()
    {
        guiAssetDict.Add(GuiFrameID.StartFrame, "UI/StartFrame");
        guiAssetDict.Add(GuiFrameID.StatisticsFrame, "UI/StatisticsFrame");
        guiAssetDict.Add(GuiFrameID.CategoryFrame, "UI/CategoryFrame");
        guiAssetDict.Add(GuiFrameID.SetUpFrame, "UI/SetUpFrame");
        guiAssetDict.Add(GuiFrameID.ExamFrame, "UI/ExamFrame");
        guiAssetDict.Add(GuiFrameID.SettlementFrame, "UI/SettlementFrame");

        prefabItemDict.Add("AchievementItem", "PrefabItem/AchievementItem");
        prefabItemDict.Add("SaveFileItem", "PrefabItem/SaveFileItem");
        prefabItemDict.Add("QuestionItem", "PrefabItem/QuestionItem");
    }

    /// <summary>
    /// 获取资源实例
    /// </summary>
    /// <param name="id">GuiFrameID</param>
    /// <returns>资源实例</returns>
    public Object GetGuiResource(GuiFrameID id)
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
    public Object GetItemResource(string name)
    {
        Object resouce = null;
        string address;
        prefabItemDict.TryGetValue(name, out address);
        if (address != null)
        {
            resouce = Resources.Load(address);
        }
        return resouce;
    }
}
