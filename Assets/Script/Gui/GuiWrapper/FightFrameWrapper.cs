using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary>
/// 答题界面
/// </summary>
public class FightFrameWrapper : GuiFrameWrapper
{
    private int                 countdownTime = 3;
    private float               amount;
    private float               startTime;
    private float               timeCost;
    private bool                order;//true: -->; false: <--
    private string              pattern;
    private string              symbol;
    private StringBuilder       result;

    private GameObject          giveUpBg;
    private GameObject          countdownBg;
    private GameObject          reverseOrderImage;
    private GameObject          timeMaskImage;
    private Text                timeBtn_Text;
    private Text                resultImg_Text;
    private Text                questionImg_Text;
    private List<GameObject>    countdownNumsList;
    private List<int>           curInstance;
    private List<List<int>>     resultList;


    void Start () 
	{
        id = GuiFrameID.FightFrame;
        RectTransform[] transforms = GameManager.Instance.GetLayoutData();
        InitLayout(transforms);
        Init();
        timeCost = 0;
        order = true;
        countdownBg.SetActive(true);
        countdownNumsList = CommonTool.GetGameObjectsByName(countdownBg, "Countdown_");
        GameManager.Instance.GetFightParameter(out pattern, out amount, out symbol);
        GameManager.Instance.ResetCheckList();
        result = new StringBuilder();
        resultList = new List<List<int>>();
        ClearAllText();
        StartCoroutine(StartFight());
    }
    
    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        giveUpBg            = GameObjectDict["GiveUpBg"];
        countdownBg         = GameObjectDict["CountdownBg"];
        timeMaskImage       = GameObjectDict["TimeMaskImage"];
        reverseOrderImage   = GameObjectDict["ReverseOrderImage"];
        timeBtn_Text        = GameObjectDict["TimeBtn_Text"].GetComponent<Text>();
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
        startTime = Time.realtimeSinceStartup;
        InvokeRepeating(pattern + "Pattern", 0.1f, 0.1f);
    }

    private void ClearAllText()
    {
        timeBtn_Text.text = string.Empty;
        resultImg_Text.text = string.Empty;
        questionImg_Text.text = string.Empty;
    }

    private void TimePattern()
    {
        float curTime = Time.realtimeSinceStartup;
        timeCost = curTime - startTime;
        timeBtn_Text.text = (amount - timeCost).ToString("f1") + "s";
        if (amount <= 0)
        {
            FightOver();
        }
    }

    private void NumberPattern()
    {
        float curTime = Time.realtimeSinceStartup;
        timeCost = curTime - startTime;
        timeBtn_Text.text = timeCost.ToString("f1") + "s";
    }

    private void RefreshResultText(string num)
    {
        StringBuilder lastResult = new StringBuilder(result.ToString());
        if (result.ToString() == "-1") result.Length = 0;
        if (order) result.Append(num);
        else result.Insert(0, num);
        if (long.Parse(result.ToString()) > int.MaxValue) result = lastResult;
        resultImg_Text.text = result.ToString();
    }

    private void ShowNextQuestion()
    {
        if (result.Length > 0)//check
        {
            curInstance.Add(int.Parse(result.ToString()));
            resultList.Add(curInstance);
            if (pattern == "Number" && resultList.Count == amount)
            {
                FightOver();
                return;
            }
        }
        curInstance = GameManager.Instance.GetQuestionInstance();
        StringBuilder question = new StringBuilder();
        question.Append(curInstance[0].ToString());
        for(int i = 1; i < curInstance.Count - 1; i++)
        {
            question.Append(symbol);
            question.Append(curInstance[i].ToString());
        }
        questionImg_Text.text = question.ToString();
        ClearResultText();
        result.Append("-1");
    }

    private void ClearResultText()
    {
        resultImg_Text.text = string.Empty;
        result.Length = 0;
    }

    private void ChangeInputOrder()
    {
        order = !order;
        reverseOrderImage.SetActive(!reverseOrderImage.activeSelf);//暂时先这么处理
    }

    private void ChangeTimeVisibility()
    {
        timeMaskImage.SetActive(!timeMaskImage.activeSelf);//暂时先这么处理
    }

    private void FightOver()
    {
        GameManager.Instance.CurTimeCost = timeCost;
        GameManager.Instance.CurResultList = resultList;
        GameManager.Instance.SwitchWrapper(GuiFrameID.FightFrame, GuiFrameID.SettlementFrame);
    }
}
