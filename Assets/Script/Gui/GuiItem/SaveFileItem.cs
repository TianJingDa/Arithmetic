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

    private SaveFileInstance content;//详情
    private GameObject detailWin;
    private Text saveFileIndex;


    private void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
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
        ArrayList dataList = new ArrayList(content.qInstancList);
        detailWin.GetComponentInChildren<InfiniteList>().InitList(dataList, "QuestionItem");
    }
    private void OnLongPress()
    {
        MyDebug.LogGreen("OnLongPress!!!");
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
