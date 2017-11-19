using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using cn.sharesdk.unity3d;

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
    private Text saveFileShareTitleInStatistics;
    private Text achievementShareTitleInStatistics;
    private GameObject saveFileWin;
    private GameObject achievementWin;
    private GameObject saveFileDetailBg;
    private GameObject deleteSaveFileBg;
    private GameObject deleteAchievementBg;
    private GameObject saveFileSummary;
    private GameObject achievementSummary;
    private GameObject saveFileDetailBgOfAchievement;
    private GameObject saveFileShareWinInStatistics;
    private GameObject saveFileSharePageInStatistics;
    private GameObject achievementDetailBgInSaveFile;
    private GameObject achievementDetailBgInStatistics;
    private GameObject achievementShareWinInStatistics;
    private GameObject achievementShareDetailBgInStatistics;
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

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementWin                          = gameObjectDict["AchievementWin"];
        saveFileWin                             = gameObjectDict["SaveFileWin"];
        saveFileGrid                            = gameObjectDict["SaveFileGrid"].GetComponent<InfiniteList>();
        achievementGrid                         = gameObjectDict["AchievementGrid"].GetComponent<InfiniteList>();
        saveFileDetailBg                        = gameObjectDict["SaveFileDetailBg"];
        deleteSaveFileBg                        = gameObjectDict["DeleteSaveFileBg"];
        deleteAchievementBg                     = gameObjectDict["DeleteAchievementBg"];
        saveFileDetailBgOfAchievement           = gameObjectDict["SaveFileDetailBgOfAchievement"];
        saveFileSummary                         = gameObjectDict["SaveFileSummary"];
        achievementSummary                      = gameObjectDict["AchievementSummary"];
        totelTimeImg_Text2                      = gameObjectDict["TotelTimeImg_Text2"].GetComponent<Text>();
        totelGameImg_Text2                      = gameObjectDict["TotelGameImg_Text2"].GetComponent<Text>();
        additionSummary_Text                    = gameObjectDict["AdditionSummary_Text"].GetComponent<Text>();
        subtractionSummary_Text                 = gameObjectDict["SubtractionSummary_Text"].GetComponent<Text>();
        multiplicationSummary_Text              = gameObjectDict["MultiplicationSummary_Text"].GetComponent<Text>();
        divisionSummary_Text                    = gameObjectDict["DivisionSummary_Text"].GetComponent<Text>();
        achievementDetailBgInSaveFile           = gameObjectDict["AchievementDetailBgInSaveFile"];
        achievementDetailBgInStatistics         = gameObjectDict["AchievementDetailBgInStatistics"];
        achievementShareWinInStatistics         = gameObjectDict["AchievementShareWinInStatistics"];
        achievementShareDetailBgInStatistics    = gameObjectDict["AchievementShareDetailBgInStatistics"];
        saveFileShareWinInStatistics            = gameObjectDict["SaveFileShareWinInStatistics"];
        saveFileSharePageInStatistics           = gameObjectDict["SaveFileSharePageInStatistics"];
        additionSaveFileItem                    = gameObjectDict["AdditionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        subtractionSaveFileItem                 = gameObjectDict["SubtractionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        multiplicationSaveFileItem              = gameObjectDict["MultiplicationSaveFileItem"].GetComponent<SummarySaveFileItem>();
        divisionSaveFileItem                    = gameObjectDict["DivisionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        lastestAchievementItem                  = gameObjectDict["LastestAchievementItem"].GetComponent<LastestAchievementItem>();
        hiddenAchievementItem                   = gameObjectDict["HiddenAchievementItem"].GetComponent<HiddenAchievementItem>();
        saveFileToggleGroup                     = gameObjectDict["SaveFileToggleGroup"].GetComponent<ToggleGroup>();
        achievementToggleGroup                  = gameObjectDict["AchievementToggleGroup"].GetComponent<ToggleGroup>();
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
            case "AchievementShareDetailBgInStatistics":
                achievementShareWinInStatistics.SetActive(false);
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
            case "SaveFileSharePageInStatistics":
                saveFileShareWinInStatistics.SetActive(false);
                break;
            case "WeChatBtnOfSaveFileInStatistics":
                if (GameManager.Instance.UserNameOfWeChat == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
                    {
                        ShareImage(saveFileSharePageInStatistics, PlatformType.WeChat, true);
                    });
                }
                else
                {
                    ShareImage(saveFileSharePageInStatistics, PlatformType.WeChat, true);
                }
                break;
            case "WeChatMomentsBtnOfSaveFileInStatistics":
                if (GameManager.Instance.UserNameOfWeChat == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
                    {
                        ShareImage(saveFileSharePageInStatistics, PlatformType.WeChatMoments, true);
                    });
                }
                else
                {
                    ShareImage(saveFileSharePageInStatistics, PlatformType.WeChatMoments, true);
                }
                break;
            case "SinaWeiboBtnOfSaveFileInStatistics":
                if (GameManager.Instance.UserNameOfSinaWeibo == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.SinaWeibo, () =>
                    {
                        ShareImage(saveFileSharePageInStatistics, PlatformType.SinaWeibo, true);
                    });
                }
                else
                {
                    ShareImage(saveFileSharePageInStatistics, PlatformType.SinaWeibo, true);
                }
                break;
            case "WeChatBtnOfAchievementInStatistics":
                if(GameManager.Instance.UserNameOfWeChat == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat,() =>
                    {
                        ShareImage(achievementShareDetailBgInStatistics, PlatformType.WeChat, false);
                    });
                }
                else
                {
                    ShareImage(achievementShareDetailBgInStatistics, PlatformType.WeChat, false);
                }
                break;
            case "WeChatMomentsBtnOfAchievementInStatistics":
                if (GameManager.Instance.UserNameOfWeChat == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
                    {
                        ShareImage(achievementShareDetailBgInStatistics, PlatformType.WeChatMoments, false);
                    });
                }
                else
                {
                    ShareImage(achievementShareDetailBgInStatistics, PlatformType.WeChatMoments, false);
                }
                break;
            case "SinaWeiboBtnOfAchievementInStatistics":
                if (GameManager.Instance.UserNameOfSinaWeibo == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.SinaWeibo, () =>
                    {
                        ShareImage(achievementShareDetailBgInStatistics, PlatformType.SinaWeibo, false);
                    });
                }
                else
                {
                    ShareImage(achievementShareDetailBgInStatistics, PlatformType.SinaWeibo, false);
                }
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
    private void ShareImage(GameObject target, PlatformType type, bool isSaveFile)
    {
        if (isSaveFile)
        {
            saveFileShareTitleInStatistics.enabled = true;
            if (type == PlatformType.WeChat || type == PlatformType.WeChatMoments)
                saveFileShareTitleInStatistics.text = string.Format(saveFileShareTitleInStatistics.text, GameManager.Instance.UserNameOfWeChat);
            else
                saveFileShareTitleInStatistics.text = string.Format(saveFileShareTitleInStatistics.text, GameManager.Instance.UserNameOfSinaWeibo);
        }
        else
        {
            achievementShareTitleInStatistics.enabled = true;
            if (type == PlatformType.WeChat || type == PlatformType.WeChatMoments)
                achievementShareTitleInStatistics.text = string.Format(achievementShareTitleInStatistics.text, GameManager.Instance.UserNameOfWeChat);
            else
                achievementShareTitleInStatistics.text = string.Format(achievementShareTitleInStatistics.text, GameManager.Instance.UserNameOfSinaWeibo);
        }
        RectTransform shotTarget = target.transform as RectTransform;
        Rect shotRect = CommonTool.GetShotTargetRect(shotTarget);
        GameManager.Instance.ShareImage(shotRect, type);
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
        RefreshHiddenAchievement();
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
    private void RefreshHiddenAchievement()
    {
        bool finishAllAchievement = GameManager.Instance.FinishAllAchievement;
        hiddenAchievementItem.gameObject.SetActive(finishAllAchievement);
        if (finishAllAchievement)
        {
            AchievementInstance hiddenAchievement = achievementDict[SymbolID.Hidden][0];
            hiddenAchievementItem.SendMessage("InitPrefabItem", hiddenAchievement);
            hiddenAchievementItem.SendMessage("InitDetailWin", achievementDetailBgInStatistics);
        }
    }
    private void RefreshAchievementList()
    {
        achievementSummary.SetActive(false);
        SymbolID symbolID = (SymbolID)curAchievementIndex;
        ArrayList dataList = new ArrayList(achievementDict[symbolID]);
        achievementGrid.InitList(dataList, "AchievementItem", achievementDetailBgInStatistics, deleteAchievementBg);
    }
    private void RefreshAchievementDict()
    {
        SymbolID symbolID = (SymbolID)curAchievementIndex;
        List<AchievementInstance> achievementList = GameManager.Instance.GetAllAchievements();
        achievementDict[symbolID] = achievementList.FindAll(x => x.cInstance.symbolID == symbolID);
        ArrayList dataList = new ArrayList(achievementDict[symbolID]);
        achievementGrid.InitList(dataList, "AchievementItem", achievementDetailBgInStatistics, deleteAchievementBg);
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
