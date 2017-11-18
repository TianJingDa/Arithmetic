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
    private GameObject curAchievementBtn;
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

    void Start () 
	{
        id = GuiFrameID.SettlementFrame;
        Init();
        InitSettlement();
        ShowAchievement();
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        settlementGrid                              = GameObjectDict["SettlementGrid"].GetComponent<InfiniteList>();
        settlementTime                              = GameObjectDict["SettlementTime"].GetComponent<Text>();
        settlementAmount                            = GameObjectDict["SettlementAmount"].GetComponent<Text>();
        settlementAccuracy                          = GameObjectDict["SettlementAccuracy"].GetComponent<Text>();
        achievementDetailMainTitleInSettlement      = GameObjectDict["AchievementDetailMainTitleInSettlement"].GetComponent<Text>();
        achievementDetailSubTitleInSettlement       = GameObjectDict["AchievementDetailSubTitleInSettlement"].GetComponent<Text>();
        achievementDetailFinishTimeInSettlement     = GameObjectDict["AchievementDetailFinishTimeInSettlement"].GetComponent<Text>();
        achievementDetailBgInSettlement             = GameObjectDict["AchievementDetailBgInSettlement"];
        achievementDetailImageInSettlement          = GameObjectDict["AchievementDetailImageInSettlement"].GetComponent<Image>();
        curAchievementBtn                           = GameObjectDict["CurAchievementBtn"];
        onlyWrongImage                              = GameObjectDict["OnlyWrongImage"];
        saveFileShareWinInSettlement                = GameObjectDict["SaveFileShareWinInSettlement"];
        achievementShareWinInSettlement             = GameObjectDict["AchievementShareWinInSettlement"];
        saveFileSharePageInSettlement               = GameObjectDict["SaveFileSharePageInSettlement"];
        achievementShareDetailBgInSettlement        = GameObjectDict["AchievementShareDetailBgInSettlement"];

    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetailBgInSettlement"://容易误点
                achievementDetailBgInSettlement.SetActive(false);
                break;
            case "AchievementDetailShareBtnInSettlement":
                OnShareAchievementBtn();
                break;
            case "AchievementShareDetailBgInSettlement":
                achievementShareWinInSettlement.SetActive(false);
                break;
            case "CurAchievementBtn":
                ShowAchievement();
                break;
            case "OnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "SaveFileSharePageInSettlement":
                saveFileShareWinInSettlement.SetActive(false);
                break;
            case "ShareBtnInSettlement":
                OnShareSaveFileBtn();
                break;
            case "Settlement2CategoryFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.CategoryFrame);
                break;
            case "Settlement2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.StartFrame);
                break;
            case "WeChatBtnOfSaveFileInSettlement":
                if (false)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
                    {
                        ShareImage(saveFileSharePageInSettlement, PlatformType.WeChat);
                    });
                }
                else
                {
                    ShareImage(saveFileSharePageInSettlement, PlatformType.WeChat);
                }
                break;
            case "WeChatMomentsBtnOfSaveFileInSettlement":
                if (false)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
                    {
                        ShareImage(saveFileSharePageInSettlement, PlatformType.WeChatMoments);
                    });
                }
                else
                {
                    ShareImage(saveFileSharePageInSettlement, PlatformType.WeChatMoments);
                }
                break;
            case "SinaWeiboBtnOfSaveFileInSettlement":
                if (GameManager.Instance.UserNameOfSinaWeibo == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.SinaWeibo, () =>
                    {
                        ShareImage(saveFileSharePageInSettlement, PlatformType.SinaWeibo);
                    });
                }
                else
                {
                    ShareImage(saveFileSharePageInSettlement, PlatformType.SinaWeibo);
                }
                break;
            case "WeChatBtnOfAchievementInSettlement":
                if (GameManager.Instance.UserNameOfWeChat == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
                    {
                        ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChat);
                    });
                }
                else
                {
                    ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChat);
                }
                break;
            case "WeChatMomentsBtnOfAchievementInSettlement":
                if (GameManager.Instance.UserNameOfWeChat == null)
                {
                    GameManager.Instance.InitShareInfo(PlatformType.WeChat, () =>
                    {
                        ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChatMoments);
                    });
                }
                else
                {
                    ShareImage(achievementShareDetailBgInSettlement, PlatformType.WeChatMoments);
                }
                break;
            case "SinaWeiboBtnOfAchievementInSettlement":
                if (GameManager.Instance.UserNameOfSinaWeibo == null)
                {
                    GameManager.Instance.InitShareInfo(cn.sharesdk.unity3d.PlatformType.SinaWeibo, () =>
                    {
                        ShareImage(achievementShareDetailBgInSettlement, PlatformType.SinaWeibo);
                    });
                }
                else
                {
                    ShareImage(achievementShareDetailBgInSettlement, PlatformType.SinaWeibo);
                }
                break;

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
    private void ShowAchievement()
    {
        if (string.IsNullOrEmpty(curSaveFileInstance.achievementName))
        {
            curAchievementBtn.SetActive(false);
            return;
        }
        else
        {
            curAchievementBtn.SetActive(true);
        }
        achievementDetailBgInSettlement.SetActive(true);
        AchievementInstance instance = GameManager.Instance.GetAchievement(curSaveFileInstance.achievementName);
        achievementDetailImageInSettlement.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
        achievementDetailMainTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
        achievementDetailSubTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
        achievementDetailFinishTimeInSettlement.text = GetFinishTime(instance.fileName);
    }
    private string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }
    private void ShareImage(GameObject target, PlatformType type )
    {
        //1、找到名称Text
        //2、用userInfo赋值
        RectTransform shotTarget = target.transform as RectTransform;
        Rect shotRect = CommonTool.GetShotTargetRect(shotTarget);
        GameManager.Instance.ShareImage(shotRect, type);
    }
    private void OnShareAchievementBtn()
    {
        achievementShareWinInSettlement.SetActive(true);
        //初始化分享界面
    }
    private void OnShareSaveFileBtn()
    {
        saveFileShareWinInSettlement.SetActive(true);
        //初始化分享界面
    }
}
