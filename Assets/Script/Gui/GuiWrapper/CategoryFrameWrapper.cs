using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 速算类别选择界面
/// </summary>
public class CategoryFrameWrapper : GuiFrameWrapper
{
    private GameObject categoryTipBg;

    void Start () 
	{
        base.id = GuiFrameID.CategoryFrame;
        InitGui();
        categoryTipBg = CommonTool.GetGameObjectByName(gameObject, "CategoryTipBg");
    }


    public override void OnClick(Button btn)
    {
        base.OnClick(btn);
        switch (btn.name)
        {
            case "Category2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.StartFrame);
                break;
            case "Category2ExamFrameBtn":                
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.ExamFrame);
                break;
            case "CategoryTipBtn":
            case "CategoryTipBg":
                categoryTipBg.SetActive(!categoryTipBg.activeSelf);
                break;
            default:
                Debug.LogError("Can not find Button:" + btn.name);
                break;
        }
    }

}
