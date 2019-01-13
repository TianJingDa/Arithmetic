using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummarySaveFileItem : SaveFileItem 
{
    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        saveFileName = CommonTool.GetComponentContainsName<Text>(gameObject, "SaveFileName");
        saveFileType_Time = CommonTool.GetComponentContainsName<Text>(gameObject, "SaveFileType_Time");
        saveFileType_Number = CommonTool.GetComponentContainsName<Text>(gameObject, "SaveFileType_Number");
        saveFileAchiOrBLE = CommonTool.GetComponentContainsName<Image>(gameObject, "SaveFileAchiOrBLE");
    }
}
