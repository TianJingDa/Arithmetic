using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏控制层
/// </summary>
public class GameManager : MonoBehaviour
{
    private GameObject                                          m_Root;                             //UI对象的根对象
    private Clock                                               m_Clock = null;                     //时钟工具
    private Dictionary<GuiFrameID, GameObject>                  m_GuiObjectDict;                    //用于在运行时存储UI对象

    private MutiLanguageController                              c_MutiLanguageCtrl;
    private ResourceController                                  c_ResourceCtrl;
    private StatisticsController                                c_StatisticsCtrl;
    private ExamController                                      c_ExamCtrl;
    private AchievementController                               c_AchievementCtrl;


    //private Dictionary<ControllerID, Controller> controllerDict;
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
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
        //ActiveGui(GuiFrameID.StartFrame);
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00000"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00001"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00002"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00003"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00004"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00005"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00006"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00007"));

    }

    void Update()
    {
        if (m_Clock != null)
        {
            m_Clock.Update();
        }
    }

    #region 公共方法
    /// <summary>
    /// 修改语言
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(Language language)
    {
        if (IsRegister(GuiFrameID.SetUpFrame))
        {
            c_MutiLanguageCtrl.Language = language;
        }
    }
    /// <summary>
    /// 注册时钟
    /// </summary>
    /// <param name="clock"></param>
    public void RegisterClock(Clock clock)
    {
        this.m_Clock = null;
        System.GC.Collect();
        this.m_Clock = clock;
    }
    /// <summary>
    /// 注销时钟
    /// </summary>
    public void UnRegisterClock()
    {
        this.m_Clock = null;
        System.GC.Collect();
    }
    /// <summary>
    /// 激活GUI
    /// </summary>
    /// <param name="id"></param>
    public void ActiveGui(GuiFrameID id)
    {
        Object reource = c_ResourceCtrl.GetResource(id);
        if (reource == null)
        {
            Debug.Log("Can not load reousce:" + id.ToString());
            return;
        }
        GameObject wrapper = Instantiate(reource, m_Root.transform) as GameObject;
        RegisterGuiObject(id, wrapper);
    }
    /// <summary>
    /// 销毁GUI
    /// </summary>
    /// <param name="id"></param>
    public void DeActiveGui(GuiFrameID id)
    {
        GameObject guiObject = GetGuiObject(id);
        UnRegisterGuiObject(id);
        Destroy(guiObject);
        System.GC.Collect();
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
            default:
                Debug.Log("Unknow Controller!!");
                break;
        }
    }
    private void RegisterGuiObject(GuiFrameID id, GameObject wrapper)
    {
        if (!m_GuiObjectDict.ContainsKey(id))
        {
            m_GuiObjectDict.Add(id, wrapper);
        }
        else
        {
            Debug.Log("重复注册：" + id.ToString());
        }
    }
    private void UnRegisterGuiObject(GuiFrameID id)
    {
        if (m_GuiObjectDict.ContainsKey(id))
        {
            m_GuiObjectDict.Remove(id);
        }
        else
        {
            Debug.Log("重复注销：" + id.ToString());
        }
    }

    private GameObject GetGuiObject(GuiFrameID id)
    {
        GameObject wrapper = null;
        m_GuiObjectDict.TryGetValue(id, out wrapper);
        return wrapper;
    }
    private bool IsRegister(GuiFrameID id)
    {
        return m_GuiObjectDict.ContainsKey(id);
    }


    #endregion

}
