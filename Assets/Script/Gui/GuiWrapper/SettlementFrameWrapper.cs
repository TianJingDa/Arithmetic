using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 结算界面
/// </summary>
public class SettlementFrameWrapper : GuiFrameWrapper
{
    private Text settlementTime_Text;
    private Text settlementAmount_Text;
    private Text settlementAccuracy_Text;
    private InfiniteList settlementGrid;

    void Start () 
	{
        id = GuiFrameID.SettlementFrame;
        Init();
        settlementTime_Text.text = settlementTime_Text.text.Replace("{0}", GameManager.Instance.CurTimeCost.ToString());
        settlementAmount_Text.text = settlementAmount_Text.text.Replace("{0}", GameManager.Instance.CurResultList.Count.ToString());
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        settlementGrid          = GameObjectDict["SettlementGrid"].GetComponent<InfiniteList>();
        settlementTime_Text     = GameObjectDict["SettlementTime_Text"].GetComponent<Text>();
        settlementAmount_Text   = GameObjectDict["SettlementAmount_Text"].GetComponent<Text>();
        settlementAccuracy_Text = GameObjectDict["SettlementAccuracy_Text"].GetComponent<Text>();
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
