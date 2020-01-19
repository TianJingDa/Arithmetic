using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NameboardFrameWrapper : GuiFrameWrapper
{
    private const float     TimeOut = 1f;
	private const string    NameURL = "http://47.105.77.226:8091/changeName";

    private bool            isCreating;
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
				if(GameManager.Instance.IsLogin)
				{
                    CreateUserName();
                }
				else
				{
                    GameManager.Instance.StartSilentLogin(CreateUserName, OnSilentLoginFail);
				}
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

    private void OnSilentLoginFail(string message)
    {
        GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
        GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
    }

    private void CreateUserName()
    {
        if (isCreating)
        {
            return;
        }
        isCreating = true;
        StartCoroutine(CreateUserName(userName));
    }

    private IEnumerator CreateUserName(string name)
    {
		WWWForm form = new WWWForm();
		form.AddField("userId", GameManager.Instance.UserID);
		form.AddField("jwttoken", GameManager.Instance.Token);
		form.AddField("userName", name);
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
				if (response.code == 200)
                {
                    MyDebug.LogGreen("Create User Name Succeed:" + name);
                    isCreating = false;
                    GameManager.Instance.UserName = name;
                    GameManager.Instance.SwitchWrapper(GuiFrameID.None);
                    yield break;
                }
                else
                {
					MyDebug.LogYellow("Create User Name Fail:" + response.code);
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
        isCreating = false;
        GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
        GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
    }

	[Serializable]
    private class CreateNameResponse
    {
        public int code;
		public string errmsg;
    }
}