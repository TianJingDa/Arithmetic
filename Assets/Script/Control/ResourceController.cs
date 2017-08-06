using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController 
{
    private Dictionary<GuiFrameID, string> guiAssetDict = new Dictionary<GuiFrameID, string>();//key：GuiFrameID，value：资源路径
    private Dictionary<GuiFrameID, Object> resourceDict = new Dictionary<GuiFrameID, Object>();//key：GuiFrameID，value：资源
    /// <summary>
    /// 注册所有资源地址
    /// </summary>
    public void RegisterAsset()
    {
        guiAssetDict.Add(GuiFrameID.StartFrame, "StartFrame");
        guiAssetDict.Add(GuiFrameID.StatisticsFrame, "StatisticsFrame");
        guiAssetDict.Add(GuiFrameID.CategoryFrame, "CategoryFrame");
        guiAssetDict.Add(GuiFrameID.SetUpFrame, "SetUpFrame");
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
