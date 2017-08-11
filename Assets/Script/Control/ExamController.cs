using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ExamController : Controller
{
    #region 单例
    private static ExamController Instance
    {
        get;
        set;
    }
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            InitController();
            SendMessage("RegisterController", Instance, SendMessageOptions.RequireReceiver);
        }
    }
    #endregion
    protected override void InitController()
    {
        base.id = ControllerID.ExamController;
        InitExamData();
    }
    private void InitExamData()
    {

    }
    public ExamQuestionInstance GetQuestionInstance()
    {
        return new ExamQuestionInstance();
    }

}
public struct ExamQuestionInstance
{

}

