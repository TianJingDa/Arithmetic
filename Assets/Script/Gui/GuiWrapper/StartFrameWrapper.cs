﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// 开始界面
/// </summary>
public class StartFrameWrapper : GuiFrameWrapper
{
    void Start()
    {
        id = GuiFrameID.StartFrame;
        Init();

        //string path = Application.dataPath + "/Resources/Layout/Vertical/Default.txt";
        //Dictionary<string, MyRectTransform> dataList = (Dictionary<string, MyRectTransform>)IOHelper.GetDataFromResources(path,typeof(Dictionary<string, MyRectTransform>));//(Dictionary<string, RectTransform>)IOHelper.GetDataFromResources(path, typeof(Dictionary<string, RectTransform>));
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "StatisticsBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StatisticsFrame, MoveID.LeftOrDown, true);
                break;
            case "CategoryBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, true);
                break;
            case "SetUpBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SetUpFrame, MoveID.RightOrUp, true);
                break;
            case "ChapterBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.ChapterFrame, true);
                break;
            case "BluetoothBtn":
                if (Application.isEditor) return;
				if (string.IsNullOrEmpty(GameManager.Instance.UserName))
				{
					//此处需要弹框，让玩家起名，弹框应该做成独立UI
					MyDebug.LogYellow("Please Set Your Name!");
					return;
				}
                GameManager.Instance.SwitchWrapper(GuiFrameID.BluetoothFrame, true);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }
}
