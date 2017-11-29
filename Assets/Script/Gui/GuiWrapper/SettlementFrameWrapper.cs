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

    private Text settlementTime;
    private Text settlementAmount;
    private Text settlementAccuracy;
    private Text achievementDetailMainTitleInSettlement;
    private Text achievementDetailSubTitleInSettlement;
    private Text achievementDetailFinishTimeInSettlement;
    private Image achievementDetailImageInSettlement;
    //private GameObject curAchievementBtn;
    private GameObject onlyWrongImage;
    private GameObject achievementDetailBgInSettlement;
    private GameObject achievementShareWinInSettlement;
    private GameObject achievementShareDetailBgInSettlement;
    private GameObject saveFileShareWinInSettlement;
    private GameObject saveFileSharePageInSettlement;
    private InfiniteList settlementGrid;
    private SaveFileInstance curSaveFileInstance;
    private List<QuentionInstance> onlyWrongList;
    private List<QuentionInstance> allInstanceList;

    //private Text saveFileShareTitleInSettlement;
    //private GameObject saveFileSharePatternInSettlement_Time;
    //private GameObject saveFileSharePatternInSettlement_Number;
    //private Text saveFileShareAmountInSettlement;
    //private Text saveFileShareTimeInSettlement;
    //private Text saveFileShareSymbolInSettlement;
    //private Text saveFileShareDigitInSettlement;
    //private Text saveFileShareOperandInSettlement;
    //private Text saveFileShareAccuracyInSettlement;
    //private Text saveFileShareMeanTimeInSettlement;
    //private Image saveFileShareImageInSettlement;
    //private Text saveFileShareMainTitleInSettlement;
    //private Text saveFileShareSubTitleInSettlement;
    //private Text saveFileShareTypeInSettlement;
    //private Text saveFileShareFinishTimeInSettlement;
    //private Text saveFileShareConditionInSettlement;
    //private GameObject saveFileShareAchievementInSettlement;
    //private GameObject saveFileShareWithoutAchievementInSettlement;

    //private Text achievementShareTitleInSettlement;
    //private Image achievementShareImageInSettlement;
    //private Text achievementShareMainTitleInSettlement;
    //private Text achievementShareSubTitleInSettlement;
    //private Text achievementShareTypeInSettlement;
    //private Text achievementShareFinishTimeInSettlement;
    //private Text achievementShareConditionInSettlement;
    //private GameObject achievementSharePatternInSettlement_Time;
    //private GameObject achievementSharePatternInSettlement_Number;
    //private Text achievementShareAmountInSettlement;
    //private Text achievementShareTimeInSettlement;
    //private Text achievementShareSymbolInSettlement;
    //private Text achievementShareDigitInSettlement;
    //private Text achievementShareOperandInSettlement;
    //private Text achievementShareAccuracyInSettlement;
    //private Text achievementShareMeanTimeInSettlement;


    void Start () 
	{
        id = GuiFrameID.SettlementFrame;
        Init();
        InitSettlement();
        InitAchievement();
        //InitSaveFileShareWin();
        //InitAchievementShareWin();
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        settlementGrid                              = gameObjectDict["SettlementGrid"].GetComponent<InfiniteList>();
        settlementTime                              = gameObjectDict["SettlementTime"].GetComponent<Text>();
        settlementAmount                            = gameObjectDict["SettlementAmount"].GetComponent<Text>();
        settlementAccuracy                          = gameObjectDict["SettlementAccuracy"].GetComponent<Text>();
        achievementDetailMainTitleInSettlement      = gameObjectDict["AchievementDetailMainTitleInSettlement"].GetComponent<Text>();
        achievementDetailSubTitleInSettlement       = gameObjectDict["AchievementDetailSubTitleInSettlement"].GetComponent<Text>();
        achievementDetailFinishTimeInSettlement     = gameObjectDict["AchievementDetailFinishTimeInSettlement"].GetComponent<Text>();
        achievementDetailBgInSettlement             = gameObjectDict["AchievementDetailBgInSettlement"];
        achievementDetailImageInSettlement          = gameObjectDict["AchievementDetailImageInSettlement"].GetComponent<Image>();
        //curAchievementBtn                           = gameObjectDict["CurAchievementBtn"];
        onlyWrongImage                              = gameObjectDict["OnlyWrongImage"];
        saveFileShareWinInSettlement                = gameObjectDict["SaveFileShareWinInSettlement"];
        achievementShareWinInSettlement             = gameObjectDict["AchievementShareWinInSettlement"];
        saveFileSharePageInSettlement               = gameObjectDict["SaveFileSharePageInSettlement"];
        achievementShareDetailBgInSettlement        = gameObjectDict["AchievementShareDetailBgInSettlement"];

        //saveFileShareTitleInSettlement              = gameObjectDict["SaveFileShareTitleInSettlement"].GetComponent<Text>();
        //saveFileSharePatternInSettlement_Time       = gameObjectDict["SaveFileSharePatternInSettlement_Time"];
        //saveFileSharePatternInSettlement_Number     = gameObjectDict["SaveFileSharePatternInSettlement_Number"];
        //saveFileShareAmountInSettlement             = gameObjectDict["SaveFileShareAmountInSettlement"].GetComponent<Text>();
        //saveFileShareTimeInSettlement               = gameObjectDict["SaveFileShareTimeInSettlement"].GetComponent<Text>();
        //saveFileShareSymbolInSettlement             = gameObjectDict["SaveFileShareSymbolInSettlement"].GetComponent<Text>();
        //saveFileShareDigitInSettlement              = gameObjectDict["SaveFileShareDigitInSettlement"].GetComponent<Text>();
        //saveFileShareOperandInSettlement            = gameObjectDict["SaveFileShareOperandInSettlement"].GetComponent<Text>();
        //saveFileShareAccuracyInSettlement           = gameObjectDict["SaveFileShareAccuracyInSettlement"].GetComponent<Text>();
        //saveFileShareMeanTimeInSettlement           = gameObjectDict["SaveFileShareMeanTimeInSettlement"].GetComponent<Text>();
        //saveFileShareImageInSettlement              = gameObjectDict["SaveFileShareImageInSettlement"].GetComponent<Image>();
        //saveFileShareMainTitleInSettlement          = gameObjectDict["SaveFileShareMainTitleInSettlement"].GetComponent<Text>();
        //saveFileShareSubTitleInSettlement           = gameObjectDict["SaveFileShareSubTitleInSettlement"].GetComponent<Text>();
        //saveFileShareTypeInSettlement               = gameObjectDict["SaveFileShareTypeInSettlement"].GetComponent<Text>();
        //saveFileShareFinishTimeInSettlement         = gameObjectDict["SaveFileShareFinishTimeInSettlement"].GetComponent<Text>();
        //saveFileShareConditionInSettlement          = gameObjectDict["SaveFileShareConditionInSettlement"].GetComponent<Text>();
        //saveFileShareAchievementInSettlement        = gameObjectDict["SaveFileShareAchievementInSettlement"];
        //saveFileShareWithoutAchievementInSettlement = gameObjectDict["SaveFileShareWithoutAchievementInSettlement"];

        //achievementShareTitleInSettlement           = gameObjectDict["AchievementShareImageInSettlement"].GetComponent<Text>();
        //achievementShareImageInSettlement           = gameObjectDict["AchievementShareImageInSettlement"].GetComponent<Image>();
        //achievementShareMainTitleInSettlement       = gameObjectDict["AchievementShareMainTitleInSettlement"].GetComponent<Text>();
        //achievementShareSubTitleInSettlement        = gameObjectDict["AchievementShareSubTitleInSettlement"].GetComponent<Text>();
        //achievementShareTypeInSettlement            = gameObjectDict["AchievementShareTypeInSettlement"].GetComponent<Text>();
        //achievementShareFinishTimeInSettlement      = gameObjectDict["AchievementShareFinishTimeInSettlement"].GetComponent<Text>();
        //achievementShareConditionInSettlement       = gameObjectDict["AchievementShareConditionInSettlement"].GetComponent<Text>();
        //achievementSharePatternInSettlement_Time    = gameObjectDict["AchievementSharePatternInSettlement_Time"];
        //achievementSharePatternInSettlement_Number  = gameObjectDict["AchievementSharePatternInSettlement_Number"];
        //achievementShareAmountInSettlement          = gameObjectDict["AchievementShareAmountInSettlement"].GetComponent<Text>();
        //achievementShareTimeInSettlement            = gameObjectDict["AchievementShareTimeInSettlement"].GetComponent<Text>();
        //achievementShareSymbolInSettlement          = gameObjectDict["AchievementShareSymbolInSettlement"].GetComponent<Text>();
        //achievementShareDigitInSettlement           = gameObjectDict["AchievementShareDigitInSettlement"].GetComponent<Text>();
        //achievementShareOperandInSettlement         = gameObjectDict["AchievementShareOperandInSettlement"].GetComponent<Text>();
        //achievementShareAccuracyInSettlement        = gameObjectDict["AchievementShareAccuracyInSettlement"].GetComponent<Text>();
        //achievementShareMeanTimeInSettlement        = gameObjectDict["AchievementShareMeanTimeInSettlement"].GetComponent<Text>();

    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetailBgInSettlement":
                achievementDetailBgInSettlement.SetActive(false);
                break;
            case "AchievementDetailShareBtnInSettlement":
                achievementShareWinInSettlement.SetActive(true);
                break;
            case "AchievementShareDetailBgInSettlement":
                achievementShareWinInSettlement.SetActive(false);
                break;
            case "CurAchievementBtn":
                achievementDetailBgInSettlement.SetActive(true);
                break;
            case "OnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "SaveFileSharePageInSettlement":
                saveFileShareWinInSettlement.SetActive(false);
                break;
            case "ShareBtnInSettlement":
                saveFileShareWinInSettlement.SetActive(true);
                break;
            case "Settlement2CategoryFrameBtn":
                if (GameManager.Instance.IsFromCategory) GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, MoveID.RightOrUp, false);
                else GameManager.Instance.SwitchWrapper(GuiFrameID.ChapterFrame, MoveID.RightOrUp, false);
                break;
            case "Settlement2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame,false);
                break;
            //case "WeChatBtnOfSaveFileInSettlement":
            //    if (string.IsNullOrEmpty(GameManager.Instance.UserNameOfWeChat))
            //    {
            //        GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
            //        {
            //            ShareImage(saveFileSharePageInSettlement, PlatformType.WeChat, true);
            //        });
            //    }
            //    else
            //    {
            //        ShareImage(saveFileSharePageInSettlement, PlatformType.WeChat, true);
            //    }
            //    break;
            //case "WeChatMomentsBtnOfSaveFileInSettlement":
            //    if (string.IsNullOrEmpty(GameManager.Instance.UserNameOfWeChat))
            //    {
            //        GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
            //        {
            //            ShareImage(saveFileSharePageInSettlement, PlatformType.WeChatMoments, true);
            //        });
            //    }
            //    else
            //    {
            //        ShareImage(saveFileSharePageInSettlement, PlatformType.WeChatMoments, true);
            //    }
            //    break;
            //case "SinaWeiboBtnOfSaveFileInSettlement":
            //    if (string.IsNullOrEmpty(GameManager.Instance.UserNameOfSinaWeibo))
            //    {
            //        GameManager.Instance.InitShareInfo(PlatformType.SinaWeibo, () =>
            //        {
            //            ShareImage(saveFileSharePageInSettlement, PlatformType.SinaWeibo, true);
            //        });
            //    }
            //    else
            //    {
            //        ShareImage(saveFileSharePageInSettlement, PlatformType.SinaWeibo, true);
            //    }
            //    break;
            //case "WeChatBtnOfAchievementInSettlement":
            //    if (string.IsNullOrEmpty(GameManager.Instance.UserNameOfWeChat))
            //    {
            //        GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
            //        {
            //            ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChat, false);
            //        });
            //    }
            //    else
            //    {
            //        ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChat, false);
            //    }
            //    break;
            //case "WeChatMomentsBtnOfAchievementInSettlement":
            //    if (string.IsNullOrEmpty(GameManager.Instance.UserNameOfWeChat))
            //    {
            //        GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
            //        {
            //            ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChatMoments,false);
            //        });
            //    }
            //    else
            //    {
            //        ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChatMoments,false);
            //    }
            //    break;
            //case "SinaWeiboBtnOfAchievementInSettlement":
            //    if (string.IsNullOrEmpty(GameManager.Instance.UserNameOfSinaWeibo))
            //    {
            //        GameManager.Instance.InitShareInfo(PlatformType.SinaWeibo, () =>
            //        {
            //            ShareImage(achievementShareDetailBgInSettlement, PlatformType.SinaWeibo, false);
            //        });
            //    }
            //    else
            //    {
            //        ShareImage(achievementShareDetailBgInSettlement, PlatformType.SinaWeibo, false);
            //    }
            //    break;

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
        settlementAccuracy.text = string.Format(settlementAccuracy.text, curSaveFileInstance.accuracy);
        allInstanceList = curSaveFileInstance.qInstancList;
        onlyWrongList = allInstanceList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
    }

    private bool FindWrong(QuentionInstance questionInstance)
    {
        int count = questionInstance.instance.Count;
        return questionInstance.instance[count - 1] != questionInstance.instance[count - 2];
    }

    private void RefreshSettlementGrid()
    {
        onlyWrongImage.SetActive(onlyWrong);
        ArrayList dataList;
        if (onlyWrong)
        {
            dataList = new ArrayList(onlyWrongList);
        }
        else
        {
            dataList = new ArrayList(allInstanceList);
        }
        settlementGrid.InitList(dataList, "QuestionItem");
    }
    private void InitAchievement()
    {
        if (string.IsNullOrEmpty(curSaveFileInstance.achievementName))
        {
            //curAchievementBtn.SetActive(false);
            return;
        }
        else
        {
            //curAchievementBtn.SetActive(true);
        }
        AchievementInstance instance = GameManager.Instance.GetAchievement(curSaveFileInstance.achievementName);
        achievementDetailImageInSettlement.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
        achievementDetailMainTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
        achievementDetailSubTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
        achievementDetailFinishTimeInSettlement.text = GetFinishTime(instance.finishTime);
    }
    private string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }
    //private void ShareImage(GameObject target, PlatformType type, bool isSaveFile)
    //{
    //    if (isSaveFile)
    //    {
    //        saveFileShareTitleInSettlement.enabled = true;
    //        if (type == PlatformType.WeChat || type == PlatformType.WeChatMoments)
    //            saveFileShareTitleInSettlement.text = string.Format(saveFileShareTitleInSettlement.text, GameManager.Instance.UserNameOfWeChat);
    //        else
    //            saveFileShareTitleInSettlement.text = string.Format(saveFileShareTitleInSettlement.text, GameManager.Instance.UserNameOfSinaWeibo);
    //    }
    //    else
    //    {
    //        achievementShareTitleInSettlement.enabled = true;
    //        if (type == PlatformType.WeChat || type == PlatformType.WeChatMoments)
    //            achievementShareTitleInSettlement.text = string.Format(achievementShareTitleInSettlement.text, GameManager.Instance.UserNameOfWeChat);
    //        else
    //            achievementShareTitleInSettlement.text = string.Format(achievementShareTitleInSettlement.text, GameManager.Instance.UserNameOfSinaWeibo);
    //    }
    //    RectTransform shotTarget = target.transform as RectTransform;
    //    Rect shotRect = CommonTool.GetShotTargetRect(shotTarget);
    //    GameManager.Instance.ShareImage(shotRect, type);
    //}
    //private void InitSaveFileShareWin()
    //{
    //    //saveFileShareTitleInSettlement.enabled = false;
    //    //saveFileSharePatternInSettlement_Time.SetActive(curSaveFileInstance.cInstance.patternID == PatternID.Time);
    //    //saveFileSharePatternInSettlement_Number.SetActive(curSaveFileInstance.cInstance.patternID == PatternID.Number);
    //    //saveFileShareAmountInSettlement.text = string.Format(saveFileShareAmountInSettlement.text, curSaveFileInstance.qInstancList.Count);
    //    //saveFileShareTimeInSettlement.text = string.Format(saveFileShareTimeInSettlement.text, curSaveFileInstance.timeCost.ToString("f1"));
    //    //saveFileShareSymbolInSettlement.text = string.Format(saveFileShareSymbolInSettlement.text, GameManager.Instance.SymbolArray[(int)curSaveFileInstance.cInstance.symbolID]);
    //    //saveFileShareDigitInSettlement.text = string.Format(saveFileShareDigitInSettlement.text, (int)(curSaveFileInstance.cInstance.digitID + 2));
    //    //saveFileShareOperandInSettlement.text = string.Format(saveFileShareOperandInSettlement.text, (int)(curSaveFileInstance.cInstance.operandID + 2));
    //    //saveFileShareAccuracyInSettlement.text = string.Format(saveFileShareAccuracyInSettlement.text, curSaveFileInstance.accuracy);
    //    //string meanTime = (curSaveFileInstance.timeCost / curSaveFileInstance.qInstancList.Count).ToString("f2");
    //    //saveFileShareMeanTimeInSettlement.text = string.Format(saveFileShareMeanTimeInSettlement.text, meanTime);
    //    //saveFileShareAchievementInSettlement.SetActive(!string.IsNullOrEmpty(curSaveFileInstance.achievementName));
    //    //saveFileShareWithoutAchievementInSettlement.SetActive(string.IsNullOrEmpty(curSaveFileInstance.achievementName));
    //    //if (!string.IsNullOrEmpty(curSaveFileInstance.achievementName))
    //    //{
    //    //    AchievementInstance instance = GameManager.Instance.GetAchievement(curSaveFileInstance.achievementName);
    //    //    saveFileShareImageInSettlement.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
    //    //    saveFileShareMainTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
    //    //    saveFileShareSubTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
    //    //    saveFileShareTypeInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.classType);
    //    //    saveFileShareFinishTimeInSettlement.text = GetFinishTime(instance.finishTime);
    //    //    saveFileShareConditionInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.condition);
    //    //}
    //}
    //private void InitAchievementShareWin()
    //{
    //    //if (string.IsNullOrEmpty(curSaveFileInstance.achievementName)) return;
    //    //AchievementInstance content = GameManager.Instance.GetAchievement(curSaveFileInstance.achievementName);
    //    //achievementShareTitleInSettlement.enabled = false;
    //    //achievementShareImageInSettlement.sprite = GameManager.Instance.GetSprite(content.imageIndex);
    //    //achievementShareMainTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
    //    //achievementShareSubTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(content.subTitleIndex);
    //    //achievementShareTypeInSettlement.text = GameManager.Instance.GetMutiLanguage(content.classType);
    //    //achievementShareFinishTimeInSettlement.text = GetFinishTime(content.finishTime);
    //    //achievementShareConditionInSettlement.text = GameManager.Instance.GetMutiLanguage(content.condition);
    //    //achievementSharePatternInSettlement_Time.SetActive(content.cInstance.patternID == PatternID.Time);
    //    //achievementSharePatternInSettlement_Number.SetActive(content.cInstance.patternID == PatternID.Number);
    //    //achievementShareAmountInSettlement.text = string.Format(achievementShareAmountInSettlement.text, curSaveFileInstance.qInstancList.Count);
    //    //achievementShareTimeInSettlement.text = string.Format(achievementShareTimeInSettlement.text, curSaveFileInstance.timeCost.ToString("f1"));
    //    //achievementShareSymbolInSettlement.text = string.Format(achievementShareSymbolInSettlement.text, GameManager.Instance.SymbolArray[(int)curSaveFileInstance.cInstance.symbolID]);
    //    //achievementShareDigitInSettlement.text = string.Format(achievementShareDigitInSettlement.text, (int)(curSaveFileInstance.cInstance.digitID + 2));
    //    //achievementShareOperandInSettlement.text = string.Format(achievementShareOperandInSettlement.text, (int)(curSaveFileInstance.cInstance.operandID + 2));
    //    //achievementShareAccuracyInSettlement.text = string.Format(achievementShareAccuracyInSettlement.text, curSaveFileInstance.accuracy);
    //    //string meanTime = (curSaveFileInstance.timeCost / curSaveFileInstance.qInstancList.Count).ToString("f2");
    //    //achievementShareMeanTimeInSettlement.text = string.Format(achievementShareMeanTimeInSettlement.text, meanTime);
    //}
}
