using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 答题界面
/// </summary>
public class ExamFrameWrapper : GuiFrameWrapper
{
    public Text timeLabel;
    public GameObject confirmPage;

	void Start () 
	{
        base.id = GuiFrameID.ExamFrame;
    }

    void Update () 
	{
		
	}
    void OnDestroy()
    {
        GameManager.Instance.UnRegisterClock();
    }

    public override void InitUI()
    {
        GameManager.Instance.RegisterClock(new Clock(timeLabel));
    }
    public void OnClick(Button btn)
    {
        switch (btn.name)
        {
            case "FinishBtn":
                confirmPage.SetActive(true);
                break;
            case "StartBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.ExamFrame);
                break;
        }
    }


}
