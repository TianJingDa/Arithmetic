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
    ExamFrame_V,               //答题界面（竖版）
    ExamFrame_H,               //答题界面（横版）
    StartFrame,                //开始界面
    StatisticsFrame,           //统计数据界面
    CategoryFrame,             //速算类别选择界面
    SetUpFrame,                //设置界面
    SettlementFrame,           //结算界面
    PayFrame                   //支付界面
}
public enum ControllerID
{
    MutiLanguageController,    //多语言控制器
    ResourceController,        //资源控制器
    StatisticsController,      //统计数据控制器
    ExamController,            //考试控制器
    AchievementController,     //成就控制器
    SkinController,            //皮肤控制器
    LayoutController,          //布局控制器
    FontController             //字体控制器
}
public enum SkinID
{
    First,
    Second
}
public enum LayoutID
{
    Horizontal,                //水平布局
    Vertical                   //垂直布局
}
