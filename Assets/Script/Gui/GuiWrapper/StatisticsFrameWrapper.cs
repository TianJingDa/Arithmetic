using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 统计数据界面
/// </summary>
public class StatisticsFrameWrapper : GuiFrameWrapper
{
    private int curSaveFileIndex;//当前所选存档类别序号，-1、综合；0、加法；1、减法；2、乘法；3、除法
    private int curSaveFileCount;//存档总数量
    private int curAchievementIndex;//当前所选成就类别序号，-1、综合；0、加法；1、减法；2、乘法；3、除法
    private Text totelTimeImg_Text2;
    private Text totelGameImg_Text2;
    private Text additionSummary_Text;
    private Text subtractionSummary_Text;
    private Text multiplicationSummary_Text;
    private Text divisionSummary_Text;
    private GameObject saveFileWin;
    private GameObject achievementWin;
    private GameObject saveFileDetailBg;
    private GameObject deleteSaveFileBg;
    private GameObject deleteAchievementBg;
    private GameObject saveFileSummary;
    private GameObject achievementSummary;
    private GameObject saveFileDetailBgOfAchievement;
    private GameObject achievementDetailBgInSaveFile;
    private GameObject achievementDetailBgInStatistics;
    private InfiniteList achievementGrid;
    private InfiniteList saveFileGrid;
    private ToggleGroup saveFileToggleGroup;
    private ToggleGroup achievementToggleGroup;
    private SummarySaveFileItem additionSaveFileItem;
    private SummarySaveFileItem subtractionSaveFileItem;
    private SummarySaveFileItem multiplicationSaveFileItem;
    private SummarySaveFileItem divisionSaveFileItem;
    private LastestAchievementItem lastestAchievementItem;
    private SummaryAchievementItem[] summaryAchievementArray;
    private HiddenAchievementItem hiddenAchievementItem;
    private Dictionary<SymbolID, string> rawSaveFileStringDict;
    private Dictionary<SymbolID, List<SaveFileInstance>> saveFileDict;
    private Dictionary<SymbolID, List<AchievementInstance>> achievementDict;

