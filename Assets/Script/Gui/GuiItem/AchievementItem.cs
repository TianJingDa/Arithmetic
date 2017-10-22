using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class AchievementItem : Item, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    private AchievementInstance content;//详情
    private GameObject detailWin;
    private GameObject achievementItem_WithoutAchievement;
    private Image image;//Item的Image！！
    private Text achievementName;
    private Text achievementName_WithoutAchievement;
    private Text achievementTpye;
    private Text achievementCondition;

    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnPointerExit(PointerEventData eventData)
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
    }

    protected override void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        achievementName                     = GameObjectDict["AchievementName"].GetComponent<Text>();
        achievementTpye                     = GameObjectDict["AchievementTpye"].GetComponent<Text>();
        achievementCondition                = GameObjectDict["AchievementConditiond"].GetComponent<Text>();
        achievementName_WithoutAchievement  = GameObjectDict["AchievementName_WithoutAchievement"].GetComponent<Text>();
        achievementItem_WithoutAchievement  = GameObjectDict["AchievementItem_WithoutAchievement"];
    }

    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as AchievementInstance;
        bool notHasAchievement = string.IsNullOrEmpty(content.fileName);
        achievementName.gameObject.SetActive(!notHasAchievement);
        achievementName_WithoutAchievement.gameObject.SetActive(notHasAchievement);
        achievementItem_WithoutAchievement.SetActive(notHasAchievement);
        achievementCondition.text = string.Format(achievementCondition.text, content.accuracy, content.meanTime);
        if (notHasAchievement)
        {

        }
        else
        {

        }
        achievementName.text = content.achievementName;

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
    public string type;
}

