using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameboardFrameWrapper : GuiFrameWrapper
{
    private const float     TimeOut = 1f;
    private const string    NameURL = "";

    private string          userName;

    private GameObject      nameBoardPage;
    private GameObject      nameTipBoard;

    private Text            nameTipBoardContent;
    private InputField      nameBoardInputField;

    void Start()
    {
        id = GuiFrameID.NameBoardFrame;
        Init();
        nameBoardInputField.onEndEdit.AddListener(OnEndEdit);
        CommonTool.GuiScale(nameBoardPage, canvasGroup, true);
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        nameBoardPage           = gameObjectDict["NameBoardPage"];
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
                StartCoroutine(CreateUserName(userName));
                break;
            case "NameTipBoardCancelBtn":
                nameTipBoard.SetActive(false);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private void OnEndEdit(string text)
    {
        userName = text;
    }

    private IEnumerator CreateUserName(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        WWW www = new WWW(NameURL, form);

        float responseTime = 0;
        while (!www.isDone && responseTime < TimeOut)
        {
            responseTime += Time.deltaTime;
            yield return www;
        }

        string message = "";
        if (www.isDone)
        {
            CreateNameResponse response = JsonUtility.FromJson<CreateNameResponse>(www.text);
            if (response != null)
            {
                if (response.error == 0)
                {
                    MyDebug.LogGreen("Create User Name Succeed:" + response.name);
                    GameManager.Instance.UserName = response.name;
                    GameManager.Instance.SwitchWrapper(GuiFrameID.None);
                    yield break;
                }
                else
                {
                    MyDebug.LogYellow("Create User Name Fail:" + response.error);
                    message = GameManager.Instance.GetMutiLanguage("Text_20066");
                }
            }
            else
            {
                MyDebug.LogYellow("Create User Name: Message Is Not Response!");
                message = GameManager.Instance.GetMutiLanguage("Text_20066");
            }
        }
        else
        {
            MyDebug.LogYellow("Create User Name Fail: Long Time!");
            message = GameManager.Instance.GetMutiLanguage("Text_20067");
        }
        GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
        GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
    }

    private class CreateNameResponse
    {
        public int error;
        public string name;
    }
}