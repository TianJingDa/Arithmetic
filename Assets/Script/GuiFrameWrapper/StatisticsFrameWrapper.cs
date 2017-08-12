using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 统计数据界面
/// </summary>
public class StatisticsFrameWrapper : GuiFrameWrapper
{

    void Start () 
	{
        base.id = GuiFrameID.StatisticsFrame;
    }

    void Update () 
	{
		
	}

    void OnDestroy()
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
                GameManager.Instance.SwitchWrapper(GuiFrameID.StatisticsFrame, GuiFrameID.StartFrame);
                break;
        }
    }
}
