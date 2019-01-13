﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LastestAchievementItem : AchievementItem 
{
    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementName = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementName");
        achievementImage = CommonTool.GetComponentContainsName<Image>(gameObject, "AchievementImage");
        achievementItem_WithoutAchievement = CommonTool.GetGameObjectContainsName(gameObject, "AchievementItem_WithoutAchievement");
    }
    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as AchievementInstance;
        bool hasLastestAchievement = !string.IsNullOrEmpty(content.achievementName);
        achievementName.gameObject.SetActive(hasLastestAchievement);
        achievementItem_WithoutAchievement.SetActive(!hasLastestAchievement);
        GameObject lastestAchievementName_WithoutAchievement = CommonTool.GetGameObjectByName(gameObject, "LastestAchievementName_WithoutAchievement");
        lastestAchievementName_WithoutAchievement.SetActive(!hasLastestAchievement);
        if (hasLastestAchievement)
        {
            achievementName.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
            achievementImage.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        }
    }

    public new void OnPointerDown(PointerEventData eventData) { }
    public new void OnPointerExit(PointerEventData eventData) { }
}
