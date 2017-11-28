using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChapterFrameWrapper : GuiFrameWrapper
{
    private GameObject chapterWin;
    private GameObject chapterDetailWin;
    private InfiniteList chapterGrid;

    void Start ()
    {
        id = GuiFrameID.ChapterFrame;
        Init();

    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        chapterWin = gameObjectDict["ChapterWin"];
        chapterDetailWin = gameObjectDict["ChapterDetailWin"];
        chapterGrid = gameObjectDict["ChapterGrid"].GetComponent<InfiniteList>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Chapter2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, false);
                break;
            case "ChapterWin2ChapterFrameBtn":
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "ChapterDetailWin":
                CommonTool.GuiScale(chapterDetailWin, canvasGroup, false);
                break;
            case "JuniorClassBtn":
                chapterWin.SetActive(true);
                List<AchievementInstance> list = GameManager.Instance.GetAllAchievements().FindAll(x => x.cInstance.symbolID == SymbolID.Addition);
                ArrayList dataList = new ArrayList(list);
                chapterGrid.InitList(dataList, "ChapterItem", chapterDetailWin);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "MediumClassBtn":
                chapterWin.SetActive(true);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "SeniorClassBtn":
                chapterWin.SetActive(true);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "UltimateClassBtn":
                chapterWin.SetActive(true);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            default:
                MyDebug.LogYellow("Can not find Button:" + btn.name);
                break;
        }
    }
}
