using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChapterFrameWrapper : GuiFrameWrapper
{
    private Text chapterStarStatisticsImg_Text;
    private GameObject chapterWin;
    private GameObject chapterTipBg;
    private GameObject chapterDetailWin;
    private ChapterItem[] chapterItemList;
    private Dictionary<DifficultyID, List<AchievementInstance>> achievementDict;


    void Start ()
    {
        id = GuiFrameID.ChapterFrame;
        Init();
        chapterItemList = chapterWin.GetComponentsInChildren<ChapterItem>();
        List<AchievementInstance> achievementList = GameManager.Instance.GetAllAchievements();
        achievementDict = new Dictionary<DifficultyID, List<AchievementInstance>>
                {
                    {DifficultyID.Junior, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Junior)},
                    {DifficultyID.Medium, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Medium)},
                    {DifficultyID.Senior, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Senior)},
                    {DifficultyID.Ultimate, achievementList.FindAll(x => x.difficulty == (int)DifficultyID.Ultimate)},
                };
        chapterStarStatisticsImg_Text.text = string.Format(chapterStarStatisticsImg_Text.text, CommonTool.CalculateAllStar());
        List<GameObject> lockList = CommonTool.GetGameObjectsContainName(gameObject, "Lock");
        //lockList[0].SetActive(false);
        for(int i = 1; i < lockList.Count; i++)
        {
            int star = CommonTool.CalculateStar(achievementDict[(DifficultyID)(i - 1)]);
            lockList[i].SetActive(star < 8);
        }
        List<Text> classBtnTextList = CommonTool.GetComponentsContainName<Text>(gameObject, "ClassBtn_Text");
        for(int i = 0; i < classBtnTextList.Count; i++)
        {
            int star = CommonTool.CalculateStar(achievementDict[(DifficultyID)i]);
            classBtnTextList[i].text = string.Format(classBtnTextList[i].text, star);
        }
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        chapterWin                      = gameObjectDict["ChapterWin"];
        chapterTipBg                    = gameObjectDict["ChapterTipBg"];
        chapterDetailWin                = gameObjectDict["ChapterDetailWin"];
        chapterStarStatisticsImg_Text   = gameObjectDict["ChapterStarStatisticsImg_Text"].GetComponent<Text>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Chapter2StartFrameBtn":
            case "ChapterWin2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, false);
                break;
            case "ChapterWin2ChapterFrameBtn":
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "ChapterTipBg":
            case "ChapterConfirmBtn":
                chapterTipBg.SetActive(false);
                break;
            case "MediumClassLock":
            case "SeniorClassLock":
            case "UltimateClassLock":
                chapterTipBg.SetActive(true);
                break;
            case "ChapterDetailWin":
                CommonTool.GuiScale(chapterDetailWin, canvasGroup, false);
                break;
            case "JuniorClassBtn":
                chapterWin.SetActive(true);
                InitAllChapterItem(achievementDict[DifficultyID.Junior]);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "MediumClassBtn":
                chapterWin.SetActive(true);
                InitAllChapterItem(achievementDict[DifficultyID.Medium]);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "SeniorClassBtn":
                chapterWin.SetActive(true);
                InitAllChapterItem(achievementDict[DifficultyID.Senior]);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "UltimateClassBtn":
                chapterWin.SetActive(true);
                InitAllChapterItem(achievementDict[DifficultyID.Ultimate]);
                CommonTool.GuiHorizontalMove(chapterWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            default:
                MyDebug.LogYellow("Can not find Button:" + btn.name);
                break;
        }
    }

    private void InitAllChapterItem(List<AchievementInstance> instanceList)
    {
        for(int i = 0; i < chapterItemList.Length; i++)
        {
            chapterItemList[i].SendMessage("InitItem", instanceList[i]);
            chapterItemList[i].SendMessage("InitDetailWin", chapterDetailWin);
        }
    }
}
