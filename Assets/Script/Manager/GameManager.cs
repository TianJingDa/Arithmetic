using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏控制层
/// </summary>
public class GameManager : MonoBehaviour
{
    private GameObject                                          m_Root;                             //UI对象的根对象
    private GameObject                                          m_CurrentWrapper;                   //当前激活的GuiWrapper
    private Clock                                               m_Clock = null;                     //时钟工具
    //private Dictionary<GuiFrameID, GameObject>                  m_GuiObjectDict;                    //用于在运行时存储UI对象

    private MutiLanguageController                              c_MutiLanguageCtrl;
    private ResourceController                                  c_ResourceCtrl;
    private StatisticsController                                c_StatisticsCtrl;
    private ExamController                                      c_ExamCtrl;
    private AchievementController                               c_AchievementCtrl;
    private SkinController                                      c_SkinCtrl;
    private LayoutController                                    c_LayoutCtrl;
    private FontController                                      c_FontCtrl;


    //private Dictionary<ControllerID, Controller> controllerDict;
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    private GuiFrameWrapper M_CurrentWrapper
    {
        get
        {
            return m_CurrentWrapper.GetComponent<GuiFrameWrapper>();
        }
    }
    public GuiFrameID M_CurExamID { get; set; }                                                     //当前答题界面（横竖）
    public static GameManager Instance//单例
    {
        get;
        private set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        m_Root = GameObject.Find("Canvas");
        m_CurrentWrapper = GameObject.Find("StartFrame");
        M_CurExamID = GuiFrameID.ExamFrame_V;
        //ActiveGui(GuiFrameID.StartFrame);
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00000"));
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00001"));
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00002"));
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00003"));
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00004"));
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00005"));
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00006"));
        Debug.Log(c_MutiLanguageCtrl.GetMutiLanguage("TJD_00007"));

    }

    void Update()
    {
        if (m_Clock != null)
        {
            m_Clock.Update();
        }
        //if (M_CurrentWrapper.id == GuiFrameID.ExamFrame_V)
        //{
        //    ((ExamFrameWrapper)M_CurrentWrapper).UpdateWrapper();
        //}
    }

    #region 公共方法
    /// <summary>
    /// 修改语言
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(Language language)
    {
        if (IsActive(GuiFrameID.SetUpFrame))
        {
            c_MutiLanguageCtrl.Language = language;
        }
    }
    public string GetMutiLanguage(string index)
    {
        return c_MutiLanguageCtrl.GetMutiLanguage(index);
    }
    /// <summary>
    /// 注册时钟
    /// </summary>
    /// <param name="clock"></param>
    public void RegisterClock(Clock clock)
    {
        this.m_Clock = clock;
    }
    /// <summary>
    /// 注销时钟
    /// </summary>
    public void UnRegisterClock()
    {
        this.m_Clock = null;
    }
    ///// <summary>
    ///// 激活GUI
    ///// </summary>
    ///// <param name="id"></param>
    //public void ActiveGui(GuiFrameID id)
    //{
    //    if (M_CurrentWrapper.id != id)
    //    {
    //        Object reource = c_ResourceCtrl.GetResource(id);
    //        if (reource == null)
    //        {
    //            Debug.Log("Can not load reousce:" + id.ToString());
    //            return;
    //        }
    //        m_CurrentWrapper = Instantiate(reource, m_Root.transform) as GameObject;
    //    }
    //    else
    //    {
    //        Debug.Log("Active Again!!");
    //    }
    //}
    ///// <summary>
    ///// 销毁GUI
    ///// </summary>
    ///// <param name="id"></param>
    //public void DeActiveGui(GuiFrameID id)
    //{
    //    if (M_CurrentWrapper.id == id)
    //    {
    //        GameObject tempWrapper = m_CurrentWrapper;
    //        m_CurrentWrapper = null;
    //        Destroy(tempWrapper);
    //    }
    //}
    /// <summary>
    /// GuiWrapper切换
    /// </summary>
    /// <param name="from_ID"></param>
    /// <param name="to_ID"></param>
    public void SwitchWrapper(GuiFrameID from_ID,GuiFrameID to_ID)
    {
        if (from_ID != to_ID && M_CurrentWrapper.id == from_ID)
        {
            GameObject tempWrapper = m_CurrentWrapper;
            m_CurrentWrapper = null;
            Destroy(tempWrapper);
            Object reource = c_ResourceCtrl.GetResource(to_ID);
            if (reource == null)
            {
                Debug.Log("Can not load reousce:" + to_ID.ToString());
                return;
            }
            m_CurrentWrapper = Instantiate(reource, m_Root.transform) as GameObject;
        }
        else
        {
            Debug.Log("Can not switch " + from_ID.ToString() + " to " + to_ID.ToString() + " !!");
        }
    }
    #endregion

    #region 私有方法
    private void RegisterController(Controller ctrl)
    {
        switch (ctrl.id)
        {
            case ControllerID.AchievementController:
                c_AchievementCtrl = (AchievementController)ctrl;
                break;
            case ControllerID.ExamController:
                c_ExamCtrl = (ExamController)ctrl;
                break;
            case ControllerID.MutiLanguageController:
                c_MutiLanguageCtrl = (MutiLanguageController)ctrl;
                break;
            case ControllerID.ResourceController:
                c_ResourceCtrl = (ResourceController)ctrl;
                break;
            case ControllerID.StatisticsController:
                c_StatisticsCtrl = (StatisticsController)ctrl;
                break;
            case ControllerID.SkinController:
                c_SkinCtrl = (SkinController)ctrl;
                break;
            case ControllerID.LayoutController:
                c_LayoutCtrl = (LayoutController)ctrl;
                break;
            case ControllerID.FontController:
                c_FontCtrl = (FontController)ctrl;
                break;
            default:
                Debug.Log("Unknow Controller:"+ctrl.id.ToString());
                break;
        }
        Debug.Log("Ctrl.id:" + ctrl.id.ToString());
    }
    private bool IsActive(GuiFrameID id)
    {
        return M_CurrentWrapper.id == id;
    }


    #endregion

}
