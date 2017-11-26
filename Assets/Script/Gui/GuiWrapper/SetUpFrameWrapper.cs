using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cn.sharesdk.unity3d;
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
    private GameObject resetConfirmBg;
    private GameObject resetToggleGroup;
    private GameObject resetTipBg;
    private GameObject resetTipPageTitle_Text_Achievement;
    private GameObject resetTipPageTitle_Text_SaveFile;
    private Button languageApplyBtn;
    private Button skinApplyBtn;
    private Button layoutApplyBtn;
    private Button resetApplyBtn;
    private Toggle resetTempTgl;
    private ToggleGroup languageToggleGroup;
    private ToggleGroup skinToggleGroup;
    private Dropdown layoutDropdown;
    private Dropdown handednessDropdown;

    void Start () 
	{
        id = GuiFrameID.SetUpFrame;
        Init();

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
        resetTipPageTitle_Text_Achievement      = gameObjectDict["ResetTipPageTitle_Text_Achievement"];
        resetTipPageTitle_Text_SaveFile         = gameObjectDict["ResetTipPageTitle_Text_SaveFile"];
        skinToggleGroup                         = gameObjectDict["SkinToggleGroup"].GetComponent<ToggleGroup>();
        languageToggleGroup                     = gameObjectDict["LanguageToggleGroup"].GetComponent<ToggleGroup>();
        skinApplyBtn                            = gameObjectDict["SkinApplyBtn"].GetComponent<Button>();
        resetApplyBtn                           = gameObjectDict["ResetApplyBtn"].GetComponent<Button>();
        layoutApplyBtn                          = gameObjectDict["LayoutApplyBtn"].GetComponent<Button>();
        languageApplyBtn                        = gameObjectDict["LanguageApplyBtn"].GetComponent<Button>();
        layoutDropdown                          = gameObjectDict["LayoutDropdown"].GetComponent<Dropdown>();
        handednessDropdown                      = gameObjectDict["HandednessDropdown"].GetComponent<Dropdown>();
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
            resetDelegateList.Add(ResetTotalTime);
            resetDelegateList.Add(ResetTotalGame);
            resetDelegateList.Add(ResetAchievement);
            resetDelegateList.Add(ResetSaveFile);
            resetDelegateList.Add(ResetSaveFileWithoutAchievement);
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
    private void ResetSaveFileWithoutAchievement()
    {
        GameManager.Instance.ResetSaveFileWithoutAchievement();
    }
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
            case "AboutUs2SetUpFrameBtn":
                aboutUsWin.SetActive(!aboutUsWin.activeSelf);
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
                break;
            case "Language2SetUpFrameBtn":
                languageWin.SetActive(false);
                break;
            case "LanguageApplyBtn":
                GameManager.Instance.CurLanguageID = (LanguageID)tempLanguageID;
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositonList, (int)GameManager.Instance.CurLanguageID);
                languageApplyBtn.interactable = false;
                RefreshGui();
                break;
            case "LayoutBtn":
                firstInLayout = true;
                tempLayoutID = (int)GameManager.Instance.CurLayoutID;
                tempHandednessID = (int)GameManager.Instance.CurHandednessID;
                layoutWin.SetActive(true);
                RefreshDropdown(layoutDropdown, tempLayoutID);
                RefreshDropdown(handednessDropdown, tempHandednessID);
                layoutApplyBtn.interactable = false;
                firstInLayout = false;
                break;
            case "HorizontalLayoutTipConfirmBtn":
                horizontalLayoutTipBg.SetActive(false);
                break;
            case "Layout2SetUpFrameBtn":
                layoutWin.SetActive(false);
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
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, MoveID.FromAndToRight, false);
                //GameManager.Instance.SwitchWrapper(GuiFrameID.SetUpFrame, GuiFrameID.StartFrame);
                break;
            case "ShareUsBtn":
                shareUsWin.SetActive(true);
                break;
            case "ShareUsWin":
                shareUsWin.SetActive(false);
                break;
            case "ResetBtn":
                resetWin.SetActive(true);
                RefreshResetWin();
                break;
            case "Reset2SetUpFrameBtn":
                resetWin.SetActive(false);
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
                break;
            case "Skin2SetUpFrameBtn":
                skinWin.SetActive(false);
                break;
            case "SkinApplyBtn":
                GameManager.Instance.CurSkinID = (SkinID)tempSkinID;
                RefreshToggleGroup(skinToggleGroup, skinTogglesAnchoredPositonList, (int)GameManager.Instance.CurSkinID);
                skinApplyBtn.interactable = false;
                RefreshGui();
                break;
            case "StrategyBtn":
            case "Strategy2SetUpFrameBtn":
                strategyWin.SetActive(!strategyWin.activeSelf);
                break;
            case "ThankDevelopersBtn":
            case "ThankDevelopersWin":
                thankDevelopersWin.SetActive(!thankDevelopersWin.activeSelf);
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
                if (tgl.index == 2 || tgl.index == 3)
                {
                    resetTempTgl = tgl;
                    ShowTip();
                    return;
                }
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
        layoutApplyBtn.interactable = true;
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
