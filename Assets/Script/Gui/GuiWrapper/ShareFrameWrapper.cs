using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;
using cn.sharesdk.unity3d;


public class ShareFrameWrapper : GuiFrameWrapper
{
    private const float endPos = -200f;
    private const float duration = 0.5f;

    private ShareInstance content;
    private RectTransform sharePage;
    private Text sharelTitle;

    private GameObject saveFileSharePage;
    private GameObject saveFileSharePattern_Time;
    private GameObject saveFileSharePattern_Number;
    private Text saveFileShareAmount;
    private Text saveFileShareTime;
    private Text saveFileShareSymbol;
    private Text saveFileShareDigit;
    private Text saveFileShareOperand;
    private Text saveFileShareAccuracy;
    private Text saveFileShareMeanTime;
    private RectTransform shareBtnsBg;

    private GameObject achievementDetailPattern_Time;
    private GameObject achievementDetailPattern_Number;
    private GameObject achievementDetailPage;
    private GameObject achievementDetailBtn;
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

    private GameObject bluetoothSharePage;
    private GameObject bluetoothSharePattern_Number;
    private Text bluetoothShareAmount;
    private Text bluetoothShareTime;
    private Text bluetoothShareSymbol;
    private Text bluetoothShareDigit;
    private Text bluetoothShareOperand;
    private Text bluetoothShareOwnScore;
    private Text bluetoothShareOtherScore;

    void Start () 
	{
        id = GuiFrameID.ShareFrame;
        Init();
        content = GameManager.Instance.CurShareInstance;
        InitShareFrame();
        shareBtnsBg.DOMoveY(endPos, duration, true).From();
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        sharePage                       = gameObjectDict["SharePage"].GetComponent<RectTransform>();
        sharelTitle                     = gameObjectDict["SharelTitle"].GetComponent<Text>();
        saveFileSharePage               = gameObjectDict["SaveFileSharePage"];
        saveFileSharePattern_Time       = gameObjectDict["SaveFileSharePattern_Time"];
        saveFileSharePattern_Number     = gameObjectDict["SaveFileSharePattern_Number"];
        shareBtnsBg                     = gameObjectDict["ShareBtnsBg"].GetComponent<RectTransform>();
        saveFileShareAmount             = gameObjectDict["SaveFileShareAmount"].GetComponent<Text>();
        saveFileShareTime               = gameObjectDict["SaveFileShareTime"].GetComponent<Text>();
        saveFileShareSymbol             = gameObjectDict["SaveFileShareSymbol"].GetComponent<Text>();
        saveFileShareDigit              = gameObjectDict["SaveFileShareDigit"].GetComponent<Text>();
        saveFileShareOperand            = gameObjectDict["SaveFileShareOperand"].GetComponent<Text>();
        saveFileShareAccuracy           = gameObjectDict["SaveFileShareAccuracy"].GetComponent<Text>();
        saveFileShareMeanTime           = gameObjectDict["SaveFileShareMeanTime"].GetComponent<Text>();

        achievementDetailPattern_Time   = gameObjectDict["AchievementDetailPattern_Time"];
        achievementDetailPattern_Number = gameObjectDict["AchievementDetailPattern_Number"];
        achievementDetailPage           = gameObjectDict["AchievementDetailPage"];
        achievementDetailBtn            = gameObjectDict["AchievementDetailBtn"];
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

        bluetoothSharePage              = gameObjectDict["BluetoothSharePage"];
        bluetoothSharePattern_Number    = gameObjectDict["BluetoothSharePattern_Number"];
        bluetoothShareAmount            = gameObjectDict["BluetoothShareAmount"].GetComponent<Text>();
        bluetoothShareTime              = gameObjectDict["BluetoothShareTime"].GetComponent<Text>();
        bluetoothShareSymbol            = gameObjectDict["BluetoothShareSymbol"].GetComponent<Text>();
        bluetoothShareDigit             = gameObjectDict["BluetoothShareDigit"].GetComponent<Text>();
        bluetoothShareOperand           = gameObjectDict["BluetoothShareOperand"].GetComponent<Text>();
        bluetoothShareOwnScore          = gameObjectDict["BluetoothShareOwnScore"].GetComponent<Text>();
        bluetoothShareOtherScore        = gameObjectDict["BluetoothShareOtherScore"].GetComponent<Text>();

    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);

