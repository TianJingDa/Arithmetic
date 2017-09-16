using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
/// <summary>
/// 设置界面
/// </summary>
public class SetUpFrameWrapper : GuiFrameWrapper
{
    private delegate void resetDelegate();

    private int tempLanguageID;
    private int tempSkinID;
    private int tempLayoutID;
    private int tempHandednessID;

    private List<int> resetTogglesIndexList;
    private List<Vector2> languageTogglesAnchoredPositonList;
    private List<Vector2> skinTogglesAnchoredPositonList;
    private Dictionary<int, resetDelegate> resetDelegateDict;

    private GameObject strategyWin;
    private GameObject languageWin;
    private GameObject skinWin;
    private GameObject layoutWin;
    private GameObject resetWin;
    private GameObject feedbackWin;
    private GameObject aboutUsWin;
    private GameObject thankDevelopersWin;
    private GameObject resetConfirmBg;
    private GameObject resetToggleGroup;
    private Button languageApplyBtn;
    private Button skinApplyBtn;
    private Button layoutApplyBtn;
    private Button resetApplyBtn;
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

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict, 
                                    Dictionary<string, Button>     ButtonDict, 
                                    Dictionary<string, Toggle>     ToggleDict, 
                                    Dictionary<string, Dropdown>   DropdownDict)
    {
        strategyWin         = GameObjectDict["StrategyWin"];
        languageWin         = GameObjectDict["LanguageWin"];
        skinWin             = GameObjectDict["SkinWin"];
        layoutWin           = GameObjectDict["LayoutWin"];
        resetWin            = GameObjectDict["ResetWin"];
        aboutUsWin          = GameObjectDict["AboutUsWin"];
        resetToggleGroup    = GameObjectDict["ResetToggleGroup"];
        skinToggleGroup     = GameObjectDict["SkinToggleGroup"].GetComponent<ToggleGroup>();
        languageToggleGroup = GameObjectDict["LanguageToggleGroup"].GetComponent<ToggleGroup>();
        feedbackWin         = ButtonDict["FeedbackWin"].gameObject;
        thankDevelopersWin  = ButtonDict["ThankDevelopersWin"].gameObject;
        resetConfirmBg      = ButtonDict["ResetConfirmBg"].gameObject;
        languageApplyBtn    = ButtonDict["LanguageApplyBtn"];
        skinApplyBtn        = ButtonDict["SkinApplyBtn"];
        resetApplyBtn       = ButtonDict["ResetApplyBtn"];
        layoutApplyBtn      = ButtonDict["LayoutApplyBtn"];
        layoutDropdown      = DropdownDict["LayoutDropdown"];
        handednessDropdown  = DropdownDict["HandednessDropdown"];
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
        if (resetDelegateDict == null)
        {
            resetDelegateDict = new Dictionary<int, resetDelegate>();
            resetDelegateDict.Add(0, ResetTotalTime);
            resetDelegateDict.Add(1, ResetTotalGame);
            resetDelegateDict.Add(2, ResetAchievement);
            resetDelegateDict.Add(3, ResetSaveFile);
            resetDelegateDict.Add(4, ResetSetUp);
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
        MyDebug.LogGreen("ResetAchievement Success!!!");
    }
    /// <summary>
    /// 重置游戏存档
    /// </summary>
    private void ResetSaveFile()
    {
        MyDebug.LogGreen("ResetSaveFile Success!!!");
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
                tempLayoutID = (int)GameManager.Instance.CurLayoutID;
                tempHandednessID = (int)GameManager.Instance.CurHandednessID;
                layoutWin.SetActive(true);
                RefreshDropdown(layoutDropdown, tempLayoutID);
                RefreshDropdown(handednessDropdown, tempHandednessID);
                layoutApplyBtn.interactable = false;
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
                GameManager.Instance.SwitchWrapper(GuiFrameID.SetUpFrame, GuiFrameID.StartFrame);
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
                    resetDelegateDict[resetTogglesIndexList[i]]();
                }
                RefreshResetWin();
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
            if (tgl.isOn) resetTogglesIndexList.Add(tgl.index);
            else resetTogglesIndexList.Remove(tgl.index);
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

    protected override void OnDropdownClick(Dropdown dpd)
    {
        base.OnDropdownClick(dpd);
        switch (dpd.name)
        {
            case "LayoutDropdown":
                tempLayoutID = dpd.value;
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
