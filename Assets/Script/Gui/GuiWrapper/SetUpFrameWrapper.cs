using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cn.sharesdk.unity3d;
using DG.Tweening;

/// <summary>
/// 设置界面
/// </summary>
public class SetUpFrameWrapper : GuiFrameWrapper
{
    private int tempLanguageID;
    private int tempSkinID;
    private int tempLayoutID;
    private int tempHandednessID;
    private bool firstInLayout;//用于标识进入布局设置界面

    private List<int> resetTogglesIndexList;
    private List<Vector2> languageTogglesAnchoredPositonList;
    private List<Vector2> skinTogglesAnchoredPositonList;
    private List<System.Action> resetDelegateList;

    private GameObject strategyWin;
    private GameObject languageWin;
    private GameObject skinWin;
    private GameObject layoutWin;
    private GameObject horizontalLayoutTipBg;
    private GameObject resetWin;
    private GameObject feedbackWin;
    private GameObject aboutUsWin;
    private GameObject shareUsWin;
    private GameObject thankDevelopersWin;
    private GameObject thankDevelopersPage;
    private GameObject resetConfirmBg;
    private GameObject resetToggleGroup;
    private GameObject resetTipBg;
    private GameObject resetTipPageTitle_Text_Achievement;
    private GameObject resetTipPageTitle_Text_SaveFile;
    private GameObject layoutV_RImg;
    private GameObject layoutV_LImg;
    private GameObject layoutH_RImg;
    private GameObject layoutH_LImg;
    private Button languageApplyBtn;
    private Button skinApplyBtn;
    private Button layoutApplyBtn;
    private Button resetApplyBtn;
    private Toggle resetTempTgl;
    private ToggleGroup languageToggleGroup;
    private ToggleGroup skinToggleGroup;
    private Dropdown layoutDropdown;
    private Dropdown handednessDropdown;
    private Text editionImg_Text;

    void Start () 
	{
        id = GuiFrameID.SetUpFrame;
        Init();
#if UNITY_ANDROID
        editionImg_Text.text = GameManager.Instance.GetMutiLanguage("Text_40008");
#elif UNITY_IOS
        editionImg_Text.text = GameManager.Instance.GetMutiLanguage("Text_40050");
#endif
        languageTogglesAnchoredPositonList = InitToggleAnchoredPositon(languageToggleGroup);
        skinTogglesAnchoredPositonList = InitToggleAnchoredPositon(skinToggleGroup);

    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        skinWin                                 = gameObjectDict["SkinWin"];
        resetWin                                = gameObjectDict["ResetWin"];
        resetTipBg                              = gameObjectDict["ResetTipBg"];
        layoutWin                               = gameObjectDict["LayoutWin"];
        horizontalLayoutTipBg                   = gameObjectDict["HorizontalLayoutTipBg"];
        aboutUsWin                              = gameObjectDict["AboutUsWin"];
        shareUsWin                              = gameObjectDict["ShareUsWin"];
        feedbackWin                             = gameObjectDict["FeedbackWin"];
        languageWin                             = gameObjectDict["LanguageWin"];
        strategyWin                             = gameObjectDict["StrategyWin"];
        resetConfirmBg                          = gameObjectDict["ResetConfirmBg"];
        resetToggleGroup                        = gameObjectDict["ResetToggleGroup"];
        thankDevelopersWin                      = gameObjectDict["ThankDevelopersWin"];
        thankDevelopersPage                     = gameObjectDict["ThankDevelopersPage"];
        resetTipPageTitle_Text_Achievement      = gameObjectDict["ResetTipPageTitle_Text_Achievement"];
        resetTipPageTitle_Text_SaveFile         = gameObjectDict["ResetTipPageTitle_Text_SaveFile"];
        layoutV_RImg                            = gameObjectDict["LayoutV_RImg"];
        layoutV_LImg                            = gameObjectDict["LayoutV_LImg"];
        layoutH_RImg                            = gameObjectDict["LayoutH_RImg"];
        layoutH_LImg                            = gameObjectDict["LayoutH_LImg"];
        skinToggleGroup                         = gameObjectDict["SkinToggleGroup"].GetComponent<ToggleGroup>();
        languageToggleGroup                     = gameObjectDict["LanguageToggleGroup"].GetComponent<ToggleGroup>();
        skinApplyBtn                            = gameObjectDict["SkinApplyBtn"].GetComponent<Button>();
        resetApplyBtn                           = gameObjectDict["ResetApplyBtn"].GetComponent<Button>();
        layoutApplyBtn                          = gameObjectDict["LayoutApplyBtn"].GetComponent<Button>();
        languageApplyBtn                        = gameObjectDict["LanguageApplyBtn"].GetComponent<Button>();
        layoutDropdown                          = gameObjectDict["LayoutDropdown"].GetComponent<Dropdown>();
        handednessDropdown                      = gameObjectDict["HandednessDropdown"].GetComponent<Dropdown>();
        editionImg_Text                         = gameObjectDict["EditionImg_Text"].GetComponent<Text>();
    }