        switch (btn.name)
        {
            case "WeChatBtn":
                ShareImage(PlatformType.WeChat);
                break;
            case "WeChatMomentsBtn":
                ShareImage(PlatformType.WeChatMoments);
                break;
            case "SinaWeiboBtn":
                ShareImage(PlatformType.SinaWeibo);
                break;
            case "ShareFrameBg":
                shareBtnsBg.DOMoveY(endPos, duration, true);
                CommonTool.GuiScale(sharePage.gameObject, canvasGroup, false, () => GameManager.Instance.SwitchWrapper(GuiFrameID.None));
                break;
            case "AchievementDetailBtn":
                AchievementInstance instance = GameManager.Instance.CurAchievementInstance;
                GameManager.Instance.CurSaveFileInstance = GameManager.Instance.ReadRecord(instance.finishTime);
                GameManager.Instance.SwitchWrapper(GuiFrameID.SaveFileFrame, true);
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
                bluetoothSharePage.SetActive(false);
                InitAchievement();
                CommonTool.GuiScale(achievementDetailPage, canvasGroup, true);
                break;
            case ShareID.SaveFile:
                saveFileSharePage.SetActive(true);
                achievementDetailPage.SetActive(false);
                bluetoothSharePage.SetActive(false);
                InitSaveFile();
                CommonTool.GuiScale(saveFileSharePage, canvasGroup, true);
                break;
            case ShareID.Bluetooth:
                saveFileSharePage.SetActive(false);
                achievementDetailPage.SetActive(false);
                bluetoothSharePage.SetActive(true);
                InitBluetooth();
                CommonTool.GuiScale(bluetoothSharePage, canvasGroup, true);
                break;
        }        
    }

    private void InitSaveFile()
    {
        SaveFileInstance instance = GameManager.Instance.CurSaveFileInstance;
        if (instance == null) return;
        sharelTitle.text = GameManager.Instance.GetMutiLanguage("Text_20037");
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
        if (instance == null) return;
        achievementDetailBtn.SetActive(GameManager.Instance.LastGUI != GuiFrameID.SaveFileFrame
                                    && GameManager.Instance.LastGUI != GuiFrameID.SettlementFrame);
        sharelTitle.text = GameManager.Instance.GetMutiLanguage("Text_20051");
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

    private void InitBluetooth()
    {
        SaveFileInstance instance = GameManager.Instance.CurSaveFileInstance;
        if (instance == null) return;
        sharelTitle.gameObject.SetActive(true);
        sharelTitle.text = GameManager.Instance.GetMutiLanguage("Text_80015");
        sharelTitle.text = string.Format(sharelTitle.text, GameManager.Instance.UserName, instance.opponentName);
        bluetoothShareAmount.text = string.Format(bluetoothShareAmount.text, instance.qInstancList.Count);
        bluetoothShareTime.text = string.Format(bluetoothShareTime.text, instance.timeCost.ToString("f1"));
        bluetoothShareSymbol.text = string.Format(bluetoothShareSymbol.text, GameManager.Instance.SymbolArray[(int)instance.cInstance.symbolID]);
        bluetoothShareDigit.text = string.Format(bluetoothShareDigit.text, (int)(instance.cInstance.digitID + 2));
        bluetoothShareOperand.text = string.Format(bluetoothShareOperand.text, (int)(instance.cInstance.operandID + 2));
        bluetoothShareOwnScore.text = instance.ownScore.ToString();
        bluetoothShareOtherScore.text = instance.ownScore.ToString();
    }

    private void ShareImage(PlatformType type)
    {
        if (string.IsNullOrEmpty(GameManager.Instance.UserName))
        {
            GameManager.Instance.SwitchWrapper(GuiFrameID.NameBoardFrame, true);
        }
        else
        {
            if (!sharelTitle.gameObject.activeSelf)
            {
                sharelTitle.gameObject.SetActive(true);
                sharelTitle.text = string.Format(sharelTitle.text, GameManager.Instance.UserName);
            }
            Rect shotRect = CommonTool.GetShotTargetRect(sharePage);
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
