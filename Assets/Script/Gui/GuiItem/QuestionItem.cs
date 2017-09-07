using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class QuestionItem : MonoBehaviour 
{
    private QuentionInstance content;//详情
    private Image image;//Item的Image！！


    private void InitPrefabItem(object data)
    {
        content = data as QuentionInstance;
        Text questionIndex = CommonTool.GetComponentByName<Text>(gameObject, "QuestionIndex");
        questionIndex.text = content.title;
    }

}
[Serializable]
public class QuentionInstance
{
    public string title;
}
