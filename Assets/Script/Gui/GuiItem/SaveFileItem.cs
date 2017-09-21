using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SaveFileItem : Item
{
    private SaveFileInstance content;//详情
    private GameObject detailWin;
    private Image image;//Item的Image！！


    private void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }

    private void InitPrefabItem(object data)
    {
        content = data as SaveFileInstance;
        Text saveFileIndex = CommonTool.GetComponentByName<Text>(gameObject, "SaveFileIndex");
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
