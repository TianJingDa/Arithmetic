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
    private int tempLanguageID;
    private int tempSkinID;
    private int tempLayoutID;
    private int tempHandednessID;
    private List<Vector2> languageTogglesAnchoredPositon;
    private List<Vector2> skinTogglesAnchoredPositon;

    private GameObject strategyWin;
    private GameObject languageWin;
    private GameObject skinWin;
    private GameObject layoutWin;
    private GameObject resetWin;
    private GameObject feedbackWin;
    private GameObject aboutUsWin;
    private GameObject thankDevelopersWin;
    private Button languageConfirmBtn;
    private Button skinConfirmBtn;
    private Button layoutConfirmBtn;
    private ToggleGroup languageToggleGroup;
    private ToggleGroup skinToggleGroup;
    private Dropdown layoutDropdown;
    private Dropdown handednessDropdown;
    void Start () 
	{
        base.id = GuiFrameID.SetUpFrame;
        InitGui();

        strategyWin = CommonTool.GetGameObjectByName(gameObject, "StrategyWin");
        languageWin = CommonTool.GetGameObjectByName(gameObject, "LanguageWin");
        skinWin = CommonTool.GetGameObjectByName(gameObject, "SkinWin");
        layoutWin = CommonTool.GetGameObjectByName(gameObject, "LayoutWin");
        resetWin = CommonTool.GetGameObjectByName(gameObject, "ResetWin");
        feedbackWin = CommonTool.GetGameObjectByName(gameObject, "FeedbackWin");
        aboutUsWin = CommonTool.GetGameObjectByName(gameObject, "AboutUsWin");
        thankDevelopersWin = CommonTool.GetGameObjectByName(gameObject, "ThankDevelopersWin");
        languageConfirmBtn = CommonTool.GetComponentByName<Button>(gameObject, "LanguageConfirmBtn");
        languageToggleGroup = CommonTool.GetComponentByName<ToggleGroup>(gameObject, "LanguageToggleGroup");
        skinConfirmBtn = CommonTool.GetComponentByName<Button>(gameObject, "SkinConfirmBtn");
        skinToggleGroup = CommonTool.GetComponentByName<ToggleGroup>(gameObject, "SkinToggleGroup");
        layoutDropdown = CommonTool.GetComponentByName<Dropdown>(gameObject, "LayoutDropdown");
        handednessDropdown = CommonTool.GetComponentByName<Dropdown>(gameObject, "HandednessDropdown");
        layoutConfirmBtn = CommonTool.GetComponentByName<Button>(gameObject, "LayoutConfirmBtn");


        languageTogglesAnchoredPositon = InitToggleAnchoredPositon(languageToggleGroup);
        skinTogglesAnchoredPositon = InitToggleAnchoredPositon(skinToggleGroup);

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
            MyDebug.LogYellow("toggleGroup is null");
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

    public override void OnButtonClick(Button btn)
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
                languageConfirmBtn.interactable = false;
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositon, (int)GameManager.Instance.CurLanguageID);
                break;
            case "Language2SetUpFrameBtn":
                languageWin.SetActive(false);
                break;
            case "LanguageConfirmBtn":
                GameManager.Instance.CurLanguageID = (LanguageID)tempLanguageID;
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositon, (int)GameManager.Instance.CurLanguageID);
                languageConfirmBtn.interactable = false;
                InitGui();
                break;
            case "LayoutBtn":
                tempLayoutID = -1;
                layoutWin.SetActive(true);
                layoutConfirmBtn.interactable = false;
                break;
            case "Layout2SetUpFrameBtn":
                layoutWin.SetActive(false);
                break;
            case "LayoutConfirmBtn":
                GameManager.Instance.CurLayoutID = (LayoutID)tempLayoutID;
                layoutConfirmBtn.interactable = false;
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
            case "Reset2SetUpFrameBtn":
                resetWin.SetActive(!resetWin.activeSelf);
                break;
            case "SkinBtn":
                tempSkinID = -1;
                skinWin.SetActive(true);
                skinConfirmBtn.interactable = false;
                RefreshToggleGroup(skinToggleGroup, skinTogglesAnchoredPositon, (int)GameManager.Instance.CurSkinID);
                break;
            case "Skin2SetUpFrameBtn":
                skinWin.SetActive(false);
                break;
            case "SkinConfirmBtn":
                GameManager.Instance.CurSkinID = (SkinID)tempSkinID;
                RefreshToggleGroup(skinToggleGroup, skinTogglesAnchoredPositon, (int)GameManager.Instance.CurSkinID);
                skinConfirmBtn.interactable = false;
                InitGui();
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
                MyDebug.LogYellow("Can not find Button:" + btn.name);
                break;
        }
    }

    public override void OnToggleClick(Toggle tgl)
    {
        base.OnToggleClick(tgl);
        if (tgl.name.Contains("LanguageToggle"))
        {
            OnToggleClick(languageConfirmBtn, ref tempLanguageID, tgl);
        }
        else if (tgl.name.Contains("SkinToggle"))
        {
            OnToggleClick(skinConfirmBtn, ref tempSkinID, tgl);
        }
    }
    private void OnToggleClick(Button confirmBtn, ref int tempID, Toggle tgl)
    {
        if (!confirmBtn.interactable && tempID != -1)
        {
            confirmBtn.interactable = true;
        }
        if (tgl.isOn)
        {
            tempID = tgl.index;
        }
    }

    public override void OnDropdownClick(Dropdown dpd)
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
