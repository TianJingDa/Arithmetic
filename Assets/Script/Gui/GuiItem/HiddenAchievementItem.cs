using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HiddenAchievementItem : AchievementItem 
{
    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementName = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementName");
        //achievementType = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementType");
        achievementCondition = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementCondition");
        achievementName_WithoutAchievement = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementName_WithoutAchievement");
        achievementItem_WithoutAchievement = CommonTool.GetGameObjectContainsName(gameObject, "AchievementItem_WithoutAchievement");
    }
    protected new void OnShortPress()
    {
        if (content == null || string.IsNullOrEmpty(content.finishTime)) return;
        detailWin.SetActive(true);
        Image achievementDetailImageInStatistics = CommonTool.GetComponentByName<Image>(detailWin, "AchievementDetailImageInStatistics");
        Text achievementDetailMainTitleInStatistics = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetailMainTitleInStatistics");
        Text achievementDetailSubTitleInStatistics = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetailSubTitleInStatistics");
        Text achievementDetailFinishTimeInStatistics = CommonTool.GetComponentByName<Text>(detailWin, "AchievementDetailFinishTimeInStatistics");
        GameObject achievementDetailShareBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "AchievementDetailShareBtnInStatistics");
        //GameObject achievementDetailSaveFileBtnInStatistics = CommonTool.GetGameObjectByName(detailWin, "AchievementDetailSaveFileBtnInStatistics");
        //achievementDetailImageInStatistics.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        achievementDetailMainTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementDetailSubTitleInStatistics.text = GameManager.Instance.GetMutiLanguage(content.subTitleIndex);
        achievementDetailFinishTimeInStatistics.text = GetFinishTime(content.finishTime);
        //CommonTool.AddEventTriggerListener(achievementDetailShareBtnInStatistics, EventTriggerType.PointerClick, OnShareBtn);
        //if (achievementDetailSaveFileBtnInStatistics.activeSelf) achievementDetailSaveFileBtnInStatistics.SetActive(false);
    }

}
