using System.Collections;
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
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
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
        guiAssetDict.Add(GuiFrameID.StartFrame, "GuiWrapper/StartFrame");
        guiAssetDict.Add(GuiFrameID.StatisticsFrame, "GuiWrapper/StatisticsFrame");
        guiAssetDict.Add(GuiFrameID.CategoryFrame, "GuiWrapper/CategoryFrame");
        guiAssetDict.Add(GuiFrameID.SetUpFrame, "GuiWrapper/SetUpFrame");
        guiAssetDict.Add(GuiFrameID.FightFrame, "GuiWrapper/FightFrame");
        guiAssetDict.Add(GuiFrameID.SettlementFrame, "GuiWrapper/SettlementFrame");
        guiAssetDict.Add(GuiFrameID.ChapterFrame, "GuiWrapper/ChapterFrame");
        guiAssetDict.Add(GuiFrameID.BluetoothFrame, "GuiWrapper/BluetoothFrame");

        prefabItemDict.Add("AchievementItem", "GuiItem/AchievementItem");
        prefabItemDict.Add("SaveFileItem", "GuiItem/SaveFileItem");
        prefabItemDict.Add("QuestionItem", "GuiItem/QuestionItem");
		prefabItemDict.Add("BluetoothItem", "GuiItem/BluetoothItem");
    }

    /// <summary>
    /// 获取资源实例
    /// </summary>
    /// <param name="id">GuiFrameID</param>
    /// <returns>资源实例</returns>
    public Object GetGuiResource(GuiFrameID id)
    {
        Object resouce = null;
		if(!resourceDict.TryGetValue(id, out resouce))
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
		if(prefabItemDict.TryGetValue(name, out address))
		{
			resouce = Resources.Load(address);
		}
        return resouce;
    }
}
