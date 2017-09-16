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
        id = GuiFrameID.SettlementFrame;
        Init();
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {

    }


    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "OnlyWrongBtn":
                Debug.Log("OnlyWrong!!!");
                break;
            case "Settlement2CategoryFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.CategoryFrame);
                break;
            case "Settlement2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.StartFrame);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }
}
