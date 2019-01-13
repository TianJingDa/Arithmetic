using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;
using cn.sharesdk.unity3d;


public class ShareFrameWrapper : GuiFrameWrapper
{
    private ShareInstance content;

    private GameObject saveFileSharePage;
    private GameObject saveFileSharePattern_Time;
    private GameObject saveFileSharePattern_Number;
    private Text saveFileShareTitle;
    private Text saveFileShareAmount;
    private Text saveFileShareTime;
    private Text saveFileShareSymbol;
    private Text saveFileShareDigit;
    private Text saveFileShareOperand;
    private Text saveFileShareAccuracy;
    private Text saveFileShareMeanTime;
    private RectTransform shareBtnsBg;

    private GameObject achievementDetailTitle;
    private GameObject achievementDetailPattern_Time;
    private GameObject achievementDetailPattern_Number;
    private GameObject achievementDetailPage;
    private Image achievementDetailImage;
    private Text achievementDetailMainTitle;
    private Text achievementDetailSubTitle;
    private Text achievementDetailFinishTime;
    private Text achievementDetailTime;
    private Text achievementDetailAmount;
    private Text achievementDetailSymbol;
    private Text achievementDetailDigit;
    private Text achievementDetailOperand;
    private Text achievementDetailCondition;


    void Start () 
	{
        id = GuiFrameID.ShareFrame;
        Init();
        content = GameManager.Instance.CurShareInstance;
        InitShareFrame();
        shareBtnsBg.DOMoveY(shareBtnsBg.rect.y, 0.3f, true).From();
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        saveFileSharePage               = gameObjectDict["SaveFileSharePage"];
        saveFileSharePattern_Time       = gameObjectDict["SaveFileSharePattern_Time"];
        saveFileSharePattern_Number     = gameObjectDict["SaveFileSharePattern_Number"];
        shareBtnsBg                     = gameObjectDict["ShareBtnsBg"].GetComponent<RectTransform>();
        saveFileShareTitle              = gameObjectDict["SaveFileShareTitle"].GetComponent<Text>();
        saveFileShareAmount             = gameObjectDict["SaveFileShareAmount"].GetComponent<Text>();
        saveFileShareTime               = gameObjectDict["SaveFileShareTime"].GetComponent<Text>();
        saveFileShareSymbol             = gameObjectDict["SaveFileShareSymbol"].GetComponent<Text>();
        saveFileShareDigit              = gameObjectDict["SaveFileShareDigit"].GetComponent<Text>();
        saveFileShareOperand            = gameObjectDict["SaveFileShareOperand"].GetComponent<Text>();
        saveFileShareAccuracy           = gameObjectDict["SaveFileShareAccuracy"].GetComponent<Text>();
        saveFileShareMeanTime           = gameObjectDict["SaveFileShareMeanTime"].GetComponent<Text>();

        achievementDetailTitle          = gameObjectDict["AchievementDetailTitle"];
        achievementDetailPattern_Time   = gameObjectDict["AchievementDetailPattern_Time"];
        achievementDetailPattern_Number = gameObjectDict["AchievementDetailPattern_Number"];
        achievementDetailPage           = gameObjectDict["AchievementDetailPage"];
        achievementDetailImage          = gameObjectDict["AchievementDetailImage"].GetComponent<Image>();
        achievementDetailMainTitle      = gameObjectDict["AchievementDetailMainTitle"].GetComponent<Text>();
        achievementDetailSubTitle       = gameObjectDict["AchievementDetailSubTitle"].GetComponent<Text>();
        achievementDetailFinishTime     = gameObjectDict["AchievementDetailFinishTime"].GetComponent<Text>();
        achievementDetailTime           = gameObjectDict["AchievementDetailTime"].GetComponent<Text>();
        achievementDetailAmount         = gameObjectDict["AchievementDetailAmount"].GetComponent<Text>();
        achievementDetailSymbol         = gameObjectDict["AchievementDetailSymbol"].GetComponent<Text>();
        achievementDetailDigit          = gameObjectDict["AchievementDetailDigit"].GetComponent<Text>();
        achievementDetailOperand        = gameObjectDict["AchievementDetailOperand"].GetComponent<Text>();
        achievementDetailCondition      = gameObjectDict["AchievementDetailCondition"].GetComponent<Text>();


    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);

