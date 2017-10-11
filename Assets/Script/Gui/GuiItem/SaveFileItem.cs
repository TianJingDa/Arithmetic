using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


public class SaveFileItem : Item, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    private float durationThreshold = 1.0f;
    private bool isLongPress;
    private bool onlyWrong;

    private SaveFileInstance content;//详情
    private List<QuentionInstance> onlyWrongList;
    private GameObject detailWin;
    private GameObject deleteWin;
    private GameObject saveFileAchievement_No;
    private Text saveFileName;
    private Text saveFileType;
    private Vector3 position;

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
    private IEnumerator TimeCounter()
    {
        float duration = 0;
        while (duration < durationThreshold)
        {
            duration += Time.deltaTime;
            yield return null;
        }
        isLongPress = true;
        Vector3 curPosition = ((RectTransform)transform).position;
        float distance = Mathf.Abs(position.y - curPosition.y);
        if (distance <= 2) OnLongPress();
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        saveFileName = GameObjectDict["SaveFileName"].GetComponent<Text>();
        saveFileType = GameObjectDict["SaveFileType"].GetComponent<Text>();
        saveFileAchievement_No = GameObjectDict["SaveFileAchievement_No"];
    }
    private void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }
    private void InitDeleteWin(GameObject deleteWin)
    {
        this.deleteWin = deleteWin;
    }
    private void InitPrefabItem(object data)
    {
        Init();
        content = data as SaveFileInstance;
        saveFileName.text = content.fileName;
        saveFileAchievement_No.SetActive(string.IsNullOrEmpty(content.achievementKey));
    }
    private void OnShortPress()
    {
        detailWin.SetActive(true);
        Text saveFileDetailTime_Text = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailTime_Text");
        Text saveFileDetailAmount_Text = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailAmount_Text");
        Text saveFileDetailAccuracy_Text = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailAccuracy_Text");
        GameObject onlyWrongBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "OnlyWrongBtnInStatistics");
        GameObject curAchievementBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "CurAchievementBtnInStatistics");
        saveFileDetailTime_Text.text = string.Format(saveFileDetailTime_Text.text, content.timeCost.ToString("f1"));
        saveFileDetailAmount_Text.text = string.Format(saveFileDetailAmount_Text.text, content.qInstancList.Count);
        saveFileDetailAccuracy_Text.text = string.Format(saveFileDetailAccuracy_Text.text, content.accuracy);
        onlyWrongList = content.qInstancList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
        CommonTool.AddEventTriggerListener(onlyWrongBtnInStatistics, EventTriggerType.PointerClick, OnOnlyWrongBtn);
        CommonTool.AddEventTriggerListener(curAchievementBtnInStatistics, EventTriggerType.PointerClick, OnAchievementBtn);
    }

    private void OnAchievementBtn(BaseEventData data)
    {

    }
    private void OnOnlyWrongBtn(BaseEventData data)
    {
        onlyWrong = !onlyWrong;
        RefreshSettlementGrid();
    }
    private void RefreshSettlementGrid()
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
    private bool FindWrong(QuentionInstance questionInstance)
    {
        int count = questionInstance.instance.Count;
        return questionInstance.instance[count - 1] != questionInstance.instance[count - 2];
    }

    private void OnLongPress()
    {
        deleteWin.SetActive(true);
        GameObject deleteConfirmBtn = CommonTool.GetGameObjectByName(deleteWin, "DeleteConfirmBtn");
        CommonTool.AddEventTriggerListener(deleteConfirmBtn, EventTriggerType.PointerClick, OnLongPress);
    }
    private void OnLongPress(BaseEventData data)
    {
        deleteWin.SetActive(false);
        GameManager.Instance.DeleteRecord(content.fileName);
    }

}
[Serializable]
public class SaveFileInstance
{
    public float timeCost;
    public string fileName;
    public string accuracy;
    public List<QuentionInstance> qInstancList;
    public string achievementKey;//所获成就
    public CategoryInstance cInstance;
}
