using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 设置界面
/// </summary>
public class SetUpFrameWrapper : GuiFrameWrapper
{
    void Start () 
	{
        base.id = GuiFrameID.SetUpFrame;
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
                GameManager.Instance.SwitchWrapper(GuiFrameID.SetUpFrame, GuiFrameID.StartFrame);
                break;
            default:
                Debug.Log("Can not find Button:" + btn.name);
                break;
        }
    }

}
