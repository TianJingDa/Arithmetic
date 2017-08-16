using System.Collections;
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
            case "CloseBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.StartFrame);
                break;
            case "StartBtn":                
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.ExamFrame);
                break;
            default:
                Debug.LogError("Can not find Button:" + btn.name);
                break;
        }
    }

}
