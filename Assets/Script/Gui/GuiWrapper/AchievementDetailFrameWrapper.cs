using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class AchievementDetailFrameWrapper : GuiFrameWrapper
{
    private AchievementInstance content;
    private GameObject achievementDetailTitle;
    private GameObject achievementDetailPattern_Time;
    private GameObject achievementDetailPattern_Number;
    private GameObject achievementDetailShareBtn;
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
        id = GuiFrameID.AchievementDetailFrame;
        Init();
        content = GameManager.Instance.CurAchievementInstance;
        InitAchievement();
        CommonTool.GuiScale(achievementDetailPage, canvasGroup, true);
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementDetailTitle                  = gameObjectDict["AchievementDetailTitle"];
        achievementDetailPattern_Time           = gameObjectDict["AchievementDetailPattern_Time"];
        achievementDetailPattern_Number         = gameObjectDict["AchievementDetailPattern_Number"];
        achievementDetailShareBtn               = gameObjectDict["AchievementDetailShareBtn"];
        achievementDetailPage                   = gameObjectDict["AchievementDetailPage"];
        achievementDetailImage                  = gameObjectDict["AchievementDetailImage"].GetComponent<Image>();
        achievementDetailMainTitle              = gameObjectDict["AchievementDetailMainTitle"].GetComponent<Text>();
        achievementDetailSubTitle               = gameObjectDict["AchievementDetailSubTitle"].GetComponent<Text>();
        achievementDetailFinishTime             = gameObjectDict["AchievementDetailFinishTime"].GetComponent<Text>();
        achievementDetailTime                   = gameObjectDict["AchievementDetailTime"].GetComponent<Text>();
        achievementDetailAmount                 = gameObjectDict["AchievementDetailAmount"].GetComponent<Text>();
        achievementDetailSymbol                 = gameObjectDict["AchievementDetailSymbol"].GetComponent<Text>();
        achievementDetailDigit                  = gameObjectDict["AchievementDetailDigit"].GetComponent<Text>();
        achievementDetailOperand                = gameObjectDict["AchievementDetailOperand"].GetComponent<Text>();
        achievementDetailCondition              = gameObjectDict["AchievementDetailCondition"].GetComponent<Text>();

    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetailBg":
                CommonTool.GuiScale(achievementDetailPage, canvasGroup, false, () => GameManager.Instance.SwitchWrapper(GuiFrameID.None));
                break;
            case "AchievementDetailShareBtn":
                if (string.IsNullOrEmpty(GameManager.Instance.UserName))
                    GameManager.Instance.SwitchWrapper(GuiFrameID.NameBoardFrame, true);
                else
                    GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }

    }

    private void InitAchievement()
    {

        achievementDetailTitle.SetActive(false);
        achievementDetailImage.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        achievementDetailMainTitle.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementDetailSubTitle.text = GameManager.Instance.GetMutiLanguage(content.subTitleIndex);
        achievementDetailFinishTime.text = GetFinishTime(content.finishTime);
        bool isTimePattern = content.cInstance.patternID == PatternID.Time;
        achievementDetailPattern_Time.SetActive(isTimePattern);
        achievementDetailPattern_Number.SetActive(!isTimePattern);
        achievementDetailTime.gameObject.SetActive(isTimePattern);
        achievementDetailAmount.gameObject.SetActive(!isTimePattern);
        if (isTimePattern)
        {
            int amount = GameManager.Instance.AmountArray_Time[(int)content.cInstance.amountID];
            achievementDetailTime.text = string.Format(achievementDetailTime.text, amount);
        }
        else
        {
            int amount = GameManager.Instance.AmountArray_Number[(int)content.cInstance.amountID];
            achievementDetailAmount.text = string.Format(achievementDetailAmount.text, amount);
        }
        string symbol = GameManager.Instance.SymbolArray[(int)content.cInstance.symbolID];
        achievementDetailSymbol.text = string.Format(achievementDetailSymbol.text, symbol);
        achievementDetailDigit.text = string.Format(achievementDetailDigit.text, (int)(content.cInstance.digitID + 2));
        achievementDetailOperand.text = string.Format(achievementDetailOperand.text, (int)(content.cInstance.operandID + 2));
        achievementDetailCondition.text = string.Format(achievementDetailCondition.text, content.accuracy, content.meanTime);
        achievementDetailShareBtn.SetActive(true);
    }

    private string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }


}
