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
                GameManager.Instance.SwitchWrapper(GuiFrameID.SetUpFrame, GuiFrameID.StartFrame);
                break;
        }
    }

}
