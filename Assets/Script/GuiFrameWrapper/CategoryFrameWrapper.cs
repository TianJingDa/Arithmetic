﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 速算类别选择界面
/// </summary>
public class CategoryFrameWrapper : GuiFrameWrapper
{

    void Start () 
	{
        base.id = GuiFrameID.CategoryFrame;
    }

    void Update () 
	{
		
	}

    public override void InitUI()
    {

    }
    public void OnClick(Button btn)
    {
        switch (btn.name)
        {
            case "CloseBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.StartFrame);
                break;
            case "StartBtn":                
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.ExamFrame);
                break;
        }
    }

}
