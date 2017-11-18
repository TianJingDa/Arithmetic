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

    protected AchievementInstance content;//详情
    protected List<QuentionInstance> onlyWrongList;
    protected List<QuentionInstance> allInstanceList;
    protected GameObject detailWin;
    protected GameObject deleteWin;
    protected GameObject achievementItem_WithoutAchievement;
    protected Text achievementName;
    protected Text achievementName_WithoutAchievement;
    protected Text achievementTpye;
    protected Text achievementCondition;
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
        if (content == null || string.IsNullOrEmpty(content.fileName)) return;
        detailWin.SetActive(true);
        Image achievementDetailImageInStatistics = CommonTool.GetComponentByName<Image>(detailWin, "AchievementDetailImageInStatistics");
        Text achievementDetailMainTitleInStatistics = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetailMainTitleInStatistics");
        Text achievementDetailSubTitleInStatistics = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetailSubTitleInStatistics");
        Text achievementDetailFinishTimeInStatistics = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetailFinishTimeInStatistics");
        GameObject achievementDetailShareBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "AchievementDetailShareBtnInStatistics");
        GameObject achievementDetailSaveFileBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "AchievementDetailSaveFileBtnInStatistics");
        achievementDetailImageInStatistics.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        achievementDetailMainTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementDetailSubTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.subTitleIndex);
        achievementDetailFinishTimeInStatistics.text = GetFinishTime(content.fileName);
        CommonTool.AddEventTriggerListener(achievementDetailShareBtnInStatistics, EventTriggerType.PointerClick, OnShareBtn);
        if (!achievementDetailSaveFileBtnInStatistics.activeSelf) achievementDetailSaveFileBtnInStatistics.SetActive(true);
        CommonTool.AddEventTriggerListener(achievementDetailSaveFileBtnInStatistics, EventTriggerType.PointerClick, OnSaveFileBtn);
    }
    protected void OnShareBtn(BaseEventData data)
    {
        GameObject achievementShareWinInStatistics = CommonTool.GetGameObjectByName(detailWin, "AchievementShareWinInStatistics");
        achievementShareWinInStatistics.SetActive(true);
        //初始化分享界面
    }

    protected void OnSaveFileBtn(BaseEventData data)
    {
        GameObject saveFileDetailBgOfAchievement = CommonTool.GetGameObjectByName(detailWin, "SaveFileDetailBgOfAchievement");
        saveFileDetailBgOfAchievement.SetActive(true);
        Text saveFileDetailImgOfAchievement_Text = CommonTool.GetComponentByName<Text>(saveFileDetailBgOfAchievement, "SaveFileDetailImgOfAchievement_Text");
        Text saveFileDetailTimeOfAchievement = CommonTool.GetComponentByName<Text>(saveFileDetailBgOfAchievement, "SaveFileDetailTimeOfAchievement");
        Text saveFileDetailAmountOfAchievement = CommonTool.GetComponentByName<Text>(saveFileDetailBgOfAchievement, "SaveFileDetailAmountOfAchievement");
        Text saveFileDetailAccuracyOfAchievement = CommonTool.GetComponentByName<Text>(saveFileDetailBgOfAchievement, "SaveFileDetailAccuracyOfAchievement");
        GameObject onlyWrongBtnOfAchievement = CommonTool.GetGameObjectByName(saveFileDetailBgOfAchievement, "OnlyWrongBtnOfAchievement");
        saveFileDetailImgOfAchievement_Text.text = content.fileName;
        SaveFileInstance saveFile = GameManager.Instance.ReadRecord(content.fileName);
        onlyWrongList = saveFile.qInstancList.FindAll(FindWrong);
        allInstanceList = saveFile.qInstancList;
        saveFileDetailTimeOfAchievement.text = string.Format(saveFileDetailTimeOfAchievement.text, saveFile.timeCost.ToString("f1"));
        saveFileDetailAmountOfAchievement.text = string.Format(saveFileDetailAmountOfAchievement.text, saveFile.qInstancList.Count);
        saveFileDetailAccuracyOfAchievement.text = string.Format(saveFileDetailAccuracyOfAchievement.text, saveFile.accuracy);
        CommonTool.AddEventTriggerListener(onlyWrongBtnOfAchievement, EventTriggerType.PointerClick, OnOnlyWrongBtn);
        onlyWrong = false;
        RefreshSettlementGrid();
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
        if (content == null || string.IsNullOrEmpty(content.fileName)) return;
        deleteWin.SetActive(true);
        GameObject deleteConfirmBtnInAchievement = CommonTool.GetGameObjectByName(deleteWin, "DeleteConfirmBtnInAchievement");
        CommonTool.AddEventTriggerListener(deleteConfirmBtnInAchievement, EventTriggerType.PointerClick, OnLongPress);
    }
    protected void OnLongPress(BaseEventData data)
    {
        deleteWin.SetActive(false);
        GameManager.Instance.DeleteAchievement(content.achievementName, content.fileName);
    }
    protected override void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }
    protected override void InitDeleteWin(GameObject deleteWin)
    {
        this.deleteWin = deleteWin;
    }
    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        achievementName                     = GameObjectDict["AchievementName"].GetComponent<Text>();
        achievementTpye                     = GameObjectDict["AchievementTpye"].GetComponent<Text>();
        achievementCondition                = GameObjectDict["AchievementCondition"].GetComponent<Text>();
        achievementName_WithoutAchievement  = GameObjectDict["AchievementName_WithoutAchievement"].GetComponent<Text>();
        achievementItem_WithoutAchievement  = GameObjectDict["AchievementItem_WithoutAchievement"];
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
        bool notHasAchievement = string.IsNullOrEmpty(content.fileName);
        achievementName.gameObject.SetActive(!notHasAchievement);
        achievementName_WithoutAchievement.gameObject.SetActive(notHasAchievement);
        achievementItem_WithoutAchievement.SetActive(notHasAchievement);
        string condition = GameManager.Instance.GetMutiLanguage(content.condition);
        achievementCondition.text = string.Format(condition, content.accuracy, content.meanTime);
        achievementTpye.text = GameManager.Instance.GetMutiLanguage(content.classType);
        if (notHasAchievement)
        {
            achievementName_WithoutAchievement.color = Color.gray;
            achievementTpye.color = Color.gray;
            achievementCondition.color = Color.gray;
        }
        else
        {
            achievementName.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
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
    public string fileName;//存档名
    public string classType;
    public CategoryInstance cInstance;
}

