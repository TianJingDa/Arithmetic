﻿using System.Collections;
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
    private Text achievementBtn_Text2;
    private Text saveFileBtn_Text2;
    private Text additionSaveFile_Text;
    private Text subtractionSaveFile_Text;
    private Text multiplicationSaveFile_Text;
    private Text divisionSaveFile_Text;
    private Text juniorStatisticsItemData;
    private Text mediumStatisticsItemData;
    private Text seniorStatisticsItemData;
    private Text ultimateStatisticsItemData;
    private GameObject saveFileWin;
    private GameObject achievementWin;
    private GameObject saveFileSummary;
    private GameObject achievementSummary;
    private InfiniteList achievementGrid;
    private InfiniteList saveFileGrid;
    private ToggleGroup saveFileToggleGroup;
    private ToggleGroup achievementToggleGroup;
    private SummarySaveFileItem additionSaveFileItem;
    private SummarySaveFileItem subtractionSaveFileItem;
    private SummarySaveFileItem multiplicationSaveFileItem;
    private SummarySaveFileItem divisionSaveFileItem;
    private LastestAchievementItem lastestAchievementItem;
    //private SummaryAchievementItem[] summaryAchievementArray;
    //private HiddenAchievementItem hiddenAchievementItem;
    private List<Text> rawAchievementTextList;
    private Dictionary<SymbolID, List<SaveFileInstance>> saveFileDict;
    private Dictionary<DifficultyID, List<AchievementInstance>> achievementDict;

    void Start () 
	{
        id = GuiFrameID.StatisticsFrame;
        Init();
        int totalTime = (int)GameManager.Instance.TotalTime;
        TimeSpan ts = new TimeSpan(0, 0, totalTime);
        totelTimeImg_Text2.text = string.Format(totelTimeImg_Text2.text, ts.Hours, ts.Minutes, ts.Seconds);
        totelGameImg_Text2.text = string.Format(totelGameImg_Text2.text, GameManager.Instance.TotalGame);
        RefreshStatisticsContent();
        rawAchievementTextList = new List<Text>
        {
            juniorStatisticsItemData,
            mediumStatisticsItemData,
            seniorStatisticsItemData,
            ultimateStatisticsItemData
        };
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementWin                          = gameObjectDict["AchievementWin"];
        saveFileWin                             = gameObjectDict["SaveFileWin"];
        saveFileGrid                            = gameObjectDict["SaveFileGrid"].GetComponent<InfiniteList>();
        achievementGrid                         = gameObjectDict["AchievementGrid"].GetComponent<InfiniteList>();
        saveFileSummary                         = gameObjectDict["SaveFileSummary"];
        achievementSummary                      = gameObjectDict["AchievementSummary"];
        totelTimeImg_Text2                      = gameObjectDict["TotelTimeImg_Text2"].GetComponent<Text>();
        totelGameImg_Text2                      = gameObjectDict["TotelGameImg_Text2"].GetComponent<Text>();
        additionSaveFile_Text                   = gameObjectDict["AdditionSaveFile_Text"].GetComponent<Text>();
        subtractionSaveFile_Text                = gameObjectDict["SubtractionSaveFile_Text"].GetComponent<Text>();
        multiplicationSaveFile_Text             = gameObjectDict["MultiplicationSaveFile_Text"].GetComponent<Text>();
        divisionSaveFile_Text                   = gameObjectDict["DivisionSaveFile_Text"].GetComponent<Text>();
        achievementBtn_Text2                    = gameObjectDict["AchievementBtn_Text2"].GetComponent<Text>();
        saveFileBtn_Text2                       = gameObjectDict["SaveFileBtn_Text2"].GetComponent<Text>();
        juniorStatisticsItemData                = gameObjectDict["JuniorStatisticsItemData"].GetComponent<Text>();
        mediumStatisticsItemData                = gameObjectDict["MediumStatisticsItemData"].GetComponent<Text>();
        seniorStatisticsItemData                = gameObjectDict["SeniorStatisticsItemData"].GetComponent<Text>();
        ultimateStatisticsItemData              = gameObjectDict["UltimateStatisticsItemData"].GetComponent<Text>();
        additionSaveFileItem                    = gameObjectDict["AdditionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        subtractionSaveFileItem                 = gameObjectDict["SubtractionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        multiplicationSaveFileItem              = gameObjectDict["MultiplicationSaveFileItem"].GetComponent<SummarySaveFileItem>();
        divisionSaveFileItem                    = gameObjectDict["DivisionSaveFileItem"].GetComponent<SummarySaveFileItem>();
        lastestAchievementItem                  = gameObjectDict["LastestAchievementItem"].GetComponent<LastestAchievementItem>();
        //hiddenAchievementItem                   = gameObjectDict["HiddenAchievementItem"].GetComponent<HiddenAchievementItem>();
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
                GameManager.Instance.SwitchWrapperWithMove(GuiFrameID.StartFrame, MoveID.LeftOrDown, false);
                break;
            case "Achievement2StatisticsFrameBtn":
                GameManager.Instance.CurAction = null;
                RefreshStatisticsContent();
                CommonTool.GuiHorizontalMove(achievementWin, Screen.width, MoveID.LeftOrDown, canvasGroup, false);
                break;
            case "AchievementBtn":
                achievementWin.SetActive(true);
                RefreshAchievementWin();
                CommonTool.GuiHorizontalMove(achievementWin, Screen.width, MoveID.LeftOrDown, canvasGroup, true);
                break;
            case "Save2StatisticsFrameBtn":
                GameManager.Instance.CurAction = null;
                RefreshStatisticsContent();
                CommonTool.GuiHorizontalMove(saveFileWin, Screen.width, MoveID.LeftOrDown, canvasGroup, false);
                break;
            case "SaveFileBtn":
                saveFileWin.SetActive(true);
                RefreshSaveFileWin();
                CommonTool.GuiHorizontalMove(saveFileWin, Screen.width, MoveID.LeftOrDown, canvasGroup, true);
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
    private void ShareImage(GameObject target, PlatformType type)
    {
        RectTransform shotTarget = target.transform as RectTransform;
        Rect shotRect = CommonTool.GetShotTargetRect(shotTarget);
        GameManager.Instance.ShareImage(shotRect, type);
    }

    private void RefreshStatisticsContent()
    {
        string achievementData = GameManager.Instance.GetMutiLanguage(achievementBtn_Text2.index);
        achievementBtn_Text2.text = string.Format(achievementData, CommonTool.CalculateAllStar());
        List<SaveFileInstance> saveFileList = GameManager.Instance.ReadAllRecords();
        saveFileBtn_Text2.text = saveFileList.Count.ToString();
    }

    #region 成就
    private void RefreshAchievementWin()
    {
        achievementWin.SetActive(true);
        //if (summaryAchievementArray == null) summaryAchievementArray = achievementSummary.GetComponentsInChildren<SummaryAchievementItem>(true);
        List<AchievementInstance> achievementList = GameManager.Instance.GetAllAchievements();
        achievementDict = new Dictionary<DifficultyID, List<AchievementInstance>>
                {
                    {DifficultyID.Junior, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Junior)},
                    {DifficultyID.Medium, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Medium)},
                    {DifficultyID.Senior, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Senior)},
                    {DifficultyID.Ultimate, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Ultimate)},
                    //{SymbolID.Summary, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Summary)},
                    //{SymbolID.Hidden, achievementList.FindAll(x => x.cInstance.symbolID == SymbolID.Hidden)}
                };
        achievementToggleGroup.SetAllTogglesOff();
        achievementToggleGroup.toggles[0].isOn = true;
        GameManager.Instance.CurAction = RefreshAchievementDict;
    }
    private void RefreshSummaryAchievementPage()
    {
        achievementSummary.SetActive(true);
        RefreshLastestAchievement();
        //RefreshSummaryAchievement();
        RefreshAchievementStatistics();
        //RefreshHiddenAchievement();
    }
    private void RefreshAchievementStatistics()
    {
        for(int i = 0; i < rawAchievementTextList.Count; i++)
        {
            string data = GameManager.Instance.GetMutiLanguage(rawAchievementTextList[i].index);
            rawAchievementTextList[i].text = string.Format(data, CommonTool.CalculateStar(achievementDict[(DifficultyID)i]));
        }
    }

    private void RefreshLastestAchievement()
    {
        AchievementInstance lastestAchievement = GameManager.Instance.GetLastestAchievement();
        lastestAchievementItem.SendMessage("InitPrefabItem", lastestAchievement);
    }
    //private void RefreshSummaryAchievement()
    //{
        //for(int i = 0; i < summaryAchievementArray.Length; i++)
        //{
        //    summaryAchievementArray[i].SendMessage("InitPrefabItem", achievementDict[SymbolID.Summary][i]);
        //    summaryAchievementArray[i].SendMessage("InitDetailWin", achievementDetailBgInStatistics);
        //}
    //}
    //private void RefreshHiddenAchievement()
    //{
        //bool finishAllAchievement = GameManager.Instance.FinishAllAchievement;
        //hiddenAchievementItem.gameObject.SetActive(finishAllAchievement);
        //if (finishAllAchievement)
        //{
        //    AchievementInstance hiddenAchievement = GameManager.Instance.GetAllAchievements().Find(x => x.cInstance.symbolID == SymbolID.Hidden);
        //    hiddenAchievementItem.SendMessage("InitPrefabItem", hiddenAchievement);
        //    hiddenAchievementItem.SendMessage("InitDetailWin", achievementDetailBgInStatistics);
        //}
    //}
    private void RefreshAchievementList()
    {
        achievementSummary.SetActive(false);
        ArrayList dataList = new ArrayList(achievementDict[(DifficultyID)curAchievementIndex]);
        achievementGrid.InitList(dataList, GuiItemID.AchievementItem);
    }
    private void RefreshAchievementDict()
    {
        List<AchievementInstance> achievementList = GameManager.Instance.GetAllAchievements();
        achievementDict[(DifficultyID)curAchievementIndex] = achievementList.FindAll(x => x.difficulty == curAchievementIndex);
        ArrayList dataList = new ArrayList(achievementDict[(DifficultyID)curAchievementIndex]);
        achievementGrid.InitList(dataList, GuiItemID.AchievementItem);
    }

    #endregion

    #region 存档
    private void RefreshSaveFileWin()
    {
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

        RefreshLatestSaveFile(additionSaveFileItem, SymbolID.Addition, additionSaveFile_Text);
        RefreshLatestSaveFile(subtractionSaveFileItem, SymbolID.Subtraction, subtractionSaveFile_Text);
        RefreshLatestSaveFile(multiplicationSaveFileItem, SymbolID.Multiplication, multiplicationSaveFile_Text);
        RefreshLatestSaveFile(divisionSaveFileItem, SymbolID.Division, divisionSaveFile_Text);
    }
    private void RefreshLatestSaveFile(SummarySaveFileItem item, SymbolID symbolID, Text summary)
    {
        List<SaveFileInstance> instanceList = saveFileDict[symbolID];
        GameObject saveFileDefaultItem = CommonTool.GetGameObjectContainsName(item.gameObject, "SaveFileDefaultItem");
        saveFileDefaultItem.SetActive(instanceList.Count <= 0);
        if (instanceList.Count > 0)
        {
            SaveFileInstance latestInstance = instanceList[instanceList.Count - 1];
            item.SendMessage("InitPrefabItem", latestInstance);
        }
        string content = GameManager.Instance.GetMutiLanguage(summary.index);
        summary.text = string.Format(content, instanceList.Count, curSaveFileCount);
    }
    private void RefreshSaveFileList()
    {
        saveFileSummary.SetActive(false);
        SymbolID symbolID = (SymbolID)curSaveFileIndex;
        ArrayList dataList = new ArrayList(saveFileDict[symbolID]);
        saveFileGrid.InitList(dataList, GuiItemID.SaveFileItem);
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
        saveFileGrid.InitList(dataList, GuiItemID.SaveFileItem);
    }
    #endregion
}
