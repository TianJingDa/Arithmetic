
public enum LanguageID
{
    Chinese,
    English
}
public enum GuiFrameID
{
    None,                      //空界面，用于从界面的返回，如起名界面等
    CategoryFrame,             //类别选择界面
    FightFrame,                //答题界面
    StartFrame,                //开始界面
    StatisticsFrame,           //统计数据界面
    SetUpFrame,                //设置界面
    SettlementFrame,           //结算界面
    ChapterFrame,              //关卡界面
    BluetoothFrame,            //蓝牙界面
    NameBoardFrame,            //起名界面
    CommonTipFrame,            //提示信息界面
    BluetoothFightFrame,       //蓝牙答题界面
    SaveFileFrame,             //存档详情界面
    ShareFrame,                //分享界面
}
public enum GuiItemID
{
    AchievementItem,           //成就
    SaveFileItem,              //存档
    QuestionItem,              //问题
    PeripheralItem,            //外部设备
    BluetoothQuestionItem      //蓝牙问题
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
    Any = -1,                  //任意
    Time,                      //限时
    Number                     //限数
}
public enum AmountID
{
    Any = -1,                  //任意
    ClassA,                    //1分钟或30道
    ClassB,                    //3分钟或50道
    ClassC                     //5分钟或100道
}
public enum SymbolID
{
    //Hidden          = -2,      //这项并非是某种法则，而是用于成就系统中初始化数据
    //Summary         = -2,      //这项并非是某种法则，而是用于成就系统中初始化数据
    Any             = -1,      //任意
    Addition,                  //加法
    Subtraction,               //减法
    Multiplication,            //乘法
    Division,                  //除法
    Random                     //随机
}
public enum DigitID
{
    Any = -1,                  //任意
    TwoDigits,                 //两位数
    ThreeDigits,               //三位数
    FourDigits,                //四位数
    FiveDigits                 //五位数
}
public enum OperandID
{
    Any = -1,                  //任意
    TwoNumbers,                //两个操作数
    ThreeNumbers               //三个操作数
}
public enum MoveID
{
    LeftOrDown      = -1,      //从左侧或者下方播放动画
    RightOrUp       = 1        //从右侧或者上方播放动画
}
public enum DifficultyID
{
    Junior,                    //初级
    Medium,                    //中级
    Senior,                    //高级
    Ultimate                   //终极
}
public enum CommonTipID
{
    Single,                    //单独确认键
    Double,                    //确认加取消
    Splash                     //无按键，自动消失
}
public enum ShareID
{
    Achievement,               //成就
    SaveFile,                  //存档
    Bluetooth                  //蓝牙
}
