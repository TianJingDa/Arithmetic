using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ExamController : Controller
{
    #region C#单例
    private static ExamController instance = null;
    private ExamController()
    {
        base.id = ControllerID.ExamController;
        InitExamData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static ExamController Instance
    {
        get { return instance ?? (instance = new ExamController()); }
    }
    #endregion

    private void InitExamData()
    {

    }
    public SaveFileInstance GetQuestionInstance()
    {
        return new SaveFileInstance();
    }

}

