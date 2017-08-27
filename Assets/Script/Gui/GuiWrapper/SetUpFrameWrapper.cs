using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 设置界面
/// </summary>
public class SetUpFrameWrapper : GuiFrameWrapper
{
    private GameObject strategyWin;
    private GameObject languageWin;
    private GameObject skinWin;
    private GameObject layoutWin;
    private GameObject feedbackWin;
    private GameObject aboutUsWin;
    private GameObject thankDevelopersWin;
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
    }


    public override void OnClick(Button btn)
    {
        base.OnClick(btn);
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
            case "Language2SetUpFrameBtn":
                languageWin.SetActive(!languageWin.activeSelf);
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

}
