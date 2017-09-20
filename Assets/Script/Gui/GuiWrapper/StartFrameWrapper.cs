using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 开始界面
/// </summary>
public class StartFrameWrapper : GuiFrameWrapper
{
    void Start()
    {
        id = GuiFrameID.StartFrame;
        Init();
        //List<QuentionInstance> dataList = (List<QuentionInstance>)IOHelper.GetData(@"C:\Users\TJD\AppData\LocalLow\DefaultCompany\Arithmetic\Save\20170920235811.sav", typeof(List<QuentionInstance>));
    }

    protected override void OnButtonClick(Button btn)
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
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }
}
