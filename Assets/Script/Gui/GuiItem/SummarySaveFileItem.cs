using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummarySaveFileItem : SaveFileItem 
{
    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        saveFileName = CommonTool.GetComponentContainsName<Text>(gameObject, "SaveFileName");
        saveFileType_Time = CommonTool.GetComponentContainsName<Text>(gameObject, "SaveFileType_Time");
        saveFileType_Number = CommonTool.GetComponentContainsName<Text>(gameObject, "SaveFileType_Number");
        saveFileAchievement_No = CommonTool.GetGameObjectContainsName(gameObject, "SaveFileAchievement_No");
    }
}
