using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileItem : MonoBehaviour 
{
    private void InitPrefabItem(object data)
    {
        SaveFileInstance content = data as SaveFileInstance;
        Text achievementName = CommonTool.GetComponentByName<Text>(gameObject, "SaveFileIndex");
        achievementName.text = content.title;
    }
}
