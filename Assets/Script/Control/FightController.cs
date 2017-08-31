using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FightController : Controller
{
    #region C#单例
    private static FightController instance = null;
    private FightController()
    {
        base.id = ControllerID.ExamController;
        InitExamData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static FightController Instance
    {
        get { return instance ?? (instance = new FightController()); }
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

