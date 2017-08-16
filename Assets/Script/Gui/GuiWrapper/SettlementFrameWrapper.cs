using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 结算界面
/// </summary>
public class SettlementFrameWrapper : GuiFrameWrapper
{

    void Start () 
	{
        base.id = GuiFrameID.SettlementFrame;
        InitGui();
    }

    void Update () 
	{
		
	}

    public override void OnClick(Button btn)
    {
        base.OnClick(btn);
        switch (btn.name)
        {
            case "BackBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.StartFrame);
                break;
            case "ContinueBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.CategoryFrame);
                break;
            default:
                Debug.LogError("Can not find Button:" + btn.name);
                break;
        }
    }
}
