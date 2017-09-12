using System.Collections;
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
        id = GuiFrameID.FightFrame;
        RectTransform[] transforms = GameManager.Instance.GetLayoutData();
        InitLayout(transforms);
        Init();

        //timeLabel = GetComponentByName<Text>("TimeLabel");
        GameManager.Instance.RegisterClock(new Clock(timeLabel));
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict, 
                                    Dictionary<string, Button>     ButtonDict, 
                                    Dictionary<string, Toggle>     ToggleDict, 
                                    Dictionary<string, Dropdown>   DropdownDict)
    {
        confirmBg = ButtonDict["ConfirmBg"].gameObject;
    }

    void OnDestroy()
    {
        GameManager.Instance.UnRegisterClock();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Fight2SettlementFrameBtn":
                confirmBg.SetActive(true);
                //不可暂停答题！
                break;
            case "ConfirmBg":
            case "CancelBtn":
                confirmBg.SetActive(false);
                break;
            case "ConfirmBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.FightFrame, GuiFrameID.CategoryFrame);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private void InitLayout(RectTransform[] transforms)
    {

    }


}
