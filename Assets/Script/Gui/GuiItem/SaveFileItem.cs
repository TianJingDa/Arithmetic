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
    private Text saveFileIndex;


    private void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }

    private void InitDeleteWin(GameObject deleteWin)
    {
        this.deleteWin = deleteWin;
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        saveFileIndex = GameObjectDict["SaveFileIndex"].GetComponent<Text>();
    }


    private void InitPrefabItem(object data)
    {
        Init();
        content = data as SaveFileInstance;
        saveFileIndex.text = content.fileName;

    }

    private void OnShortPress()
    {
        detailWin.SetActive(true);
        Text saveFileDetailTime_Text = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailTime_Text");
        Text saveFileDetailAmount_Text = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailAmount_Text");
        Text saveFileDetailAccuracy_Text = CommonTool.GetComponentByName<Text>(detailWin, "SaveFileDetailAccuracy_Text");
        Button onlyWrongBtnInStatistics = CommonTool.GetComponentByName<Button>(detailWin, "OnlyWrongBtnInStatistics");
        Button curAchievementBtnInStatistics = CommonTool.GetComponentByName<Button>(detailWin, "CurAchievementBtnInStatistics");
        saveFileDetailTime_Text.text = saveFileDetailTime_Text.text.Replace("{0}", content.timeCost.ToString("f1"));
        saveFileDetailAmount_Text.text = saveFileDetailAmount_Text.text.Replace("{0}", content.qInstancList.Count.ToString());
        saveFileDetailAccuracy_Text.text = saveFileDetailAccuracy_Text.text.Replace("{0}", content.accuracy);
        onlyWrongList = content.qInstancList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
        onlyWrongBtnInStatistics.onClick.AddListener(OnOnlyWrongBtn);
    }

    private void OnOnlyWrongBtn()
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
        Button deleteConfirmBtn = CommonTool.GetComponentByName<Button>(deleteWin, "DeleteConfirmBtn");
        deleteConfirmBtn.onClick.AddListener(
            () => 
            {
                deleteWin.SetActive(false);
                GameManager.Instance.DeleteRecord(content.fileName);
            } );
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
        OnLongPress();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isLongPress = false;
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
}
[Serializable]
public class SaveFileInstance
{
    public float timeCost;
    public string fileName;
    public string accuracy;
    public List<QuentionInstance> qInstancList;
    public List<string> achievementKeys;//所获成就
}
