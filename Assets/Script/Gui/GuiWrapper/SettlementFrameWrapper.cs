using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using cn.sharesdk.unity3d;

/// <summary>
/// 结算界面
/// </summary>
public class SettlementFrameWrapper : GuiFrameWrapper
{
    private bool onlyWrong;
    private bool isBluetooth;

    private GameObject commonResult;
    private GameObject onlyWrongImage;
    private GameObject curAchievementBtn;
    private GameObject bluetoothResult;
    private GameObject bluetoothOnlyWrongImage;
    private GameObject achievementDetailBgInSettlement;
    private GameObject achievementDetailPageInSettlement;
    private Image achievementDetailImageInSettlement;
    private Text settlementTime;
    private Text settlementAmount;
    private Text settlementAccuracy;
    private Text saveFileOwnName;
    private Text saveFileOtherName;
    private Text achievementDetailMainTitleInSettlement;
    private Text achievementDetailSubTitleInSettlement;
    private Text achievementDetailFinishTimeInSettlement;
    private RectTransform settlementGrid;
    private SaveFileInstance curSaveFileInstance;
    private AchievementInstance curAchievementInstance;
    private List<QuestionInstance> onlyWrongList;
    private List<QuestionInstance> allInstanceList;



    void Start () 
	{
        id = GuiFrameID.SettlementFrame;
        Init();
        InitSettlement();
        InitAchievement();
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        settlementGrid                              = gameObjectDict["SettlementGrid"].GetComponent<RectTransform>();
        settlementTime                              = gameObjectDict["SettlementTime"].GetComponent<Text>();
        settlementAmount                            = gameObjectDict["SettlementAmount"].GetComponent<Text>();
        settlementAccuracy                          = gameObjectDict["SettlementAccuracy"].GetComponent<Text>();
        achievementDetailMainTitleInSettlement      = gameObjectDict["AchievementDetailMainTitleInSettlement"].GetComponent<Text>();
        achievementDetailSubTitleInSettlement       = gameObjectDict["AchievementDetailSubTitleInSettlement"].GetComponent<Text>();
        achievementDetailFinishTimeInSettlement     = gameObjectDict["AchievementDetailFinishTimeInSettlement"].GetComponent<Text>();
        saveFileOwnName                             = gameObjectDict["SaveFileOwnName"].GetComponent<Text>();
        saveFileOtherName                           = gameObjectDict["SaveFileOtherName"].GetComponent<Text>();
        achievementDetailBgInSettlement             = gameObjectDict["AchievementDetailBgInSettlement"];
        achievementDetailImageInSettlement          = gameObjectDict["AchievementDetailImageInSettlement"].GetComponent<Image>();
        commonResult                                = gameObjectDict["CommonResult"];
        onlyWrongImage                              = gameObjectDict["OnlyWrongImage"];
        curAchievementBtn                           = gameObjectDict["CurAchievementBtn"];
        bluetoothResult                             = gameObjectDict["BluetoothResult"];
        bluetoothOnlyWrongImage                     = gameObjectDict["BluetoothOnlyWrongImage"];
        achievementDetailPageInSettlement           = gameObjectDict["AchievementDetailPageInSettlement"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetailBgInSettlement":
                CommonTool.GuiScale(achievementDetailPageInSettlement, canvasGroup, false,()=> achievementDetailBgInSettlement.SetActive(false));
                break;
            case "OnlyWrongBtn":
            case "BluetoothOnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "Settlement2CategoryFrameBtn":
				GameManager.Instance.SwitchWrapperWithMove(GameManager.Instance.CompetitionGUI,MoveID.RightOrUp,false);
                break;
            case "Settlement2StartFrameBtn":
                GameManager.Instance.SwitchWrapperWithScale(GuiFrameID.StartFrame,false);
                break;
            case "ShareBtnInSettlement":
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.SaveFile);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            case "BluetoothShareBtn":
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.Bluetooth);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            case "CurAchievementBtn":
                achievementDetailBgInSettlement.SetActive(true);
                achievementDetailPageInSettlement.SetActive(true);
                CommonTool.GuiScale(achievementDetailPageInSettlement, canvasGroup, true);
                break;
            case "UploadDataBtn":
                if (string.IsNullOrEmpty(GameManager.Instance.UserName))
                {
                    GameManager.Instance.SwitchWrapper(GuiFrameID.NameBoardFrame, true);
                    return;
                }

