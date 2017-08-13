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
    public GameObject confirmWin;

	void Start () 
	{
        base.id = CurExamID;
        InitGui();
        GameManager.Instance.RegisterClock(new Clock(timeLabel));
    }

    void Update () 
	{
		
	}
    void OnDestroy()
    {
        GameManager.Instance.UnRegisterClock();
    }

    public override void OnClick(Button btn)
    {
        base.OnClick(btn);
        switch (btn.name)
        {
            case "FinishBtn":
                confirmWin.SetActive(true);
                //不可暂停答题！
                break;
            case "ConfirmWin":
            case "CancelBtn":
                confirmWin.SetActive(false);
                break;
            case "ConfirmBtn":
                GameManager.Instance.SwitchWrapper(CurExamID, GuiFrameID.SettlementFrame);
                break;
            default:
                Debug.Log("Can not find Button:" + btn.name);
                break;
        }
    }


}
