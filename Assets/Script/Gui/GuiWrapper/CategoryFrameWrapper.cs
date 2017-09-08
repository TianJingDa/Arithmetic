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
        InitUnSelectableGui();
        InitSelectableGui(OnButtonClick, null, OnDropdownClick);


        categoryTipBg = CommonTool.GetGameObjectByName(gameObject, "CategoryTipBg");
    }


    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Category2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.StartFrame);
                break;
            case "Category2ExamFrameBtn":                
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.FightFrame);
                break;
            case "CategoryTipBtn":
            case "CategoryTipBg":
                categoryTipBg.SetActive(!categoryTipBg.activeSelf);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

}
