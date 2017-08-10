﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ExamCtrl : Controller
{
    #region 单例
    private static ExamCtrl Instance
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
            GameManager.Instance.RegisterController(base.id, Instance);
        }
    }
    #endregion
    protected override void InitController()
    {
        base.id = ControllerID.ExamCtrl;
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

