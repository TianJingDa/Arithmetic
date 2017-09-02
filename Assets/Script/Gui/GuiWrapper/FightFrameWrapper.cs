﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary>
/// 答题界面
/// </summary>
public class FightFrameWrapper : GuiFrameWrapper
{
    private Text timeLabel;
    private GameObject confirmBg;
    private GameObject confirmBtn;


    void Start () 
	{
        base.id = GuiFrameID.FightFrame;
        RectTransform[] transforms = GameManager.Instance.GetLayoutData();
        InitLayout(transforms);
        InitGui();
        //timeLabel = GetComponentByName<Text>("TimeLabel");
        confirmBg = CommonTool.GetGameObjectByName(gameObject, "ConfirmBg");
        GameManager.Instance.RegisterClock(new Clock(timeLabel));
    }


    void Update () 
	{
		
	}
    void OnDestroy()
    {
        GameManager.Instance.UnRegisterClock();
    }

    public override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "FinishBtn":
                confirmBg.SetActive(true);
                //不可暂停答题！
                break;
            case "ConfirmBg":
            case "CancelBtn":
                confirmBg.SetActive(false);
                break;
            case "ConfirmBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.FightFrame, GuiFrameID.SettlementFrame);
                break;
            default:
                Debug.LogError("Can not find Button:" + btn.name);
                break;
        }
    }

    private void InitLayout(RectTransform[] transforms)
    {

    }


}
