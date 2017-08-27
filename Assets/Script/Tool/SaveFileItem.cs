using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileItem : MonoBehaviour 
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

        #region 假数据
        ArrayList dataList = new ArrayList();
        for (int i = 0; i < 30; i++)
        {
            QuentionInstance instance = new QuentionInstance();
            instance.title = i.ToString();
            dataList.Add(instance);
        }
        #endregion

        detailWin.GetComponentInChildren<InfiniteList>().InitList(dataList, "QuestionItem");
    }

}
