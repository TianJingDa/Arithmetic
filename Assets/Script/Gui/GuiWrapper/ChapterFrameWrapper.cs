using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterFrameWrapper : GuiFrameWrapper
{


	void Start ()
    {
        id = GuiFrameID.ChapterFrame;
        Init();

    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {

    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Chapter2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, false);
                break;
            default:
                MyDebug.LogYellow("Can not find Button:" + btn.name);
                break;
        }
    }
}