    /// <summary>
    /// 刷新toggles的排列位置
    /// </summary>
    /// <param name="toggleGroup"></param>
    /// <param name="togglesAnchoredPositon"></param>
    /// <param name="curID"></param>
    private void RefreshToggleGroup(ToggleGroup toggleGroup, List<Vector2> togglesAnchoredPositon, int curID)
    {
        if (!toggleGroup)
        {
            MyDebug.LogYellow("toggleGroup is null!");
            return;
        }
        if (togglesAnchoredPositon == null || togglesAnchoredPositon.Count == 0)
        {
            MyDebug.LogYellow("togglesAnchoredPositon NO data!");
            return;
        }

        toggleGroup.SetAllTogglesOff();
        List<Vector2> tempTogglesAnchoredPositon = new List<Vector2>(togglesAnchoredPositon);
        Vector2 curToggleAnchoredPositon = tempTogglesAnchoredPositon[0];
        tempTogglesAnchoredPositon.RemoveAt(0);
        tempTogglesAnchoredPositon.Insert(curID, curToggleAnchoredPositon);
        for(int i=0;i< toggleGroup.toggles.Count; i++)
        {
            RectTransform toggleRectTransform = toggleGroup.toggles[i].transform as RectTransform;
            toggleRectTransform.anchoredPosition = tempTogglesAnchoredPositon[i];
        }
        toggleGroup.toggles[curID].isOn = true;
    }
    /// <summary>
    /// 记录toggles初始的位置
    /// </summary>
    /// <param name="toggleGroup"></param>
    /// <returns></returns>
    private List<Vector2> InitToggleAnchoredPositon(ToggleGroup toggleGroup)
    {
        if (!toggleGroup)
        {
            MyDebug.LogYellow("toggleGroup is null");
            return null;
        }
        List<Vector2> toggleAnchoredPositon = new List<Vector2>();
        for(int i=0;i< toggleGroup.transform.childCount; i++)
        {
            RectTransform toggleRectTransform = toggleGroup.transform.GetChild(i) as RectTransform;
            toggleAnchoredPositon.Add(toggleRectTransform.anchoredPosition);
        }
        return toggleAnchoredPositon;
    }
    /// <summary>
    /// 刷新Dropdown的状态
    /// </summary>
    /// <param name="dpd"></param>
    /// <param name="index"></param>
    private void RefreshDropdown(Dropdown dpd, int index)
    {
        if (!dpd)
        {
            MyDebug.LogYellow("Dropdown is null!");
            return;
        }
        for(int i = 0; i < dpd.options.Count; i++)
        {
            dpd.options[i].text = GameManager.Instance.GetMutiLanguage(dpd.options[i].text);
        }
        dpd.value = index;
        dpd.RefreshShownValue();
    }
    /// <summary>
    /// 刷新重置界面
    /// </summary>
    private void RefreshResetWin()
    {
        for(int i = 0; i < resetToggleGroup.transform.childCount; i++)
        {
            resetToggleGroup.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
        }
        resetTogglesIndexList = new List<int>();
        if (resetDelegateList == null)
        {
            resetDelegateList = new List<System.Action>();
            //resetDelegateList.Add(ResetTotalTime);
            //resetDelegateList.Add(ResetTotalGame);
            resetDelegateList.Add(ResetAchievement);
            resetDelegateList.Add(ResetSaveFile);
            //resetDelegateList.Add(ResetSaveFileWithoutAchievement);
            resetDelegateList.Add(ResetSetUp);
        }
    }
    /// <summary>
    /// 重置游戏时间
    /// </summary>
    private void ResetTotalTime()
    {
        PlayerPrefs.DeleteKey("TotalTime");
    }
    /// <summary>
    /// 重置游戏次数
    /// </summary>
    private void ResetTotalGame()
    {
        PlayerPrefs.DeleteKey("TotalGame");
    }
    /// <summary>
    /// 重置游戏成就
    /// </summary>
    private void ResetAchievement()
    {
        GameManager.Instance.ResetAchievement();
    }
    /// <summary>
    /// 重置所有游戏存档
    /// </summary>
    private void ResetSaveFile()
    {
        GameManager.Instance.ResetSaveFile();
    }
    /// <summary>
    /// 重置所有不带成就的游戏存档
    /// </summary>
    //private void ResetSaveFileWithoutAchievement()
    //{
    //    GameManager.Instance.ResetSaveFileWithoutAchievement();
    //}
    /// <summary>
    /// 重置游戏设置
    /// </summary>
    private void ResetSetUp()
    {
        PlayerPrefs.DeleteKey("LanguageID");
        PlayerPrefs.DeleteKey("SkinID");
        PlayerPrefs.DeleteKey("LayoutID");
        PlayerPrefs.DeleteKey("HandednessID");
        RefreshGui();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AboutUsBtn":
                aboutUsWin.SetActive(true);
                CommonTool.GuiHorizontalMove(aboutUsWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "AboutUs2SetUpFrameBtn":
                CommonTool.GuiHorizontalMove(aboutUsWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "FeedbackBtn":
            case "FeedbackWin":
                feedbackWin.SetActive(!feedbackWin.activeSelf);
                break;
            case "LanguageBtn":
                tempLanguageID = -1;
                languageWin.SetActive(true);
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositonList, (int)GameManager.Instance.CurLanguageID);
                languageApplyBtn.interactable = false;
                CommonTool.GuiHorizontalMove(languageWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "Language2SetUpFrameBtn":
                CommonTool.GuiHorizontalMove(languageWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "LanguageApplyBtn":
                GameManager.Instance.CurLanguageID = (LanguageID)tempLanguageID;
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositonList, (int)GameManager.Instance.CurLanguageID);
                languageApplyBtn.interactable = false;
                RefreshGui();
                break;
            case "LayoutBtn":
                RectTransform layoutRect = layoutWin.GetComponent<RectTransform>();
                if (layoutRect) layoutRect.anchoredPosition = Vector2.zero;
                firstInLayout = true;
                tempLayoutID = (int)GameManager.Instance.CurLayoutID;
                tempHandednessID = (int)GameManager.Instance.CurHandednessID;
                layoutWin.SetActive(true);
                RefreshDropdown(layoutDropdown, tempLayoutID);
                RefreshDropdown(handednessDropdown, tempHandednessID);
                RefreshLayoutSketch();
                layoutApplyBtn.interactable = false;
                firstInLayout = false;
                CommonTool.GuiHorizontalMove(layoutWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "HorizontalLayoutTipConfirmBtn":
                horizontalLayoutTipBg.SetActive(false);
                break;
            case "Layout2SetUpFrameBtn":
                CommonTool.GuiHorizontalMove(layoutWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "LayoutApplyBtn":
                GameManager.Instance.CurLayoutID = (LayoutID)tempLayoutID;
                GameManager.Instance.CurHandednessID = (HandednessID)tempHandednessID;
                layoutApplyBtn.interactable = false;
                break;
            case "AboutUs2StartFrameBtn":
            case "Language2StartFrameBtn":
            case "Layout2StartFrameBtn":
            case "Reset2StartFrameBtn":
            case "SetUp2StartFrameBtn":
            case "Skin2StartFrameBtn":
            case "Strategy2StartFrameBtn":
                GameManager.Instance.SwitchWrapperWithMove(GuiFrameID.StartFrame, MoveID.RightOrUp, false);
                break;
            case "ShareUsBtn":
                shareUsWin.SetActive(true);
                ShowShareUsWin();
                break;
            case "ShareUsWin":
                shareUsWin.SetActive(false);
                break;
            case "ResetBtn":
                RectTransform resetRect = resetWin.GetComponent<RectTransform>();
                if (resetRect) resetRect.anchoredPosition = Vector2.zero;
                resetWin.SetActive(true);
                RefreshResetWin();
                CommonTool.GuiHorizontalMove(resetWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "Reset2SetUpFrameBtn":
                CommonTool.GuiHorizontalMove(resetWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "ResetApplyBtn":
                resetConfirmBg.SetActive(true);
                break;
            case "ResetCancelBtn":
                resetConfirmBg.SetActive(false);
                break;
            case "ResetConfirmBg":
                resetConfirmBg.SetActive(false);
                break;
            case "ResetConfirmBtn":
                resetConfirmBg.SetActive(false);
                for (int i = 0; i < resetTogglesIndexList.Count; i++)
                {
                    int index = resetTogglesIndexList[i];
                    resetDelegateList[index]();
                }
                RefreshResetWin();
                break;
            case "ResetTipCancelBtn":
                resetTipBg.SetActive(false);
                resetTempTgl.isOn = false;
                break;
            case "ResetTipConfirmBtn":
                resetTipBg.SetActive(false);
                resetApplyBtn.interactable = resetTogglesIndexList.Count != 0;
                break;
            case "SkinBtn":
                tempSkinID = -1;
                skinWin.SetActive(true);
                RefreshToggleGroup(skinToggleGroup, skinTogglesAnchoredPositonList, (int)GameManager.Instance.CurSkinID);
                skinApplyBtn.interactable = false;
                CommonTool.GuiHorizontalMove(skinWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "Skin2SetUpFrameBtn":
                CommonTool.GuiHorizontalMove(skinWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "SkinApplyBtn":
                GameManager.Instance.CurSkinID = (SkinID)tempSkinID;
                RefreshToggleGroup(skinToggleGroup, skinTogglesAnchoredPositonList, (int)GameManager.Instance.CurSkinID);
                skinApplyBtn.interactable = false;
                RefreshGui();
                break;
            case "StrategyBtn":
                strategyWin.SetActive(true);
                CommonTool.GuiHorizontalMove(strategyWin, Screen.width, MoveID.RightOrUp, canvasGroup, true);
                break;
            case "Strategy2SetUpFrameBtn":
                CommonTool.GuiHorizontalMove(strategyWin, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                break;
            case "ThankDevelopersBtn":
                thankDevelopersWin.SetActive(true);
                thankDevelopersPage.SetActive(true);
                CommonTool.GuiScale(thankDevelopersPage, canvasGroup, true);
                break;
            case "ThankDevelopersWin":
                CommonTool.GuiScale(thankDevelopersPage, canvasGroup, false, () => thankDevelopersWin.SetActive(false));
                break;
            case "WeChatBtnInSetUp":
                GameManager.Instance.ShareUrl(PlatformType.WeChat);
                break;
            case "WeChatMomentsInSetUp":
                GameManager.Instance.ShareUrl(PlatformType.WeChatMoments);
                break;
            case "SinaWeiboBtnInSetUp":
                GameManager.Instance.ShareUrl(PlatformType.SinaWeibo);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    protected override void OnToggleClick(Toggle tgl)
    {
        base.OnToggleClick(tgl);
        if (tgl.name.Contains("LanguageToggle"))
        {
            OnToggleClick(languageApplyBtn, ref tempLanguageID, tgl);
        }
        else if (tgl.name.Contains("SkinToggle"))
        {
            OnToggleClick(skinApplyBtn, ref tempSkinID, tgl);
        }
        else if (tgl.name.Contains("ResetToggle"))
        {
            if (tgl.isOn)
            {
                resetTogglesIndexList.Add(tgl.index);
                //if (tgl.index == 2 || tgl.index == 3)
                //{
                //    resetTempTgl = tgl;
                //    ShowTip();
                //    return;
                //}
            }
            else
            {
                resetTogglesIndexList.Remove(tgl.index);
            }
            resetApplyBtn.interactable = resetTogglesIndexList.Count != 0;
        }
    }
    private void OnToggleClick(Button confirmBtn, ref int tempID, Toggle tgl)
    {
        confirmBtn.interactable = true;

        if (tgl.isOn)
        {
            tempID = tgl.index;
        }
    }
    private void ShowShareUsWin()
    {
        Sequence shareUsSequence = DOTween.Sequence();
        shareUsSequence.Append(shareUsWin.transform.DOLocalMoveX(Screen.width, 0.2f, true).From().SetEase(Ease.OutQuint));
        RectTransform shareUsPage = CommonTool.GetComponentByName<RectTransform>(shareUsWin, "ShareUsPage");
        shareUsSequence.Append(shareUsPage.DOMoveY(shareUsPage.rect.y, 0.2f, true).From());
        shareUsSequence.OnStart(() => canvasGroup.blocksRaycasts = false).
                        OnComplete(() => canvasGroup.blocksRaycasts = true);

    }

    private void ShowTip()
    {
        resetTipBg.SetActive(true);
        resetTipPageTitle_Text_Achievement.SetActive(resetTempTgl.index == 2);
        resetTipPageTitle_Text_SaveFile.SetActive(resetTempTgl.index == 3);
    }
    protected override void OnDropdownClick(Dropdown dpd)
    {
        base.OnDropdownClick(dpd);
        switch (dpd.name)
        {
            case "LayoutDropdown":
                tempLayoutID = dpd.value;
                horizontalLayoutTipBg.SetActive((LayoutID)tempLayoutID == LayoutID.Horizontal && !firstInLayout);
                break;
            case "HandednessDropdown":
                tempHandednessID = dpd.value;
                break;
            default:
                MyDebug.LogYellow("Can not find Dropdown:" + dpd.name);
                break;
        }
        RefreshLayoutSketch();
        layoutApplyBtn.interactable = true;
    }

    private void RefreshLayoutSketch()
    {
        layoutV_RImg.SetActive(tempLayoutID == (int)LayoutID.Vertical && tempHandednessID == (int)HandednessID.Right);
        layoutV_LImg.SetActive(tempLayoutID == (int)LayoutID.Vertical && tempHandednessID == (int)HandednessID.Left);
        layoutH_RImg.SetActive(tempLayoutID == (int)LayoutID.Horizontal && tempHandednessID == (int)HandednessID.Right);
        layoutH_LImg.SetActive(tempLayoutID == (int)LayoutID.Horizontal && tempHandednessID == (int)HandednessID.Left);

    }
    //public override void OnToggleClick(bool check)
    //{
    //    if (check)
    //    {
    //        tempLanguageID = languageToggleGroup.ActiveToggles().FirstOrDefault().index;
    //        Debug.Log("tempLanguageID:" + tempLanguageID);
    //    }
    //}


}
