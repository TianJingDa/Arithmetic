using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryAchievementItem : AchievementItem 
{
    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        achievementName = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementName");
        achievementTpye = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementTpye");
        achievementCondition = CommonTool.GetComponentContainsName<Text>(gameObject, "AchievementCondition");
        achievementItem_WithoutAchievement = CommonTool.GetGameObjectContainsName(gameObject, "AchievementItem_WithoutAchievement");
    }
    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as AchievementInstance;
        bool notHasAchievement = string.IsNullOrEmpty(content.fileName);
        achievementItem_WithoutAchievement.SetActive(notHasAchievement);
        achievementCondition.text = content.condition;
        int countWithAchievement = 0;
        int countOfSymbol = GetAchievementCountBySymbol(content.cInstance.symbolID, out countWithAchievement);
        achievementTpye.text = string.Format(achievementTpye.text, countWithAchievement, countOfSymbol);
        achievementName.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        if (notHasAchievement)
        {
            achievementName.color = Color.gray;
            achievementTpye.color = Color.gray;
            achievementCondition.color = Color.gray;
        }
    }
    private int GetAchievementCountBySymbol(SymbolID symbol, out int countWithAchievement)
    {
        List<AchievementInstance> instanceList = GameManager.Instance.GetAllAchievements().FindAll(x => x.cInstance.symbolID == symbol);
        countWithAchievement = 0;
        for (int i = 0; i < instanceList.Count; i++)
        {
            if (string.IsNullOrEmpty(instanceList[i].fileName))
            {
                countWithAchievement++;
            }
        }
        return instanceList.Count;
    }
}
