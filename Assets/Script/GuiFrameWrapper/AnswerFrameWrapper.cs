using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 答题界面
/// </summary>
public class AnswerFrameWrapper : GuiFrameWrapper
{
    public Text timeLabel;

	void Start () 
	{
        InitUI();
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

}
