using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour 
{

    private void InitPrefabItem(object data)
    {
        AchievementInstance content = data as AchievementInstance;
        Text achievementName = CommonTool.GetComponentByName<Text>(gameObject, "AchievementName");
        achievementName.text = content.title;
    }
}
