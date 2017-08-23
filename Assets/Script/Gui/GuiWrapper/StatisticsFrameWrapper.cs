using System.Collections;
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
    private InfiniteList achievementGrid;
    void Start () 
	{
        base.id = GuiFrameID.StatisticsFrame;
        InitGui();
        achievementWin = CommonTool.GetGameObjectByName(gameObject, "AchievementWin");
        saveFileWin = CommonTool.GetGameObjectByName(gameObject, "SaveFileWin");
        achievementGrid = CommonTool.GetComponentByName<InfiniteList>(gameObject, "AchievementGrid");
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
            case "Statistics2StartFrameBtn":
            case "Achievement2StartFrameBtn":
            case "Save2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StatisticsFrame, GuiFrameID.StartFrame);
                break;
            case "AchievementBtn":
                InitInfiniteList();
                //achievementGrid.InitList()
                //achievementWin.SetActive(true);
                break;
            case "SaveFileBtn":
                saveFileWin.SetActive(true);
                break;
            case "Achievement2StatisticsFrameBtn":
                achievementWin.SetActive(false);
                break;
            case "Save2StatisticsFrameBtn":
                saveFileWin.SetActive(false);
                break;
            default:
                Debug.LogError("Can not find Button:" + btn.name);
                break;
        }
    }

    private void InitInfiniteList()
    {
        ArrayList dataList = new ArrayList();
        for(int i = 0; i < 30; i++)
        {
            AchievementInstance instance = new AchievementInstance();
            instance.title = i.ToString();
            dataList.Add(instance);
        }
        achievementGrid.InitList(dataList, "AchievementItem");
        achievementWin.SetActive(true);
    }
}
