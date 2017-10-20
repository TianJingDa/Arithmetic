using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


public class SaveFileItem : Item, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    protected float durationThreshold = 1.0f;
    protected bool isLongPress;
    protected bool onlyWrong;

    protected SaveFileInstance content;//详情
    protected List<QuentionInstance> onlyWrongList;
    protected GameObject detailWin;
    protected GameObject deleteWin;
    protected GameObject saveFileAchievement_No;
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

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        saveFileName = GameObjectDict["SaveFileName"].GetComponent<Text>();
        saveFileType_Time = GameObjectDict["SaveFileType_Time"].GetComponent<Text>();
        saveFileType_Number = GameObjectDict["SaveFileType_Number"].GetComponent<Text>();
        saveFileAchievement_No = GameObjectDict["SaveFileAchievement_No"];
    }
    protected void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }
    protected void InitDeleteWin(GameObject deleteWin)
    {
        this.deleteWin = deleteWin;
    }
    protected void InitPrefabItem(object data)
    {
        Init();
        content = data as SaveFileInstance;
        saveFileName.text = content.fileName;
        saveFileAchievement_No.SetActive(string.IsNullOrEmpty(content.achievementName));
        int digit = (int)content.cInstance.digitID + 2;
        int operand = (int)content.cInstance.operandID + 2;
        if (content.cInstance.patternID == PatternID.Time)
        {
            int pattern = GameManager.Instance.AmountArray_Time[(int)content.cInstance.amountID];
            saveFileType_Time.text = string.Format(saveFileType_Time.text, pattern, digit, operand);
            saveFileType_Number.gameObject.SetActive(false);
        }
        else
        {
            int pattern = GameManager.Instance.AmountArray_Number[(int)content.cInstance.amountID];
            saveFileType_Number.text = string.Format(saveFileType_Number.text, pattern, digit, operand);
            saveFileType_Time.gameObject.SetActive(false);
        }
    }
    protected void OnShortPress()
    {
        detailWin.SetActive(true);
        Text saveFileDetailTime = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailTime");
        Text saveFileDetailAmount = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailAmount");
        Text saveFileDetailAccuracy = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailAccuracy");
        GameObject shareBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "ShareBtnInStatistics");
        GameObject onlyWrongBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "OnlyWrongBtnInStatistics");
        GameObject curAchievementBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "CurAchievementBtnInStatistics");
        saveFileDetailTime.text = string.Format(saveFileDetailTime.text, content.timeCost.ToString("f1"));
        saveFileDetailAmount.text = string.Format(saveFileDetailAmount.text, content.qInstancList.Count);
        saveFileDetailAccuracy.text = string.Format(saveFileDetailAccuracy.text, content.accuracy);
        onlyWrongList = content.qInstancList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
        CommonTool.AddEventTriggerListener(shareBtnInStatistics, EventTriggerType.PointerClick, OnShareBtn);
        CommonTool.AddEventTriggerListener(onlyWrongBtnInStatistics, EventTriggerType.PointerClick, OnOnlyWrongBtn);
        CommonTool.AddEventTriggerListener(curAchievementBtnInStatistics, EventTriggerType.PointerClick, OnAchievementBtn);
    }
    protected void OnShareBtn(BaseEventData data)
    {

    }
    protected void OnAchievementBtn(BaseEventData data)
    {

    }
    protected void OnOnlyWrongBtn(BaseEventData data)
    {
        onlyWrong = !onlyWrong;
        RefreshSettlementGrid();
    }
    protected void RefreshSettlementGrid()
    {
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
        GameObject deleteConfirmBtn = CommonTool.GetGameObjectByName(deleteWin, "DeleteConfirmBtn");
        CommonTool.AddEventTriggerListener(deleteConfirmBtn, EventTriggerType.PointerClick, OnLongPress);
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