        switch (btn.name)
        {
            case "WeChatBtn":
                if (content.id == ShareID.Achievement)
                    ShareImage(achievementDetailPage, PlatformType.WeChat);
                else
                    ShareImage(saveFileSharePage, PlatformType.WeChat);
                break;
            case "WeChatMomentsBtn":
                if (content.id == ShareID.Achievement)
                    ShareImage(achievementDetailPage, PlatformType.WeChatMoments);
                else
                    ShareImage(saveFileSharePage, PlatformType.WeChatMoments);
                break;
            case "SinaWeiboBtn":
                if (content.id == ShareID.Achievement)
                    ShareImage(achievementDetailPage, PlatformType.SinaWeibo);
                else
                    ShareImage(saveFileSharePage, PlatformType.SinaWeibo);
                break;
            case "ShareFrameBg":
                CommonTool.GuiScale(achievementDetailPage, canvasGroup, false, () => GameManager.Instance.SwitchWrapper(GuiFrameID.None));
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private void InitShareFrame()
    {
        switch (content.id)
        {
            case ShareID.Achievement:
                saveFileSharePage.SetActive(false);
                achievementDetailPage.SetActive(true);
                InitAchievement();
                CommonTool.GuiScale(achievementDetailPage, canvasGroup, true);
                break;
            case ShareID.SaveFile:
                saveFileSharePage.SetActive(true);
                achievementDetailPage.SetActive(false);
                InitSaveFile();
                CommonTool.GuiScale(saveFileSharePage, canvasGroup, true);
                break;
            case ShareID.Bluetooth:
                break;
        }        
    }

    private void InitSaveFile()
    {
        SaveFileInstance instance = GameManager.Instance.CurSaveFileInstance;
        saveFileShareTitle.text = string.Format(saveFileShareTitle.text, GameManager.Instance.UserName);
        saveFileSharePattern_Time.SetActive(instance.cInstance.patternID == PatternID.Time);
        saveFileSharePattern_Number.SetActive(instance.cInstance.patternID == PatternID.Number);
        saveFileShareAmount.text = string.Format(saveFileShareAmount.text, instance.qInstancList.Count);
        saveFileShareTime.text = string.Format(saveFileShareTime.text, instance.timeCost.ToString("f1"));
        saveFileShareSymbol.text = string.Format(saveFileShareSymbol.text, GameManager.Instance.SymbolArray[(int)instance.cInstance.symbolID]);
        saveFileShareDigit.text = string.Format(saveFileShareDigit.text, (int)(instance.cInstance.digitID + 2));
        saveFileShareOperand.text = string.Format(saveFileShareOperand.text, (int)(instance.cInstance.operandID + 2));
        saveFileShareAccuracy.text = string.Format(saveFileShareAccuracy.text, instance.accuracy);
        string meanTime = (instance.timeCost / instance.qInstancList.Count).ToString("f1");
        saveFileShareMeanTime.text = string.Format(saveFileShareMeanTime.text, meanTime);
    }

    private void InitAchievement()
    {
        AchievementInstance instance = GameManager.Instance.CurAchievementInstance;
        achievementDetailTitle.SetActive(false);
        achievementDetailImage.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
        achievementDetailMainTitle.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
        achievementDetailSubTitle.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
        achievementDetailFinishTime.text = GetFinishTime(instance.finishTime);
        bool isTimePattern = instance.cInstance.patternID == PatternID.Time;
        achievementDetailPattern_Time.SetActive(isTimePattern);
        achievementDetailPattern_Number.SetActive(!isTimePattern);
        achievementDetailTime.gameObject.SetActive(isTimePattern);
        achievementDetailAmount.gameObject.SetActive(!isTimePattern);
        if (isTimePattern)
        {
            int amount = GameManager.Instance.AmountArray_Time[(int)instance.cInstance.amountID];
            achievementDetailTime.text = string.Format(achievementDetailTime.text, amount);
        }
        else
        {
            int amount = GameManager.Instance.AmountArray_Number[(int)instance.cInstance.amountID];
            achievementDetailAmount.text = string.Format(achievementDetailAmount.text, amount);
        }
        string symbol = GameManager.Instance.SymbolArray[(int)instance.cInstance.symbolID];
        achievementDetailSymbol.text = string.Format(achievementDetailSymbol.text, symbol);
        achievementDetailDigit.text = string.Format(achievementDetailDigit.text, (int)(instance.cInstance.digitID + 2));
        achievementDetailOperand.text = string.Format(achievementDetailOperand.text, (int)(instance.cInstance.operandID + 2));
        achievementDetailCondition.text = string.Format(achievementDetailCondition.text, instance.accuracy, instance.meanTime);
    }

    private string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }

    private void ShareImage(GameObject target, PlatformType type)
    {
        if (string.IsNullOrEmpty(GameManager.Instance.UserName))
        {
            GameManager.Instance.SwitchWrapper(GuiFrameID.NameBoardFrame, true);
        }
        else
        {
            RectTransform shotTarget = target.transform as RectTransform;
            Rect shotRect = CommonTool.GetShotTargetRect(shotTarget);
            GameManager.Instance.ShareImage(shotRect, type);
        }
    }


}

public class ShareInstance
{
    public ShareID id;

    public ShareInstance() { }

    public ShareInstance(ShareID id)
    {
        this.id = id;
    }
}
