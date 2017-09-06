using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 开始界面
/// </summary>
public class StartFrameWrapper : GuiFrameWrapper
{
    void Start()
    {
        base.id = GuiFrameID.StartFrame;
        InitGui();
    }
    public override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "StatisticsBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, GuiFrameID.StatisticsFrame);
                break;
            case "CategoryBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, GuiFrameID.CategoryFrame);
                break;
            case "SetUpBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, GuiFrameID.SetUpFrame);
                break;
            default:
                MyDebug.LogYellow("Can not find Button:" + btn.name);
                break;
        }
    }
}
