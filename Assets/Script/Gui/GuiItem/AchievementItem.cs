using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using System;


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
    protected Text achievementName_WithoutAchievement;
    //protected Text achievementType;
    //protected Text achievementCondition;
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
        Image achievementDetailImageInStatistics = detailWinDict["AchievementDetailImageInStatistics"].GetComponent<Image>();
        Text achievementDetailMainTitleInStatistics = detailWinDict["AchievementDetailMainTitleInStatistics"].GetComponent<Text>();
        Text achievementDetailSubTitleInStatistics = detailWinDict["AchievementDetailSubTitleInStatistics"].GetComponent<Text>();
        Text achievementDetailFinishTimeInStatistics = detailWinDict["AchievementDetailFinishTimeInStatistics"].GetComponent<Text>();
        GameObject achievementDetailShareBtnInStatistics = detailWinDict["AchievementDetailShareBtnInStatistics"];
        //GameObject achievementDetailSaveFileBtnInStatistics = detailWinDict["AchievementDetailSaveFileBtnInStatistics"];
        //achievementDetailImageInStatistics.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        achievementDetailMainTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementDetailSubTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.subTitleIndex);
        achievementDetailFinishTimeInStatistics.text = GetFinishTime(content.finishTime);
        CommonTool.AddEventTriggerListener(achievementDetailShareBtnInStatistics, EventTriggerType.PointerClick, OnShareBtn);
        //if (!achievementDetailSaveFileBtnInStatistics.activeSelf) achievementDetailSaveFileBtnInStatistics.SetActive(true);
        //CommonTool.AddEventTriggerListener(achievementDetailSaveFileBtnInStatistics, EventTriggerType.PointerClick, OnSaveFileBtn);
    }
    protected void OnShareBtn(BaseEventData data)
    {
        if (SetUserName()) return;
        GameObject achievementShareWinInStatistics = detailWinDict["AchievementShareWinInStatistics"];
        achievementShareWinInStatistics.SetActive(true);
        Text achievementShareTitleInStatistics = detailWinDict["AchievementShareTitleInStatistics"].GetComponent<Text>();
        Image achievementShareImageInStatistics = detailWinDict["AchievementShareImageInStatistics"].GetComponent<Image>();
        Text achievementShareMainTitleInStatistics = detailWinDict["AchievementShareMainTitleInStatistics"].GetComponent<Text>();
        Text achievementShareSubTitleInStatistics = detailWinDict["AchievementShareSubTitleInStatistics"].GetComponent<Text>();
        Text achievementShareTypeInStatistics = detailWinDict["AchievementShareTypeInStatistics"].GetComponent<Text>();
        Text achievementShareFinishTimeInStatistics = detailWinDict["AchievementShareFinishTimeInStatistics"].GetComponent<Text>();
        Text achievementShareConditionInStatistics = detailWinDict["AchievementShareConditionInStatistics"].GetComponent<Text>();
        GameObject achievementSharePatternInStatistics_Time = detailWinDict["AchievementSharePatternInStatistics_Time"];
        GameObject achievementSharePatternInStatistics_Number = detailWinDict["AchievementSharePatternInStatistics_Number"];
        //Text achievementShareAmountInStatistics = detailWinDict["AchievementShareAmountInStatistics"].GetComponent<Text>();
        //Text achievementShareTimeInStatistics = detailWinDict["AchievementShareTimeInStatistics"].GetComponent<Text>();
        //Text achievementShareSymbolInStatistics = detailWinDict["AchievementShareSymbolInStatistics"].GetComponent<Text>();
        //Text achievementShareDigitInStatistics = detailWinDict["AchievementShareDigitInStatistics"].GetComponent<Text>();
        //Text achievementShareOperandInStatistics = detailWinDict["AchievementShareOperandInStatistics"].GetComponent<Text>();
        //Text achievementShareAccuracyInStatistics = detailWinDict["AchievementShareAccuracyInStatistics"].GetComponent<Text>();
        //Text achievementShareMeanTimeInStatistics = detailWinDict["AchievementShareMeanTimeInStatistics"].GetComponent<Text>()
        achievementShareTitleInStatistics.text = string.Format(achievementShareTitleInStatistics.text, GameManager.Instance.UserName);
        achievementShareImageInStatistics.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        achievementShareMainTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementShareSubTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.subTitleIndex);
        //achievementShareTypeInStatistics.text = GameManager.Instance.GetMutiLanguage(content.classType);
        achievementShareFinishTimeInStatistics.text = GetFinishTime(content.finishTime);
        achievementShareConditionInStatistics.text = GameManager.Instance.GetMutiLanguage(content.condition);
        achievementSharePatternInStatistics_Time.SetActive(content.cInstance.patternID == PatternID.Time);
        achievementSharePatternInStatistics_Number.SetActive(content.cInstance.patternID == PatternID.Number);
        //SaveFileInstance saveFile = GameManager.Instance.ReadRecord(content.fileName);
        //achievementShareAmountInStatistics.text = string.Format(achievementShareAmountInStatistics.text, saveFile.qInstancList.Count);
        //achievementShareTimeInStatistics.text = string.Format(achievementShareTimeInStatistics.text, saveFile.timeCost.ToString("f1"));
        //achievementShareSymbolInStatistics.text = string.Format(achievementShareSymbolInStatistics.text, GameManager.Instance.SymbolArray[(int)content.cInstance.symbolID]);
        //achievementShareDigitInStatistics.text = string.Format(achievementShareDigitInStatistics.text, (int)(content.cInstance.digitID + 2));
        //achievementShareOperandInStatistics.text = string.Format(achievementShareOperandInStatistics.text, (int)(content.cInstance.operandID + 2));
        //achievementShareAccuracyInStatistics.text = string.Format(achievementShareAccuracyInStatistics.text, content.accuracy);
        //string meanTime = (saveFile.timeCost / saveFile.qInstancList.Count).ToString("f2");
        //achievementShareMeanTimeInStatistics.text = string.Format(achievementShareMeanTimeInStatistics.text, meanTime);
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
            CommonTool.AddEventTriggerListener(achievementInputFieldConfirmBtn, EventTriggerType.PointerClick, OnConfirmBtn);
        }
        return string.IsNullOrEmpty(GameManager.Instance.UserName);
    }

    protected void OnConfirmBtn(BaseEventData data)
    {
        GameManager.Instance.UserName = userName;
        detailWinDict["AchievementNameBoard"].SetActive(false);
    }

    protected void OnEndEdit(string text)
    {
        userName = text;
    }

    //protected void OnSaveFileBtn(BaseEventData data)
    //{
    //    //GameObject saveFileDetailBgOfAchievement = detailWinDict["SaveFileDetailBgOfAchievement"];
    //    //saveFileDetailBgOfAchievement.SetActive(true);
    //    //Text saveFileDetailTimeOfAchievement = detailWinDict["SaveFileDetailTimeOfAchievement"].GetComponent<Text>();
    //    //Text saveFileDetailAmountOfAchievement = detailWinDict["SaveFileDetailAmountOfAchievement"].GetComponent<Text>();
    //    //Text saveFileDetailAccuracyOfAchievement = detailWinDict["SaveFileDetailAccuracyOfAchievement"].GetComponent<Text>();
    //    //GameObject onlyWrongBtnOfAchievement = detailWinDict["OnlyWrongBtnOfAchievement"];
    //    //GameObject saveFileDetaiTitle_Time = detailWinDict["SaveFileDetaiTitle_Time"];
    //    //GameObject saveFileDetaiTitle_Number = detailWinDict["SaveFileDetaiTitle_Number"];
    //    //SaveFileInstance saveFile = GameManager.Instance.ReadRecord(content.fileName);
    //    //onlyWrongList = saveFile.qInstancList.FindAll(FindWrong);
    //    //allInstanceList = saveFile.qInstancList;
    //    //saveFileDetaiTitle_Time.SetActive(saveFile.cInstance.patternID == PatternID.Time);
    //    //saveFileDetaiTitle_Number.SetActive(saveFile.cInstance.patternID == PatternID.Number);
    //    //saveFileDetailTimeOfAchievement.text = string.Format(saveFileDetailTimeOfAchievement.text, saveFile.timeCost.ToString("f1"));
    //    //saveFileDetailAmountOfAchievement.text = string.Format(saveFileDetailAmountOfAchievement.text, saveFile.qInstancList.Count);
    //    //saveFileDetailAccuracyOfAchievement.text = string.Format(saveFileDetailAccuracyOfAchievement.text, saveFile.accuracy);
    //    //CommonTool.AddEventTriggerListener(onlyWrongBtnOfAchievement, EventTriggerType.PointerClick, OnOnlyWrongBtn);
    //    //onlyWrong = false;
    //    //RefreshSettlementGrid();
    //    //CommonTool.GuiVerticalMove(saveFileDetailBgOfAchievement, Screen.height, MoveID.LeftOrDown, GameManager.Instance.CurCanvasGroup, true);
    //}
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
        //achievementType                     = gameObjectDict["AchievementType"].GetComponent<Text>();
        //achievementCondition                = gameObjectDict["AchievementCondition"].GetComponent<Text>();
        achievementName_WithoutAchievement  = gameObjectDict["AchievementName_WithoutAchievement"].GetComponent<Text>();
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
        achievementName.gameObject.SetActive(!notHasAchievement);
        achievementName_WithoutAchievement.gameObject.SetActive(notHasAchievement);
        achievementItem_WithoutAchievement.SetActive(notHasAchievement);
        string condition = GameManager.Instance.GetMutiLanguage(content.condition);
        //achievementCondition.text = string.Format(condition, content.accuracy, content.meanTime);
        //achievementType.text = GameManager.Instance.GetMutiLanguage(content.classType);
        if (notHasAchievement)
        {
            achievementName_WithoutAchievement.color = Color.gray;
            //achievementType.color = Color.gray;
            //achievementCondition.color = Color.gray;
        }
        else
        {
            achievementName.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        }
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
    public string condition;
    public float accuracy;
    public float meanTime;
    public string mainTitleIndex;
    public string subTitleIndex;
    public string imageIndex;
    public string finishTime;//完成时间
    //public string classType;
    public CategoryInstance cInstance;
    public int star;
    public int difficulty;
}

