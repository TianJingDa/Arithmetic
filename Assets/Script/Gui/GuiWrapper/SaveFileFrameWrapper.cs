using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileFrameWrapper : GuiFrameWrapper
{
    private bool onlyWrong;
    private bool isBluetooth;
    private List<QuestionInstance> onlyWrongList;

    private SaveFileInstance content;
    private GameObject saveFileDetailBg;
    private GameObject commonResult;
    private GameObject onlyWrongImage;
    private GameObject achievementBtn;
    private GameObject bluetoothResult;
    private GameObject bluetoothOnlyWrongImage;
    private Text saveFileDetailTime;
    private Text saveFileDetailAmount;
    private Text saveFileDetailAccuracy;
    private Text saveFileOwnName;
    private Text saveFileOtherName;
    private RectTransform saveFileDetailGrid;

    void Start()
    {
        id = GuiFrameID.SaveFileFrame;
        Init();
        content = GameManager.Instance.CurSaveFileInstance;
        saveFileDetailTime.text = string.Format(saveFileDetailTime.text, content.timeCost.ToString("f1"));
        saveFileDetailAmount.text = string.Format(saveFileDetailAmount.text, content.qInstancList.Count);
        saveFileDetailAccuracy.text = string.Format(saveFileDetailAccuracy.text, content.accuracy.ToString("f1"));
        achievementBtn.SetActive(GameManager.Instance.LastGUI != GuiFrameID.ShareFrame 
                              && !string.IsNullOrEmpty(content.achievementName));
        isBluetooth = !string.IsNullOrEmpty(content.opponentName);
        commonResult.SetActive(!isBluetooth);
        bluetoothResult.SetActive(isBluetooth);
        if (isBluetooth)
        {
            saveFileOwnName.text = GameManager.Instance.UserName;
            saveFileOtherName.text = content.opponentName;
        }
        onlyWrongList = content.qInstancList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
        CommonTool.GuiVerticalMove(saveFileDetailBg, Screen.height, MoveID.LeftOrDown, canvasGroup, true);
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        saveFileDetailBg                = gameObjectDict["SaveFileDetailBg"];
        commonResult                    = gameObjectDict["CommonResult"];
        onlyWrongImage                  = gameObjectDict["OnlyWrongImage"];
        achievementBtn                  = gameObjectDict["AchievementBtn"];
        bluetoothResult                 = gameObjectDict["BluetoothResult"];
        bluetoothOnlyWrongImage         = gameObjectDict["BluetoothOnlyWrongImage"];
        saveFileDetailTime              = gameObjectDict["SaveFileDetailTime"].GetComponent<Text>();
        saveFileDetailAmount            = gameObjectDict["SaveFileDetailAmount"].GetComponent<Text>();
        saveFileDetailAccuracy          = gameObjectDict["SaveFileDetailAccuracy"].GetComponent<Text>();
        saveFileOwnName                 = gameObjectDict["SaveFileOwnName"].GetComponent<Text>();
        saveFileOtherName               = gameObjectDict["SaveFileOtherName"].GetComponent<Text>();
        saveFileDetailGrid              = gameObjectDict["SaveFileDetailGrid"].GetComponent<RectTransform>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "ShareBtn":
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.SaveFile);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            case "BluetoothShareBtn":
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.Bluetooth);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            case "OnlyWrongBtn":
            case "BluetoothOnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "SaveFileDetailClosedBtn":
                CommonTool.GuiVerticalMove(saveFileDetailBg, Screen.height, MoveID.LeftOrDown, canvasGroup, false, () => GameManager.Instance.SwitchWrapper(GuiFrameID.None));
                break;
            case "AchievementBtn":
                AchievementInstance instance = GameManager.Instance.GetAchievement(content.achievementName);
                GameManager.Instance.CurAchievementInstance = instance;
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.Achievement);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            case "UploadDataBtn":
                if (string.IsNullOrEmpty(GameManager.Instance.UserName))
                {
                    GameManager.Instance.SwitchWrapper(GuiFrameID.NameBoardFrame, true);
                    return;
                }

                WWWForm form = new WWWForm();
                form.AddField("pattern", (int)content.cInstance.patternID);
                form.AddField("amount", (int)content.cInstance.amountID);
                form.AddField("symbol", (int)content.cInstance.symbolID);
                form.AddField("digit", (int)content.cInstance.digitID);
                form.AddField("operand", (int)content.cInstance.operandID);
                float result = content.timeCost * (1 - content.accuracy);
                form.AddField("result", result.ToString());
                RankInstance rInstance = new RankInstance();
                rInstance.userName = GameManager.Instance.UserName;
                rInstance.saveFile = content;
                string data = JsonUtility.ToJson(rInstance);
                form.AddField("data", data);

                GameManager.Instance.UploadData(form, null);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private bool FindWrong(QuestionInstance questionInstance)
    {
        int count = questionInstance.instance.Count;
        return questionInstance.instance[count - 1] != questionInstance.instance[count - 2];
    }

    private void RefreshSettlementGrid()
    {
        onlyWrongImage.SetActive(onlyWrong);
        bluetoothOnlyWrongImage.SetActive(onlyWrong);
        ArrayList dataList;
        if (onlyWrong)
        {
            dataList = new ArrayList(onlyWrongList);
        }
        else
        {
            dataList = new ArrayList(content.qInstancList);
        }

        if (isBluetooth) CommonTool.RefreshScrollContent(saveFileDetailGrid, dataList, GuiItemID.BluetoothQuestionItem);
        else CommonTool.RefreshScrollContent(saveFileDetailGrid, dataList, GuiItemID.QuestionItem);
    }
}