                WWWForm form = new WWWForm();
                form.AddField("pattern", (int)curSaveFileInstance.cInstance.patternID);
                form.AddField("amount", (int)curSaveFileInstance.cInstance.amountID);
                form.AddField("symbol", (int)curSaveFileInstance.cInstance.symbolID);
                form.AddField("digit", (int)curSaveFileInstance.cInstance.digitID);
                form.AddField("operand", (int)curSaveFileInstance.cInstance.operandID);
                float result = curSaveFileInstance.timeCost * (1 - curSaveFileInstance.accuracy);
                form.AddField("result", result.ToString());
                RankInstance instance = new RankInstance();
                instance.userName = GameManager.Instance.UserName;
                instance.saveFile = curSaveFileInstance;
                string data = JsonUtility.ToJson(instance);
                form.AddField("data", data);

				GameManager.Instance.UploadData(form, OnUploadFinished);
                break;
            case "AchievementDetailShareBtn":
                GameManager.Instance.CurAchievementInstance = curAchievementInstance;
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.Achievement);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private void InitSettlement()
    {
        curSaveFileInstance = GameManager.Instance.CurSaveFileInstance;
        settlementTime.text = string.Format(settlementTime.text, curSaveFileInstance.timeCost.ToString("f1"));
        settlementAmount.text = string.Format(settlementAmount.text, curSaveFileInstance.qInstancList.Count);
        settlementAccuracy.text = string.Format(settlementAccuracy.text, curSaveFileInstance.accuracy.ToString("f1"));
        isBluetooth = !string.IsNullOrEmpty(curSaveFileInstance.opponentName);
        commonResult.SetActive(!isBluetooth);
        bluetoothResult.SetActive(isBluetooth);
        if (isBluetooth)
        {
            saveFileOwnName.text = GameManager.Instance.UserName;
            saveFileOtherName.text = curSaveFileInstance.opponentName;
        }
        allInstanceList = curSaveFileInstance.qInstancList;
        onlyWrongList = allInstanceList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
    }

    private bool FindWrong(QuestionInstance questionInstance)
    {
        int count = questionInstance.instance.Count;
        return questionInstance.instance[count - 1] != questionInstance.instance[count - 2];
    }

    private void RefreshSettlementGrid()
    {
        onlyWrongImage.SetActive(onlyWrong);
        bluetoothOnlyWrongImage.SetActive(onlyWrong);
        ArrayList dataList;
        if (onlyWrong)
        {
            dataList = new ArrayList(onlyWrongList);
        }
        else
        {
            dataList = new ArrayList(allInstanceList);
        }

        if (isBluetooth) CommonTool.RefreshScrollContent(settlementGrid, dataList, GuiItemID.BluetoothQuestionItem);
        else CommonTool.RefreshScrollContent(settlementGrid, dataList, GuiItemID.QuestionItem);
    }
    private void InitAchievement()
    {
		if (!string.IsNullOrEmpty(GameManager.Instance.CurAchievementName) && GameManager.Instance.CompetitionGUI == GuiFrameID.ChapterFrame)
        {
            curAchievementBtn.SetActive(true);
            achievementDetailBgInSettlement.SetActive(true);
            achievementDetailPageInSettlement.SetActive(true);
            CommonTool.GuiScale(achievementDetailPageInSettlement, canvasGroup, true);
            curAchievementInstance = GameManager.Instance.GetAchievement();
            achievementDetailImageInSettlement.sprite = GameManager.Instance.GetSprite(curAchievementInstance.imageIndex);
            achievementDetailMainTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(curAchievementInstance.mainTitleIndex);
            achievementDetailSubTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(curAchievementInstance.subTitleIndex);
            achievementDetailFinishTimeInSettlement.text = GetFinishTime(curAchievementInstance.finishTime);
        }
        else
        {
            curAchievementBtn.SetActive(false);
        }
    }
    private string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }

	private void OnUploadFinished(string message)
	{
		GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
		GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
	}
}
