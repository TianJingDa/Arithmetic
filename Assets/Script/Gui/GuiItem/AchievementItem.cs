using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class AchievementItem : Item
{
    private AchievementInstance content;//详情
    private GameObject detailWin;
    private Image image;//Item的Image！！

    private void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }

    private void InitPrefabItem(object data)
    {
        content = data as AchievementInstance;
        Text achievementName = CommonTool.GetComponentByName<Text>(gameObject, "AchievementName");
        achievementName.text = content.achievementKey;

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        detailWin.SetActive(true);
        Text achievementDetaiPage_Text = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetaiPage_Text");
        achievementDetaiPage_Text.text = content.detail;
    }
}
[Serializable]
public class AchievementInstance
{
    public string achievementKey;
    public string detail;
    public string fileName;//存档名
}

