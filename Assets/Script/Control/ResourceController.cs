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
        frameAddressDict = new Dictionary<GuiFrameID, string>();
        frameDict = new Dictionary<GuiFrameID, Object>();
        itemAddressDict = new Dictionary<GuiItemID, string>();
        itemDict = new Dictionary<GuiItemID, Object>();
        InitReourceData();
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
    }
    public static ResourceController Instance
    {
        get { return instance ?? (instance = new ResourceController()); }
    }
    #endregion
    private Dictionary<GuiFrameID, string> frameAddressDict;//key：GuiFrameID，value：资源路径
    private Dictionary<GuiFrameID, Object> frameDict;//key：GuiFrameID，value：资源
    private Dictionary<GuiItemID, string> itemAddressDict;//key：Item名称，value：资源路径
    private Dictionary<GuiItemID, Object> itemDict;//key：GuiFrameID，value：资源

    /// <summary>
    /// 注册所有资源地址
    /// </summary>
    private void InitReourceData()
    {
        frameAddressDict.Add(GuiFrameID.StartFrame, "GuiWrapper/StartFrame");
        frameAddressDict.Add(GuiFrameID.StatisticsFrame, "GuiWrapper/StatisticsFrame");
        frameAddressDict.Add(GuiFrameID.CategoryFrame, "GuiWrapper/CategoryFrame");
        frameAddressDict.Add(GuiFrameID.SetUpFrame, "GuiWrapper/SetUpFrame");
        frameAddressDict.Add(GuiFrameID.FightFrame, "GuiWrapper/FightFrame");
        frameAddressDict.Add(GuiFrameID.SettlementFrame, "GuiWrapper/SettlementFrame");
        frameAddressDict.Add(GuiFrameID.ChapterFrame, "GuiWrapper/ChapterFrame");
        frameAddressDict.Add(GuiFrameID.BluetoothFrame, "GuiWrapper/BluetoothFrame");
        frameAddressDict.Add(GuiFrameID.NameBoardFrame, "GuiWrapper/NameBoardFrame");
        frameAddressDict.Add(GuiFrameID.CommonTipFrame, "GuiWrapper/CommonTipFrame");
        frameAddressDict.Add(GuiFrameID.BluetoothFightFrame, "GuiWrapper/BluetoothFightFrame");
        frameAddressDict.Add(GuiFrameID.SaveFileFrame, "GuiWrapper/SaveFileFrame");
		frameAddressDict.Add(GuiFrameID.ShareFrame, "GuiWrapper/ShareFrame");
		frameAddressDict.Add(GuiFrameID.RankFrame, "GuiWrapper/RankFrame");

        itemAddressDict.Add(GuiItemID.AchievementItem, "GuiItem/AchievementItem");
        itemAddressDict.Add(GuiItemID.SaveFileItem, "GuiItem/SaveFileItem");
        itemAddressDict.Add(GuiItemID.QuestionItem, "GuiItem/QuestionItem");
        itemAddressDict.Add(GuiItemID.PeripheralItem, "GuiItem/PeripheralItem");
		itemAddressDict.Add(GuiItemID.BluetoothQuestionItem, "GuiItem/BluetoothQuestionItem");
		itemAddressDict.Add(GuiItemID.RankItem, "GuiItem/RankItem");
    }

    /// <summary>
    /// 获取资源实例
    /// </summary>
    /// <param name="id">GuiFrameID</param>
    /// <returns>资源实例</returns>
    public Object GetGuiResource(GuiFrameID id)
    {
        Object resouce = null;
		if(!frameDict.TryGetValue(id, out resouce))
        {
            resouce = Resources.Load(frameAddressDict[id]);
            frameDict.Add(id, resouce);
        }
        return resouce;
    }
    public Object GetItemResource(GuiItemID id)
    {
        Object resouce = null;
		if(!itemDict.TryGetValue(id, out resouce))
		{
			resouce = Resources.Load(itemAddressDict[id]);
            itemDict.Add(id, resouce);
		}
        return resouce;
    }
}
