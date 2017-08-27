﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 统计数据界面
/// </summary>
public class StatisticsFrameWrapper : GuiFrameWrapper
{
    private GameObject achievementWin;
    private GameObject saveFileWin;
    private GameObject achievementDetailBg;
    private GameObject saveFileDetailScrollRect;
    private InfiniteList achievementGrid;
    private InfiniteList saveFileGrid;
    void Start () 
	{
        base.id = GuiFrameID.StatisticsFrame;
        InitGui();
        achievementWin = CommonTool.GetGameObjectByName(gameObject, "AchievementWin");
        saveFileWin = CommonTool.GetGameObjectByName(gameObject, "SaveFileWin");
        achievementDetailBg = CommonTool.GetGameObjectByName(gameObject, "AchievementDetailBg");
        saveFileDetailScrollRect = CommonTool.GetGameObjectByName(gameObject, "SaveFileDetailScrollRect");
        achievementGrid = CommonTool.GetComponentByName<InfiniteList>(gameObject, "AchievementGrid");
        saveFileGrid = CommonTool.GetComponentByName<InfiniteList>(gameObject, "SaveFileGrid");
    }

    void Update () 
	{
		
	}

    void OnDestroy()
    {
        
    }

    public override void OnClick(Button btn)
    {
        base.OnClick(btn);
        switch (btn.name)
        {
            case "Achievement2StartFrameBtn":
            case "Save2StartFrameBtn":
            case "Statistics2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StatisticsFrame, GuiFrameID.StartFrame);
                break;
            case "Achievement2StatisticsFrameBtn":
                achievementWin.SetActive(false);
                break;
            case "AchievementBtn":
                InitAchievementList();
                break;
            case "AchievementDetailBg":
                achievementDetailBg.SetActive(false);
                break;
            case "Save2StatisticsFrameBtn":
                saveFileWin.SetActive(false);
                break;
            case "SaveFileBtn":
                InitSaveFileList();
                break;
            default:
                Debug.LogError("Can not find Button:" + btn.name);
                break;
        }
    }

    private void InitAchievementList()
    {
        achievementWin.SetActive(true);
        ArrayList dataList = new ArrayList();
        for(int i = 0; i < 30; i++)
        {
            AchievementInstance instance = new AchievementInstance();
            instance.title = i.ToString();
            instance.detail = i.ToString();
            dataList.Add(instance);
        }
        achievementGrid.InitList(dataList, "AchievementItem", achievementDetailBg);
    }

    private void InitSaveFileList()
    {
        saveFileWin.SetActive(true);
        ArrayList dataList = new ArrayList();
        for (int i = 0; i < 30; i++)
        {
            SaveFileInstance instance = new SaveFileInstance();
            instance.title = i.ToString();
            dataList.Add(instance);
        }
        saveFileGrid.InitList(dataList, "SaveFileItem" , saveFileDetailScrollRect);
    }

}