    void Start () 
	{
        id = GuiFrameID.StatisticsFrame;
        Init();
        int totalTime = (int)GameManager.Instance.TotalTime;
        TimeSpan ts = new TimeSpan(0, 0, totalTime);
        totelTimeImg_Text2.text = string.Format(totelTimeImg_Text2.text, ts.Hours, ts.Minutes, ts.Seconds);
        totelGameImg_Text2.text = string.Format(totelGameImg_Text2.text, GameManager.Instance.TotalGame);
        rawSaveFileStringDict = new Dictionary<SymbolID, string>
        {
            {SymbolID.Addition, additionSummary_Text.text },
            {SymbolID.Subtraction, subtractionSummary_Text.text },
            {SymbolID.Multiplication, multiplicationSummary_Text.text },
            {SymbolID.Division, divisionSummary_Text.text }
        };
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        achievementWin                  = GameObjectDict["AchievementWin"];
        saveFileWin                     = GameObjectDict["SaveFileWin"];
        saveFileGrid                    = GameObjectDict["SaveFileGrid"].GetComponent<InfiniteList>();
        achievementGrid                 = GameObjectDict["AchievementGrid"].GetComponent<InfiniteList>();
        saveFileDetailBg                = GameObjectDict["SaveFileDetailBg"];
        deleteSaveFileBg                = GameObjectDict["DeleteSaveFileBg"];
        deleteAchievementBg             = GameObjectDict["DeleteAchievementBg"];
        saveFileDetailBgOfAchievement   = GameObjectDict["SaveFileDetailBgOfAchievement"];
        saveFileSummary                 = GameObjectDict["SaveFileSummary"];
        achievementSummary              = GameObjectDict["AchievementSummary"];
        totelTimeImg_Text2              = GameObjectDict["TotelTimeImg_Text2"].GetComponent<Text>();
        totelGameImg_Text2              = GameObjectDict["TotelGameImg_Text2"].GetComponent<Text>();
        additionSummary_Text            = GameObjectDict["AdditionSummary_Text"].GetComponent<Text>();
        subtractionSummary_Text         = GameObjectDict["SubtractionSummary_Text"].GetComponent<Text>();
        multiplicationSummary_Text      = GameObjectDict["MultiplicationSummary_Text"].GetComponent<Text>();
        divisionSummary_Text            = GameObjectDict["DivisionSummary_Text"].GetComponent<Text>();
        achievementDetailBgInSaveFile   = GameObjectDict["AchievementDetailBgInSaveFile"];
        achievementDetailBgInStatistics = GameObjectDict["AchievementDetailBgInStatistics"];
        additionSaveFileItem            = GameObjectDict["AdditionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        subtractionSaveFileItem         = GameObjectDict["SubtractionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        multiplicationSaveFileItem      = GameObjectDict["MultiplicationSaveFileItem"].GetComponent<SummarySaveFileItem>();
        divisionSaveFileItem            = GameObjectDict["DivisionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        lastestAchievementItem          = GameObjectDict["LastestAchievementItem"].GetComponent<LastestAchievementItem>();
        hiddenAchievementItem           = GameObjectDict["HiddenAchievementItem"].GetComponent<HiddenAchievementItem>();
        saveFileToggleGroup             = GameObjectDict["SaveFileToggleGroup"].GetComponent<ToggleGroup>();
        achievementToggleGroup          = GameObjectDict["AchievementToggleGroup"].GetComponent<ToggleGroup>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Achievement2StartFrameBtn":
            case "Save2StartFrameBtn":
            case "Statistics2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StatisticsFrame, GuiFrameID.StartFrame);
                break;
            case "Achievement2StatisticsFrameBtn":
                GameManager.Instance.CurAction = null;
                achievementWin.SetActive(false);
                break;
            case "AchievementBtn":
                achievementWin.SetActive(true);
                RefreshAchievementWin();
                break;
            case "AchievementDetailBgInStatistics":
                achievementDetailBgInStatistics.SetActive(false);
                break;
            case "AchievementDetailBgInSaveFile":
                achievementDetailBgInSaveFile.SetActive(false);
                break;
            case "DeleteSaveFileBg":
            case "DeleteCancelBtnInSaveFile":
                deleteSaveFileBg.SetActive(false);
                break;
            case "DeleteCancelBtnInAchievement":
                deleteAchievementBg.SetActive(false);
                break;
            case "SaveFileDetai2AchievementDetailBtn":
                saveFileDetailBgOfAchievement.SetActive(false);
                break;
            case "Save2StatisticsFrameBtn":
                GameManager.Instance.CurAction = null;
                saveFileWin.SetActive(false);
                break;
            case "SaveFileBtn":
                RefreshSaveFileWin();
                break;
            case "SaveFileDetai2SaveFileWinBtn":
                saveFileDetailBg.SetActive(false);
                break;
            case "SaveFileDetai2StatisticsFrameBtn":
                GameManager.Instance.CurAction = null;
                saveFileDetailBg.SetActive(false);
                saveFileWin.SetActive(false);
                break;
            default:
                MyDebug.LogYellow("Can not find Button:" + btn.name);
                break;
        }
    }

    protected override void OnToggleClick(Toggle tgl)
    {
        base.OnToggleClick(tgl);
        if (!tgl.isOn) return;
        if (tgl.name.Contains("SaveFileToggle_"))
        {
            if (tgl.index < 0)
            {
                RefreshSummarySaveFilePage();
            }
            else
            {
                curSaveFileIndex = tgl.index;
                RefreshSaveFileList();
            }
        }
        else if (tgl.name.Contains("AchievementToggle_"))
        {
            if (tgl.index < 0)
            {
                RefreshSummaryAchievementPage();
            }
            else
            {
                curAchievementIndex = tgl.index;
                RefreshAchievementList();
            }
        }
    }
    #region 成就
    private void RefreshAchievementWin()
    {
        achievementWin.SetActive(true);
        if (summaryAchievementArray == null) summaryAchievementArray = achievementSummary.GetComponentsInChildren<SummaryAchievementItem>(true);
        List<AchievementInstance> achievementList = GameManager.Instance.GetAllAchievements();
        achievementDict = new Dictionary<SymbolID, List<AchievementInstance>>
                {
                    {SymbolID.Addition, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Addition)},
                    {SymbolID.Subtraction, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Subtraction)},
                    {SymbolID.Multiplication, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Multiplication)},
                    {SymbolID.Division, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Division)},
                    {SymbolID.Summary, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Summary)},
                    {SymbolID.Hidden, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Hidden)}
                };
        achievementToggleGroup.SetAllTogglesOff();
        achievementToggleGroup.toggles[0].isOn = true;
        GameManager.Instance.CurAction = RefreshAchievementDict;
    }
    private void RefreshSummaryAchievementPage()
    {
        achievementSummary.SetActive(true);
        RefreshLastestAchievement();
        RefreshSummaryAchievement();
    }
    private void RefreshLastestAchievement()
    {
        AchievementInstance lastestAchievement = GameManager.Instance.GetLastestAchievement();
        lastestAchievementItem.SendMessage("InitPrefabItem", lastestAchievement);
        lastestAchievementItem.SendMessage("InitDetailWin", achievementDetailBgInStatistics);
    }
    private void RefreshSummaryAchievement()
    {
        for(int i = 0; i < summaryAchievementArray.Length; i++)
        {
            summaryAchievementArray[i].SendMessage("InitPrefabItem", achievementDict[SymbolID.Summary][i]);
            summaryAchievementArray[i].SendMessage("InitDetailWin", achievementDetailBgInStatistics);
        }
    }

    private void RefreshAchievementList()
    {

    }
    private void RefreshAchievementDict()
    {

    }

    #endregion

    #region 存档
    private void RefreshSaveFileWin()
    {
        saveFileWin.SetActive(true);
        List<SaveFileInstance> saveFileList = GameManager.Instance.ReadAllRecords();
        curSaveFileCount = saveFileList.Count;
        saveFileDict = new Dictionary<SymbolID, List<SaveFileInstance>>
                {
                    {SymbolID.Addition, saveFileList.FindAll(x => x.cInstance.symbolID == SymbolID.Addition)},
                    {SymbolID.Subtraction, saveFileList.FindAll(x => x.cInstance.symbolID == SymbolID.Subtraction)},
                    {SymbolID.Multiplication, saveFileList.FindAll(x => x.cInstance.symbolID == SymbolID.Multiplication)},
                    {SymbolID.Division, saveFileList.FindAll(x => x.cInstance.symbolID == SymbolID.Division)}
                };
        saveFileToggleGroup.SetAllTogglesOff();
        saveFileToggleGroup.toggles[0].isOn = true;
        GameManager.Instance.CurAction = RefreshSaveFileDict;
    }
    private void RefreshSummarySaveFilePage()
    {
        saveFileSummary.SetActive(true);

        RefreshLatestSaveFile(additionSaveFileItem, SymbolID.Addition, additionSummary_Text);
        RefreshLatestSaveFile(subtractionSaveFileItem, SymbolID.Subtraction, subtractionSummary_Text);
        RefreshLatestSaveFile(multiplicationSaveFileItem, SymbolID.Multiplication, multiplicationSummary_Text);
        RefreshLatestSaveFile(divisionSaveFileItem, SymbolID.Division, divisionSummary_Text);
    }
    private void RefreshLatestSaveFile(SummarySaveFileItem item, SymbolID symbolID, Text summary)
    {
        List<SaveFileInstance> instanceList = saveFileDict[symbolID];
        if (instanceList.Count > 0)
        {
            SaveFileInstance latestInstance = instanceList[instanceList.Count - 1];
            item.SendMessage("InitPrefabItem", latestInstance);
            item.SendMessage("InitDetailWin", saveFileDetailBg);
        }
        else
        {
            item.gameObject.SetActive(false);
        }
        summary.text = string.Format(rawSaveFileStringDict[symbolID], instanceList.Count, curSaveFileCount);
    }
    private void RefreshSaveFileList()
    {
        saveFileSummary.SetActive(false);
        SymbolID symbolID = (SymbolID)curSaveFileIndex;
        ArrayList dataList = new ArrayList(saveFileDict[symbolID]);
        saveFileGrid.InitList(dataList, "SaveFileItem", saveFileDetailBg, deleteSaveFileBg);
    }
    /// <summary>
    /// 用于删除存档时对saveFileDict进行更新，并更新显示
    /// 根据成就系统相关函数进行调整
    /// </summary>
    private void RefreshSaveFileDict()
    {
        SymbolID symbolID = (SymbolID)curSaveFileIndex;
        List<SaveFileInstance> saveFileList = GameManager.Instance.ReadAllRecords();
        curSaveFileCount = saveFileList.Count;
        saveFileDict[symbolID] = saveFileList.FindAll(x => x.cInstance.symbolID == symbolID);
        ArrayList dataList = new ArrayList(saveFileDict[symbolID]);
        saveFileGrid.InitList(dataList, "SaveFileItem", saveFileDetailBg, deleteSaveFileBg);
    }
    #endregion
}
