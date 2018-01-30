using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using System;
using DG.Tweening;


public class AchievementItem : Item, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    protected float durationThreshold = 1.0f;
    protected bool isLongPress;
    protected bool onlyWrong;
    protected string userName;

    protected AchievementInstance content;//详情
    protected List<QuentionInstance> onlyWrongList;
    protected List<QuentionInstance> allInstanceList;
    protected GameObject detailWin;
    protected GameObject deleteWin;
    protected GameObject achievementItem_WithoutAchievement;
    protected Dictionary<string, GameObject> detailWinDict;
    protected Text achievementName;
    protected Image achievementImage;
    protected Vector3 position;

    public void OnPointerDown(PointerEventData eventData)
    {
        isLongPress = false;
        position = ((RectTransform)transform).position;
        StartCoroutine("TimeCounter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("TimeCounter");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isLongPress)
        {
            StopCoroutine("TimeCounter");
            OnShortPress();
        }
    }
    protected IEnumerator TimeCounter()
    {
        float duration = 0;
        while (duration < durationThreshold)
        {
            duration += Time.deltaTime;
            yield return null;
        }
        isLongPress = deleteWin != null;
        Vector3 curPosition = ((RectTransform)transform).position;
        float distance = Mathf.Abs(position.y - curPosition.y);
        if (distance <= 2 && isLongPress) OnLongPress();
    }
    protected void OnShortPress()
    {
        if (content == null || string.IsNullOrEmpty(content.finishTime)) return;
        detailWin.SetActive(true);
        CommonTool.GuiScale(detailWin, GameManager.Instance.CurCanvasGroup, true);
        detailWinDict = CommonTool.InitGameObjectDict(detailWin);
        CommonTool.InitText(detailWin);
        CommonTool.InitImage(detailWin);
        GameObject achievementDetailTitleInStatistics = detailWinDict["AchievementDetailTitleInStatistics"];
        Image achievementDetailImageInStatistics = detailWinDict["AchievementDetailImageInStatistics"].GetComponent<Image>();
        Text achievementDetailMainTitleInStatistics = detailWinDict["AchievementDetailMainTitleInStatistics"].GetComponent<Text>();
        Text achievementDetailSubTitleInStatistics = detailWinDict["AchievementDetailSubTitleInStatistics"].GetComponent<Text>();
        Text achievementDetailFinishTimeInStatistics = detailWinDict["AchievementDetailFinishTimeInStatistics"].GetComponent<Text>();
        GameObject achievementDetailPatternInStatistics_Time = detailWinDict["AchievementDetailPatternInStatistics_Time"];
        GameObject achievementDetailPatternInStatistics_Number = detailWinDict["AchievementDetailPatternInStatistics_Number"];
        Text achievementDetailTimeInStatistics = detailWinDict["AchievementDetailTimeInStatistics"].GetComponent<Text>();
        Text achievementDetailAmountInStatistics = detailWinDict["AchievementDetailAmountInStatistics"].GetComponent<Text>();
        Text achievementDetailSymbolInStatistics = detailWinDict["AchievementDetailSymbolInStatistics"].GetComponent<Text>();
        Text achievementDetailDigitInStatistics = detailWinDict["AchievementDetailDigitInStatistics"].GetComponent<Text>();
        Text achievementDetailOperandInStatistics = detailWinDict["AchievementDetailOperandInStatistics"].GetComponent<Text>();
        Text achievementDetailConditionInStatistics = detailWinDict["AchievementDetailConditionInStatistics"].GetComponent<Text>();
        GameObject achievementDetailShareBtnInStatistics = detailWinDict["AchievementDetailShareBtnInStatistics"];

        achievementDetailTitleInStatistics.SetActive(false);
        achievementDetailImageInStatistics.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        achievementDetailMainTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementDetailSubTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.subTitleIndex);
        achievementDetailFinishTimeInStatistics.text = GetFinishTime(content.finishTime);
        bool isTimePattern = content.cInstance.patternID == PatternID.Time;
        achievementDetailPatternInStatistics_Time.SetActive(isTimePattern);
        achievementDetailPatternInStatistics_Number.SetActive(!isTimePattern);
        achievementDetailTimeInStatistics.gameObject.SetActive(isTimePattern);
        achievementDetailAmountInStatistics.gameObject.SetActive(!isTimePattern);
        if (isTimePattern)
        {
            int amount = GameManager.Instance.AmountArray_Time[(int)content.cInstance.amountID];
            achievementDetailTimeInStatistics.text = string.Format(achievementDetailTimeInStatistics.text, amount);
        }
        else
        {
            int amount = GameManager.Instance.AmountArray_Number[(int)content.cInstance.amountID];
            achievementDetailAmountInStatistics.text = string.Format(achievementDetailAmountInStatistics.text, amount);
        }
        string symbol = GameManager.Instance.SymbolArray[(int)content.cInstance.symbolID];
        achievementDetailSymbolInStatistics.text = string.Format(achievementDetailSymbolInStatistics.text, symbol);
        achievementDetailDigitInStatistics.text = string.Format(achievementDetailDigitInStatistics.text, (int)(content.cInstance.digitID + 2));
        achievementDetailOperandInStatistics.text = string.Format(achievementDetailOperandInStatistics.text, (int)(content.cInstance.operandID + 2));
        achievementDetailConditionInStatistics.text = string.Format(achievementDetailConditionInStatistics.text, content.accuracy, content.meanTime);
        achievementDetailShareBtnInStatistics.SetActive(true);
        CommonTool.AddEventTriggerListener(achievementDetailShareBtnInStatistics, EventTriggerType.PointerClick, OnShareBtn);
        detailWinDict["AchievementShareBtnsBgInStatistics"].SetActive(false);
    }
    protected void OnShareBtn(BaseEventData data)
    {
        if (SetUserName()) return;
        ShowShareBtn();
    }
    protected void ShowShareBtn()
    {
        Text achievementDetailTitleInStatistics = detailWinDict["AchievementDetailTitleInStatistics"].GetComponent<Text>();
        achievementDetailTitleInStatistics.gameObject.SetActive(true);
        achievementDetailTitleInStatistics.text = string.Format(achievementDetailTitleInStatistics.text, GameManager.Instance.UserName);
        detailWinDict["AchievementDetailShareBtnInStatistics"].SetActive(false);
        RectTransform achievementShareBtnsBgInStatistics = detailWinDict["AchievementShareBtnsBgInStatistics"].transform as RectTransform;
        achievementShareBtnsBgInStatistics.gameObject.SetActive(true);
        achievementShareBtnsBgInStatistics.DOMoveY(achievementShareBtnsBgInStatistics.rect.y, 0.3f, true).From();
    }
    protected bool SetUserName()
    {
        if (string.IsNullOrEmpty(GameManager.Instance.UserName))
        {
            GameObject achievementNameBoard = detailWinDict["AchievementNameBoard"];
            GameObject achievementInputFieldConfirmBtn = detailWinDict["AchievementInputFieldConfirmBtn"];
            InputField achievementInputField = detailWinDict["AchievementInputField"].GetComponent<InputField>();
            achievementNameBoard.SetActive(true);
            achievementInputField.onEndEdit.AddListener(OnEndEdit);
            CommonTool.AddEventTriggerListener(achievementInputFieldConfirmBtn, EventTriggerType.PointerClick, OnNameConfirmBtn);
        }
        return string.IsNullOrEmpty(GameManager.Instance.UserName);
    }

    protected void OnNameConfirmBtn(BaseEventData data)
    {
        if (string.IsNullOrEmpty(userName)) return;
        detailWinDict["AchievementNameBoard"].SetActive(false);
        detailWinDict["AchievementNameTipBoard"].SetActive(true);
        Text achievementNameTipBoardContent = detailWinDict["AchievementNameTipBoardContent"].GetComponent<Text>();
        string curName = GameManager.Instance.GetMutiLanguage(achievementNameTipBoardContent.index);
        achievementNameTipBoardContent.text = string.Format(curName, userName);
        CommonTool.AddEventTriggerListener(detailWinDict["AchievementNameTipBoardConfirmBtn"], EventTriggerType.PointerClick, OnTipConfirmBtn);
    }
    protected void OnTipConfirmBtn(BaseEventData data)
    {
        GameManager.Instance.UserName = userName;
        detailWinDict["AchievementNameTipBoard"].SetActive(false);
        detailWinDict["AchievementNameBoard"].SetActive(false);
        ShowShareBtn();
    }


    protected void OnEndEdit(string text)
    {
        userName = text;
    }

    protected bool FindWrong(QuentionInstance questionInstance)
    {
        int count = questionInstance.instance.Count;
        return questionInstance.instance[count - 1] != questionInstance.instance[count - 2];
    }
    protected void OnOnlyWrongBtn(BaseEventData data)
    {
        onlyWrong = !onlyWrong;
        RefreshSettlementGrid();
    }
    protected void RefreshSettlementGrid()
    {
        GameObject onlyWrongImageOfAchievement = CommonTool.GetGameObjectByName(detailWin, "OnlyWrongImageOfAchievement");
        onlyWrongImageOfAchievement.SetActive(onlyWrong);
        ArrayList dataList;
        if (onlyWrong)
        {
            dataList = new ArrayList(onlyWrongList);
        }
        else
        {
            dataList = new ArrayList(allInstanceList);
        }
        detailWin.GetComponentInChildren<InfiniteList>().InitList(dataList, "QuestionItem");
    }
    protected string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }
    protected void OnLongPress()
    {
        if (content == null || string.IsNullOrEmpty(content.finishTime)) return;
        deleteWin.SetActive(true);
        GameObject deleteConfirmBtnInAchievement = CommonTool.GetGameObjectByName(deleteWin, "DeleteConfirmBtnInAchievement");
        CommonTool.AddEventTriggerListener(deleteConfirmBtnInAchievement, EventTriggerType.PointerClick, OnLongPress);
    }
    protected void OnLongPress(BaseEventData data)
    {
        deleteWin.SetActive(false);
        GameManager.Instance.DeleteAchievement(content.achievementName);
    }
    protected override void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }
    protected override void InitDeleteWin(GameObject deleteWin)
    {
        this.deleteWin = deleteWin;
    }
    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementName                     = gameObjectDict["AchievementName"].GetComponent<Text>();
        achievementImage                    = gameObjectDict["AchievementImage"].GetComponent<Image>();
        achievementItem_WithoutAchievement  = gameObjectDict["AchievementItem_WithoutAchievement"];
    }
    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as AchievementInstance;
        if (content == null)
        {
            MyDebug.LogYellow("AchievementInstance is null!!");
            return;
        }
        bool notHasAchievement = string.IsNullOrEmpty(content.finishTime);
        achievementName.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementItem_WithoutAchievement.SetActive(notHasAchievement);
        if (!notHasAchievement) achievementImage.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        List<GameObject> stars = CommonTool.GetGameObjectsContainName(gameObject, "Star");
        for(int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive((i + 1) <= content.star);
        }
    }
}
[Serializable]
public class AchievementInstance
{
    public string achievementName;
    public float accuracy;
    public float meanTime;
    public string mainTitleIndex;
    public string subTitleIndex;
    public string imageIndex;
    public string chapterImageIndex;
    public string finishTime;//完成时间
    //public string classType;
    public CategoryInstance cInstance;
    public int star;
    public int difficulty;
}

