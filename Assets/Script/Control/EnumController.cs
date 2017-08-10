using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    Chinese,
    English
}
public enum GuiFrameID
{
    StartFrame,                //开始界面
    StatisticsFrame,           //统计数据界面
    CategoryFrame,             //速算类别选择界面
    SetUpFrame,                //设置界面
    AnswerFrame,               //答题界面
    SettlementFrame            //结算界面
}
public enum ControllerID
{
    MutiLanguageCtrl,    //多语言控制器
    ResourceCtrl,        //资源控制器
    GuiObjectCtrl,       //GuiObject控制器
    StatisticsCtrl,      //统计数据控制器
    ExamCtrl,            //考试控制器
    AchievementCtrl      //成就控制器
}
