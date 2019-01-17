using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class BluetoothQuestionItem : Item
{
    private QuentionInstance content;//详情
    private Image questionOwnAnswerPage;
    private Image questionOtherAnswerPage;
    private Text questionOwnAnswer_Text;
    private Text questionOtherAnswer_Text;
    private Text questionIndex;
    private Text questionContent;


    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        questionOwnAnswerPage           = gameObjectDict["QuestionOwnAnswerPage"].GetComponent<Image>();
        questionOtherAnswerPage         = gameObjectDict["QuestionOtherAnswerPage"].GetComponent<Image>();
        questionOwnAnswer_Text          = gameObjectDict["QuestionOwnAnswer_Text"].GetComponent<Text>();
        questionOtherAnswer_Text        = gameObjectDict["QuestionOtherAnswer_Text"].GetComponent<Text>();
        questionIndex                   = gameObjectDict["QuestionIndex"].GetComponent<Text>();
        questionContent                 = gameObjectDict["QuestionContent"].GetComponent<Text>();
    }

    protected override void InitPrefabItem(object data)
    {
        content = data as QuentionInstance;
        if (content == null)
        {
            MyDebug.LogYellow("BluetoothQuentionInstance is null!!");
            return;
        }
        Init();
        questionIndex.text = content.index + ".";
        int count = content.instance.Count;
        StringBuilder question = new StringBuilder();
        question.Append(content.instance[0].ToString());
        for (int i = 1; i < count - 3; i++)
        {
            question.Append(content.symbol);
            question.Append(content.instance[i].ToString());
        }
        questionContent.text = question.ToString();
        questionOwnAnswer_Text.text = content.instance[count - 1].ToString();
        questionOtherAnswer_Text.text = content.instance[count - 3].ToString();
        questionOwnAnswerPage.color = content.instance[count - 1] == content.instance[count - 2] ? Color.green : Color.red;
        questionOtherAnswerPage.color = content.instance[count - 3] == content.instance[count - 2] ? Color.green : Color.red;
    }
}
