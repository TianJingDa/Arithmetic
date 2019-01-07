using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameboardFrameWrapper : GuiFrameWrapper
{
    private string          userName;

    private GameObject      nameTipBoard;

    private Text            nameTipBoardContent;
    private InputField      nameBoardInputField;

    void Start()
    {
        id = GuiFrameID.NameBoardFrame;
        Init();
        nameBoardInputField.onEndEdit.AddListener(OnEndEdit);
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        nameTipBoard            = gameObjectDict["NameTipBoard"];
        nameTipBoardContent     = gameObjectDict["NameTipBoardContent"].GetComponent<Text>();
        nameBoardInputField     = gameObjectDict["NameBoardInputField"].GetComponent<InputField>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "NameBoardInputFieldConfirmBtn":
                if (string.IsNullOrEmpty(userName)) return;
                nameTipBoard.SetActive(true);
                string curName = GameManager.Instance.GetMutiLanguage(nameTipBoardContent.index);
                nameTipBoardContent.text = string.Format(curName, userName);
                break;
            case "NameBoardInputFieldCancelBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.None);
                break;
            case "NameTipBoardConfirmBtn":
                GameManager.Instance.UserName = userName;
                GameManager.Instance.SwitchWrapper(GuiFrameID.None);
                break;
            case "NameTipBoardCancelBtn":
                nameTipBoard.SetActive(false);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    protected void OnEndEdit(string text)
    {
        userName = text;
    }
}