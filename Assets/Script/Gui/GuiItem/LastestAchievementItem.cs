﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastestAchievementItem : AchievementItem 
{
    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementName = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementName");
        achievementTpye = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementTpye");
        achievementCondition = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementCondition");
        achievementName_WithoutAchievement = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementName_WithoutAchievement");
        achievementItem_WithoutAchievement = CommonTool.GetGameObjectContainsName(gameObject, "AchievementItem_WithoutAchievement");
    }
    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as AchievementInstance;
        bool hasLastestAchievement = !string.IsNullOrEmpty(content.achievementName);
        achievementName.gameObject.SetActive(hasLastestAchievement);
        achievementName_WithoutAchievement.gameObject.SetActive(!hasLastestAchievement);
        achievementItem_WithoutAchievement.SetActive(!hasLastestAchievement);
        achievementCondition.gameObject.SetActive(hasLastestAchievement);
        if (hasLastestAchievement)
        {
            string condition = GameManager.Instance.GetMutiLanguage(content.condition);
            achievementCondition.text = string.Format(condition, content.accuracy, content.meanTime);
            achievementName.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        }
    }
}