using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
/// <summary>
/// 答题界面
/// </summary>
public class FightFrameWrapper : GuiFrameWrapper
{
    private int                 countdownTime = 3;
    private StringBuilder       answer;

    private Action              fighting;
    private GameObject          giveUpBg;
    private GameObject          countdownBg;
    private Text                timeBtn_Text;
    private Text                indexImg_Text;
    private Text                resultImg_Text;
    private Text                questionImg_Text;
    private List<GameObject>    countdownNumsList;
    private List<Action>        fightActionList;

    private PatternID           curPatternID;
    private AmountID            curAmountID;

    void Start () 
	{
        id = GuiFrameID.FightFrame;
        RectTransform[] transforms = GameManager.Instance.GetLayoutData();
        InitLayout(transforms);
        Init();
        countdownBg.SetActive(true);
        countdownNumsList = CommonTool.GetGameObjectsByName(countdownBg, "Countdown_");
        GameManager.Instance.GetPatternAndAmount(out curPatternID, out curAmountID);
        fightActionList.Add(FightTime);
        fightActionList.Add(FightNumber);
        answer = new StringBuilder();
        ClearAllText();
        StartCoroutine(StartFight());
    }

    void Update()
    {
        if (fighting != null) fighting();
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict,
                                Dictionary<string, Button> ButtonDict,
                                Dictionary<string, Toggle> ToggleDict,
                                Dictionary<string, Dropdown> DropdownDict)
    {
        giveUpBg            = ButtonDict["GiveUpBg"].gameObject;
        countdownBg         = GameObjectDict["CountdownBg"];
        timeBtn_Text        = GameObjectDict["TimeBtn_Text"].GetComponent<Text>();
        indexImg_Text       = GameObjectDict["IndexImg_Text"].GetComponent<Text>();
        resultImg_Text      = GameObjectDict["ResultImg_Text"].GetComponent<Text>();
        questionImg_Text    = GameObjectDict["QuestionImg_Text"].GetComponent<Text>();
    }


    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                RefreshResultText(btn.name);
                break;
            case "NextBtn":
                ShowNextQuestion();
                break;
            case "ClearBtn":
                ClearResultText();
                break;
            case "OrderBtn":
                ChangeInputOrder();
                break;
            case "TimeBtn":
                ChangeTimeVisibility();
                break;
            case "Fight2SettlementFrameBtn":
                giveUpBg.SetActive(true);
                //不可暂停答题！
                break;
            case "GiveUpBg":
            case "CancelBtn":
                giveUpBg.SetActive(false);
                break;
            case "ConfirmBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.FightFrame, GuiFrameID.CategoryFrame);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private void InitLayout(RectTransform[] transforms)
    {

    }

    private IEnumerator StartFight()
    {
        while (countdownTime > 0)
        {
            countdownNumsList[3 - countdownTime].SetActive(true);
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        countdownBg.SetActive(false);
        ShowNextQuestion();
        fighting = fightActionList[(int)curPatternID];
    }

    private void ClearAllText()
    {
        timeBtn_Text.text = string.Empty;
        indexImg_Text.text = string.Empty;
        resultImg_Text.text = string.Empty;
        questionImg_Text.text = string.Empty;
    }

    private void FightTime()
    {

    }

    private void FightNumber()
    {

    }

    private void RefreshResultText(string num)
    {

    }

    private void ShowNextQuestion()
    {

    }

    private void ClearResultText()
    {
        resultImg_Text.text = string.Empty;
        //answer.c
    }

    private void ChangeInputOrder()
    {

    }

    private void ChangeTimeVisibility()
    {

    }
}
