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
        achievementName.text = content.achievementName;

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        detailWin.SetActive(true);
        Text achievementDetaiPage_Text = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetaiPage_Text");
    }
}
[Serializable]
public class AchievementInstance
{
    public string achievementName;
    public int type;
    public float accuracy;
    public float meanTime;
    public string mainTitleIndex;
    public string subTitleIndex;
    public string imageIndex;
    public string fileName;//存档名
}

