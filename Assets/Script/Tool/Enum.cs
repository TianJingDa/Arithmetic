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
    FightController,           //答题控制器
    AchievementController,     //成就控制器
    SkinController,            //皮肤控制器
    LayoutController,          //布局控制器
    FontController,            //字体控制器
    TextColorController,       //字色控制器
    RecordController           //存档控制器
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
    Vertical,                  //垂直布局
    Horizontal                 //水平布局
}
public enum HandednessID
{
    Right,                     //右撇子
    Left,                      //左撇子
    Custom                     //自定义
}
public enum PatternID
{
    Time,                      //限时
    Number                     //限数
}
public enum AmountID
{
    Three,                     //3分钟或30道
    Five,                      //5分钟或50道
    Ten                        //10分钟或100道
}
public enum SymbolID
{
    Addition,                  //加法
    Subtraction,               //减法
    Multiplication,            //乘法
    Division,                  //除法
    Random                     //随机
}
public enum DigitID
{
    TwoDigits,                 //两位数
    ThreeDigits,               //三位数
    FourDigits,                //四位数
    FiveDigits                 //五位数
}
public enum OperandID
{
    TwoNumbers,                //两个操作数
    ThreeNumbers               //三个操作数
}

