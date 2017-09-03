﻿using System.Collections;
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
    private List<Vector2> languageTogglesAnchoredPositon;

    private GameObject strategyWin;
    private GameObject languageWin;
    private GameObject skinWin;
    private GameObject layoutWin;
    private GameObject feedbackWin;
    private GameObject aboutUsWin;
    private GameObject thankDevelopersWin;
    private ToggleGroup languageToggleGroup;
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
        languageToggleGroup = CommonTool.GetComponentByName<ToggleGroup>(gameObject, "LanguageToggleGroup");
        languageTogglesAnchoredPositon = InitToggleAnchoredPositon(languageToggleGroup);

    }

    private void RefreshToggleGroup(ToggleGroup toggleGroup, List<Vector2> togglesAnchoredPositon)
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
        int curLanguageID = (int)GameManager.Instance.curLanguageID;
        Vector2 curToggleAnchoredPositon = togglesAnchoredPositon[curLanguageID];
        togglesAnchoredPositon.RemoveAt(curLanguageID);
        togglesAnchoredPositon.Insert(0, curToggleAnchoredPositon);
        for(int i=0;i< toggleGroup.toggles.Count; i++)
        {
            RectTransform toggleRectTransform = toggleGroup.toggles[i].transform as RectTransform;
            toggleRectTransform.anchoredPosition = togglesAnchoredPositon[i];
        }
        toggleGroup.NotifyToggleOn(0);
    }

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
                languageWin.SetActive(!languageWin.activeSelf);
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositon);
                break;
            case "Language2SetUpFrameBtn":
                languageWin.SetActive(!languageWin.activeSelf);
                break;
            case "LanguageConfirmBtn":
                GameManager.Instance.curLanguageID = (LanguageID)tempLanguageID;
                RefreshToggleGroup(languageToggleGroup, languageTogglesAnchoredPositon);
                break;
            case "LayoutBtn":
            case "Layout2SetUpFrameBtn":
                layoutWin.SetActive(!layoutWin.activeSelf);
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
            case "Skin2SetUpFrameBtn":
                skinWin.SetActive(!skinWin.activeSelf);
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
    public override void OnToggleClick(bool check)
    {
        if (check)
        {
            tempLanguageID = languageToggleGroup.ActiveToggles().FirstOrDefault().index;
            Debug.Log("tempLanguageID:" + tempLanguageID);
        }
    }


}
