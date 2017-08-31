using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageID
{
    Chinese,
    English
}
public enum GuiFrameID
{
    CategoryFrame,             //速算类别选择界面
    FightFrame,                //答题界面
    PayFrame,                  //支付界面
    StartFrame,                //开始界面
    StatisticsFrame,           //统计数据界面
    SetUpFrame,                //设置界面
    SettlementFrame,           //结算界面
}
public enum ControllerID
{
    MutiLanguageController,    //多语言控制器
    ResourceController,        //资源控制器
    ExamController,            //考试控制器
    AchievementController,     //成就控制器
    SkinController,            //皮肤控制器
    LayoutController,          //布局控制器
    FontController             //字体控制器
}
public enum SkinID
{
    Default,                   //默认皮肤
    FreshGreen,                //清新绿
    RosePink,                  //玫瑰粉
    SkyBlue                    //天空蓝
}
public enum LayoutID
{
    Horizontal,                //水平布局
    Vertical                   //垂直布局
}
