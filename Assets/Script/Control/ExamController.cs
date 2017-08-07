using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ExamController 
{
    #region C#单例
    private static ExamController instance = null;
    private ExamController() { }
    public static ExamController Instance
    {
        get { return instance ?? (instance = new ExamController()); }
    }
    #endregion
    public QuestionInstance GetQuestionInstance()
    {
        return new QuestionInstance();
    }

}
public class QuestionInstance
{

}

