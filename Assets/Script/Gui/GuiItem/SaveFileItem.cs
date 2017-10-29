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
        GameObject shareBtnInSaveFile = CommonTool.GetGameObjectByName(detailWin, "ShareBtnInSaveFile");
        GameObject onlyWrongBtnInSaveFile = CommonTool.GetGameObjectByName(detailWin, "OnlyWrongBtnInSaveFile");
        GameObject curAchievementBtnInSaveFile = CommonTool.GetGameObjectByName(detailWin, "CurAchievementBtnInSaveFile");
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

    }
    protected void OnAchievementBtn(BaseEventData data)
    {
        GameObject achievementDetailBgInSaveFile = CommonTool.GetGameObjectByName(detailWin, "AchievementDetailBgInSaveFile");
        achievementDetailBgInSaveFile.SetActive(true);
        Image achievementDetailImageInSaveFile = CommonTool.GetComponentByName<Image>(achievementDetailBgInSaveFile, "AchievementDetailImageInSaveFile");
        Text achievementDetailMainTitleInSaveFile = CommonTool.GetComponentByName<Text>(achievementDetailBgInSaveFile, "AchievementDetailMainTitleInSaveFile");
        Text achievementDetailSubTitleInSaveFile = CommonTool.GetComponentByName<Text>(achievementDetailBgInSaveFile, "AchievementDetailSubTitleInSaveFile");
        Text achievementDetailFinishTimeInSaveFile = CommonTool.GetComponentByName<Text>(achievementDetailBgInSaveFile, "AchievementDetailFinishTimeInSaveFile");
        AchievementInstance instance = GameManager.Instance.GetAchievement(content.achievementName);
        achievementDetailImageInSaveFile.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
        achievementDetailMainTitleInSaveFile.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
        achievementDetailSubTitleInSaveFile.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
        achievementDetailFinishTimeInSaveFile.text = GetFinishTime(content.fileName);

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
