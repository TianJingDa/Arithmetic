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
        id = GuiFrameID.CategoryFrame;
        Init();
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict, 
                                    Dictionary<string, Button>     ButtonDict, 
                                    Dictionary<string, Toggle>     ToggleDict, 
                                    Dictionary<string, Dropdown>   DropdownDict)
    {
        categoryTipBg = ButtonDict["CategoryTipBg"].gameObject;
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
