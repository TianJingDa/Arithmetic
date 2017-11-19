using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Text;


public class SaveFileItem : Item, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    protected float durationThreshold = 1.0f;
    protected bool isLongPress;
    protected bool onlyWrong;
    protected bool hasAchievement;

    protected SaveFileInstance content;//详情
    protected List<QuentionInstance> onlyWrongList;
    protected GameObject detailWin;
    protected GameObject deleteWin;
    protected GameObject saveFileAchievement_No;
    protected Dictionary<string, GameObject> detailWinDict;
    protected Text saveFileName;
    protected Text saveFileType_Time;
    protected Text saveFileType_Number;
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

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        saveFileName = gameObjectDict["SaveFileName"].GetComponent<Text>();
        saveFileType_Time = gameObjectDict["SaveFileType_Time"].GetComponent<Text>();
        saveFileType_Number = gameObjectDict["SaveFileType_Number"].GetComponent<Text>();
        saveFileAchievement_No = gameObjectDict["SaveFileAchievement_No"];
    }
    protected override void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }
    protected override void InitDeleteWin(GameObject deleteWin)
    {
        this.deleteWin = deleteWin;
    }
    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as SaveFileInstance;
        if (content == null)
        {
            MyDebug.LogYellow("SaveFileInstance is null!!");
            return;
        }
        hasAchievement = !string.IsNullOrEmpty(content.achievementName);
        saveFileName.text = content.fileName;
        saveFileAchievement_No.SetActive(!hasAchievement);
        int digit = (int)content.cInstance.digitID + 2;
        int operand = (int)content.cInstance.operandID + 2;
        if (content.cInstance.patternID == PatternID.Time)
        {
            int amount = GameManager.Instance.AmountArray_Time[(int)content.cInstance.amountID];
            saveFileType_Time.text = string.Format(saveFileType_Time.text, amount / 60, digit, operand);
            saveFileType_Number.gameObject.SetActive(false);
        }
        else
        {
            int amount = GameManager.Instance.AmountArray_Number[(int)content.cInstance.amountID];
            saveFileType_Number.text = string.Format(saveFileType_Number.text, amount, digit, operand);
            saveFileType_Time.gameObject.SetActive(false);
        }
    }
    protected void OnShortPress()
    {
        detailWin.SetActive(true);
        detailWinDict = CommonTool.InitGameObjectDict(detailWin);
        CommonTool.InitText(detailWin);
        CommonTool.InitImage(detailWin);
        Text saveFileDetailTime = detailWinDict["SaveFileDetailTime"].GetComponent<Text>();
        Text saveFileDetailAmount = detailWinDict["SaveFileDetailAmount"].GetComponent<Text>();
        Text saveFileDetailAccuracy = detailWinDict["SaveFileDetailAccuracy"].GetComponent<Text>();
        GameObject shareBtnInSaveFile = detailWinDict["ShareBtnInSaveFile"];
        GameObject onlyWrongBtnInSaveFile = detailWinDict["OnlyWrongBtnInSaveFile"];
        GameObject curAchievementBtnInSaveFile = detailWinDict["CurAchievementBtnInSaveFile"];
        saveFileDetailTime.text = string.Format(saveFileDetailTime.text, content.timeCost.ToString("f1"));
        saveFileDetailAmount.text = string.Format(saveFileDetailAmount.text, content.qInstancList.Count);
        saveFileDetailAccuracy.text = string.Format(saveFileDetailAccuracy.text, content.accuracy);
        onlyWrongList = content.qInstancList.FindAll(FindWrong);
        CommonTool.AddEventTriggerListener(shareBtnInSaveFile, EventTriggerType.PointerClick, OnShareBtn);
        CommonTool.AddEventTriggerListener(onlyWrongBtnInSaveFile, EventTriggerType.PointerClick, OnOnlyWrongBtn);
        curAchievementBtnInSaveFile.SetActive(hasAchievement);
        if (hasAchievement) CommonTool.AddEventTriggerListener(curAchievementBtnInSaveFile, EventTriggerType.PointerClick, OnAchievementBtn);
        onlyWrong = false;
        RefreshSettlementGrid();
    }
    protected void OnShareBtn(BaseEventData data)
    {
        GameObject saveFileShareWinInStatistics = detailWinDict["SaveFileShareWinInStatistics"];
        saveFileShareWinInStatistics.SetActive(true);
        GameObject saveFileShareTitleInStatistics = detailWinDict["SaveFileShareTitleInStatistics"];
        GameObject saveFileSharePatternInStatistics_Time = detailWinDict["SaveFileSharePatternInStatistics_Time"];
        GameObject saveFileSharePatternInStatistics_Number = detailWinDict["SaveFileSharePatternInStatistics_Number"];
        GameObject saveFileShareAchievementInStatistics = detailWinDict["SaveFileShareAchievementInStatistics"];
        GameObject saveFileShareWithoutAchievementInStatistics = detailWinDict["SaveFileShareWithoutAchievementInStatistics"];
        Text saveFileShareAmountInStatistics = detailWinDict["SaveFileShareAmountInStatistics"].GetComponent<Text>();
        Text saveFileShareTimeInStatistics = detailWinDict["SaveFileShareTimeInStatistics"].GetComponent<Text>();
        Text saveFileShareSymbolInStatistics = detailWinDict["SaveFileShareSymbolInStatistics"].GetComponent<Text>();
        Text saveFileShareDigitInStatistics = detailWinDict["SaveFileShareDigitInStatistics"].GetComponent<Text>();
        Text saveFileShareOperandInStatistics = detailWinDict["SaveFileShareOperandInStatistics"].GetComponent<Text>();
        Text saveFileShareAccuracyInStatistics = detailWinDict["SaveFileShareAccuracyInStatistics"].GetComponent<Text>();
        Text saveFileShareMeanTimeInStatistics = detailWinDict["SaveFileShareMeanTimeInStatistics"].GetComponent<Text>();
        Image saveFileShareImageInStatistics = detailWinDict["SaveFileShareImageInStatistics"].GetComponent<Image>();
        Text saveFileShareMainTitleInStatistics = detailWinDict["SaveFileShareMainTitleInStatistics"].GetComponent<Text>();
        Text saveFileShareSubTitleInStatistics = detailWinDict["SaveFileShareSubTitleInStatistics"].GetComponent<Text>();
        Text saveFileShareTypeInStatistics = detailWinDict["SaveFileShareTypeInStatistics"].GetComponent<Text>();
        Text saveFileShareFinishTimeInStatistics = detailWinDict["SaveFileShareFinishTimeInStatistics"].GetComponent<Text>();
        Text saveFileShareConditionInStatistics = detailWinDict["SaveFileShareConditionInStatistics"].GetComponent<Text>();
        saveFileShareTitleInStatistics.GetComponent<Text>().enabled = false;
        saveFileSharePatternInStatistics_Time.SetActive(content.cInstance.patternID == PatternID.Time);
        saveFileSharePatternInStatistics_Number.SetActive(content.cInstance.patternID == PatternID.Number);
        saveFileShareAmountInStatistics.text = string.Format(saveFileShareAmountInStatistics.text, content.qInstancList.Count);
        saveFileShareTimeInStatistics.text = string.Format(saveFileShareTimeInStatistics.text, content.timeCost.ToString("f1"));
        saveFileShareSymbolInStatistics.text = string.Format(saveFileShareSymbolInStatistics.text, GameManager.Instance.SymbolArray[(int)content.cInstance.symbolID]);
        saveFileShareDigitInStatistics.text = string.Format(saveFileShareDigitInStatistics.text, (int)(content.cInstance.digitID + 2));
        saveFileShareOperandInStatistics.text = string.Format(saveFileShareOperandInStatistics.text, (int)(content.cInstance.operandID + 2));
        saveFileShareAccuracyInStatistics.text = string.Format(saveFileShareAccuracyInStatistics.text, content.accuracy);
        string meanTime = (content.timeCost / content.qInstancList.Count).ToString("f2");
        saveFileShareMeanTimeInStatistics.text = string.Format(saveFileShareMeanTimeInStatistics.text, meanTime);
        saveFileShareAchievementInStatistics.SetActive(!string.IsNullOrEmpty(content.achievementName));
        saveFileShareWithoutAchievementInStatistics.SetActive(string.IsNullOrEmpty(content.achievementName));
        if (!string.IsNullOrEmpty(content.achievementName))
        {
            AchievementInstance instance = GameManager.Instance.GetAchievement(content.achievementName);
            saveFileShareImageInStatistics.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
            saveFileShareMainTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
            saveFileShareSubTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
            saveFileShareTypeInStatistics.text = GameManager.Instance.GetMutiLanguage(instance.classType);
            saveFileShareFinishTimeInStatistics.text = GetFinishTime(instance.fileName);
            saveFileShareConditionInStatistics.text = GameManager.Instance.GetMutiLanguage(instance.condition);
        }
    }
    protected void OnAchievementBtn(BaseEventData data)
    {
        GameObject achievementDetailBgInSaveFile = detailWinDict["AchievementDetailBgInSaveFile"];
        achievementDetailBgInSaveFile.SetActive(true);
        Image achievementDetailImageInSaveFile = detailWinDict["AchievementDetailImageInSaveFile"].GetComponent<Image>();
        Text achievementDetailMainTitleInSaveFile = detailWinDict["AchievementDetailMainTitleInSaveFile"].GetComponent<Text>();
        Text achievementDetailSubTitleInSaveFile = detailWinDict["AchievementDetailSubTitleInSaveFile"].GetComponent<Text>();
        Text achievementDetailFinishTimeInSaveFile = detailWinDict["AchievementDetailFinishTimeInSaveFile"].GetComponent<Text>();
        AchievementInstance instance = GameManager.Instance.GetAchievement(content.achievementName);
        achievementDetailImageInSaveFile.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
        achievementDetailMainTitleInSaveFile.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
        achievementDetailSubTitleInSaveFile.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
        achievementDetailFinishTimeInSaveFile.text = GetFinishTime(instance.fileName);

    }
    protected string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }
    protected void OnOnlyWrongBtn(BaseEventData data)
    {
        onlyWrong = !onlyWrong;
        RefreshSettlementGrid();
    }
    protected void RefreshSettlementGrid()
    {
        GameObject onlyWrongImageInSaveFile = detailWinDict["OnlyWrongImageInSaveFile"];
        onlyWrongImageInSaveFile.SetActive(onlyWrong);
        ArrayList dataList;
        if (onlyWrong)
        {
            dataList = new ArrayList(onlyWrongList);
        }
        else
        {
            dataList = new ArrayList(content.qInstancList);
        }
        detailWin.GetComponentInChildren<InfiniteList>().InitList(dataList, "QuestionItem");
    }
    protected bool FindWrong(QuentionInstance questionInstance)
    {
        int count = questionInstance.instance.Count;
        return questionInstance.instance[count - 1] != questionInstance.instance[count - 2];
    }

    protected void OnLongPress()
    {
        deleteWin.SetActive(true);
        GameObject deleteConfirmBtnInSaveFile = CommonTool.GetGameObjectByName(deleteWin, "DeleteConfirmBtnInSaveFile");
        CommonTool.AddEventTriggerListener(deleteConfirmBtnInSaveFile, EventTriggerType.PointerClick, OnLongPress);
    }
    protected void OnLongPress(BaseEventData data)
    {
        deleteWin.SetActive(false);
        GameManager.Instance.DeleteRecord(content.fileName, content.achievementName);
    }

}
[Serializable]
public class SaveFileInstance
{
    public float timeCost;
    public string fileName;
    public string accuracy;
    public List<QuentionInstance> qInstancList;
    public string achievementName;//所获成就
    public CategoryInstance cInstance;
}
