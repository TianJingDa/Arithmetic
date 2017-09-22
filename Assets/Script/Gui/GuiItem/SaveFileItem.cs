using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SaveFileItem : Item
{
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
        saveFileIndex.text = content.title;

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        detailWin.SetActive(true);
        ArrayList dataList = new ArrayList(content.instance);
        detailWin.GetComponentInChildren<InfiniteList>().InitList(dataList, "QuestionItem");
    }

}
[Serializable]
public class SaveFileInstance//还需要添加成就相关字段
{
    public string title;
    public List<QuentionInstance> instance;
}
