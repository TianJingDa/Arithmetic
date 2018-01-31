﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;



public class QuestionItem : Item
{
    private int count;
    private QuentionInstance content;//详情
    private GameObject questionRightAnswerBg;
    private Text questionIndex;
    private Text questionContent;
    private Text questionRightAnswerPage_Text;

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        questionRightAnswerBg           = gameObjectDict["QuestionRightAnswerBg"];
        questionIndex                   = gameObjectDict["QuestionIndex"].GetComponent<Text>();
        questionContent                 = gameObjectDict["QuestionContent"].GetComponent<Text>();
        questionRightAnswerPage_Text    = gameObjectDict["QuestionRightAnswerPage_Text"].GetComponent<Text>();
    }

    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as QuentionInstance;
        if (content == null)
        {
            MyDebug.LogYellow("QuentionInstance is null!!");
            return;
        }
        questionIndex.text = content.index + ".";
        count = content.instance.Count;
        StringBuilder question = new StringBuilder();
        question.Append(content.instance[0].ToString());
        for(int i = 1; i < count - 2; i++)
        {
            question.Append(content.symbol);
            question.Append(content.instance[i].ToString());
        }
        question.Append("=");
        question.Append(content.instance[count - 1].ToString());
        questionContent.text = question.ToString();
        questionRightAnswerBg.SetActive(content.instance[count - 2] != content.instance[count - 1]);
        questionRightAnswerPage_Text.text = content.instance[count - 2].ToString();
    }
}
[Serializable]
public class QuentionInstance
{
    public string index;
    public string symbol;
    public List<int> instance;
}
