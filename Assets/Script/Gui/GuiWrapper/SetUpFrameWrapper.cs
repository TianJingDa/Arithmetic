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
    private List<Vector2> languageTogglesAnchoredPositon;
    private List<Vector2> skinTogglesAnchoredPositon;
    private List<Vector2> layoutTogglesAnchoredPositon;

    private GameObject strategyWin;
    private GameObject languageWin;
    private GameObject skinWin;
    private GameObject layoutWin;
    private GameObject feedbackWin;
    private GameObject aboutUsWin;
    private GameObject thankDevelopersWin;
    private Button languageConfirmBtn;
    private Button skinConfirmBtn;
    private Button layoutConfirmBtn;
    private ToggleGroup languageToggleGroup;
    private ToggleGroup skinToggleGroup;
    private ToggleGroup layoutToggleGroup;
    void Start () 
	{
        base.id = GuiFrameID.SetUpFrame;
        InitGui();

        strategyWin = CommonTool.GetGameObjectByName(gameObject, "StrategyWin");
        languageWin = CommonTool.GetGameObjectByName(gameObject, "LanguageWin");
        skinWin = CommonTool.GetGameObjectByName(gameObject, "SkinWin");
        layoutWin = CommonTool.GetGameObjectByName(gameObject, "LayoutWin");
        feedbackWin = CommonTool.GetGameObjectByName(gameObject, "FeedbackWin");
        aboutUsWin = CommonTool.GetGameObjectByName(gameObject, "AboutUsWin");
        thankDevelopersWin = CommonTool.GetGameObjectByName(gameObject, "ThankDevelopersWin");
        languageConfirmBtn = CommonTool.GetComponentByName<Button>(gameObject, "LanguageConfirmBtn");
        languageToggleGroup = CommonTool.GetComponentByName<ToggleGroup>(gameObject, "LanguageToggleGroup");
        skinConfirmBtn = CommonTool.GetComponentByName<Button>(gameObject, "SkinConfirmBtn");
        skinToggleGroup = CommonTool.GetComponentByName<ToggleGroup>(gameObject, "SkinToggleGroup");
        layoutConfirmBtn = CommonTool.GetComponentByName<Button>(gameObject, "LayoutConfirmBtn");
        layoutToggleGroup = CommonTool.GetComponentByName<ToggleGroup>(gameObject, "LayoutToggleGroup");

        languageTogglesAnchoredPositon = InitToggleAnchoredPositon(languageToggleGroup);
        skinTogglesAnchoredPositon = InitToggleAnchoredPositon(skinToggleGroup);
        layoutTogglesAnchoredPositon = InitToggleAnchoredPositon(layoutToggleGroup);
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
            Debug.Log("toggleGroup is null");
            return;
        }
        if (togglesAnchoredPositon == null || togglesAnchoredPositon.Count == 0)
        {
            Debug.Log("togglesAnchoredPositon NO data!");
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
            Debug.Log("toggleGroup is null");
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
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositon, (int)GameManager.Instance.curLanguageID);
                break;
            case "Language2SetUpFrameBtn":
                languageWin.SetActive(false);
                break;
            case "LanguageConfirmBtn":
                GameManager.Instance.SetLanguageID(tempLanguageID);
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositon, (int)GameManager.Instance.curLanguageID);
                languageConfirmBtn.interactable = false;
                InitGui();
                break;
            case "LayoutBtn":
                tempLayoutID = -1;
                layoutWin.SetActive(true);
                layoutConfirmBtn.interactable = false;
                RefreshToggleGroup(layoutToggleGroup, layoutTogglesAnchoredPositon, (int)GameManager.Instance.curLayoutID);
                break;
            case "Layout2SetUpFrameBtn":
                layoutWin.SetActive(false);
                break;
            case "LayoutConfirmBtn":
                GameManager.Instance.SetLayoutID(tempLayoutID);
                RefreshToggleGroup(layoutToggleGroup, layoutTogglesAnchoredPositon, (int)GameManager.Instance.curLayoutID);
                layoutConfirmBtn.interactable = false;
                break;
            case "AboutUs2StartFrameBtn":
            case "Language2StartFrameBtn":
            case "Layout2StartFrameBtn":
            case "SetUp2StartFrameBtn":
            case "Skin2StartFrameBtn":
            case "Strategy2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SetUpFrame, GuiFrameID.StartFrame);
                break;
            case "SkinBtn":
                tempSkinID = -1;
                skinWin.SetActive(true);
                skinConfirmBtn.interactable = false;
                RefreshToggleGroup(skinToggleGroup, skinTogglesAnchoredPositon, (int)GameManager.Instance.curSkinID);
                break;
            case "Skin2SetUpFrameBtn":
                skinWin.SetActive(false);
                break;
            case "SkinConfirmBtn":
                GameManager.Instance.SetSkinID(tempSkinID);
                RefreshToggleGroup(skinToggleGroup, skinTogglesAnchoredPositon, (int)GameManager.Instance.curSkinID);
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
                Debug.LogError("Can not find Button:" + btn.name);
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
        else if (tgl.name.Contains("LayoutToggle"))
        {
            OnToggleClick(layoutConfirmBtn, ref tempLayoutID, tgl);
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
    //public override void OnToggleClick(bool check)
    //{
    //    if (check)
    //    {
    //        tempLanguageID = languageToggleGroup.ActiveToggles().FirstOrDefault().index;
    //        Debug.Log("tempLanguageID:" + tempLanguageID);
    //    }
    //}


}
