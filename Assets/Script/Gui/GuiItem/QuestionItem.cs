using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;



public class QuestionItem : Item
{
    private QuentionInstance content;//详情
    private GameObject questionItem_Wrong;
    private GameObject questionRightAnswer;
    private Text questionIndex;
    private Text questionContent;
    private Text questionRightAnswer_Text;

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        questionItem_Wrong          = GameObjectDict["QuestionItem_Wrong"];
        questionRightAnswer         = GameObjectDict["QuestionRightAnswer"];
        questionIndex               = GameObjectDict["QuestionIndex"].GetComponent<Text>();
        questionContent             = GameObjectDict["QuestionContent"].GetComponent<Text>();
        questionRightAnswer_Text    = GameObjectDict["QuestionRightAnswer_Text"].GetComponent<Text>();
    }

    private void InitPrefabItem(object data)
    {
        content = data as QuentionInstance;
        Init();
        questionIndex.text = content.index;
        int count = content.instance.Count;
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
        if (content.instance[count - 2] != content.instance[count - 1])
        {
            questionItem_Wrong.SetActive(true);
            questionRightAnswer_Text.text = questionRightAnswer_Text.text + content.instance[count - 2].ToString();
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

    }
    private void OnClick()
    {
        questionRightAnswer.SetActive(!questionRightAnswer.activeSelf);
    }

}
[Serializable]
public class QuentionInstance
{
    public string index;
    public string symbol;
    public List<int> instance;
}
