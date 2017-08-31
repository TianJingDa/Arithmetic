using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏控制层
/// </summary>
public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public  LanguageID                                          m_CurLanguageID ;                   //当前语言
    [HideInInspector]
    public SkinID                                               m_CurSkinID;                        //当前皮肤
    [HideInInspector]
    public LayoutID                                             m_CurLayoutID;                      //当前布局

    private GameObject                                          m_Root;                             //UI对象的根对象
    private GameObject                                          m_CurWrapper;                       //当前激活的GuiWrapper
    private Clock                                               m_Clock;                            //时钟工具
    private MutiLanguageController                              c_MutiLanguageCtrl;
    private ResourceController                                  c_ResourceCtrl;
    private FightController                                      c_ExamCtrl;
    private AchievementController                               c_AchievementCtrl;
    private SkinController                                      c_SkinCtrl;
    private LayoutController                                    c_LayoutCtrl;
    private FontController                                      c_FontCtrl;
    private TextColorController                                 c_TextColorCtrl;

    //private Dictionary<GuiFrameID, GameObject>                  m_GuiObjectDict;                    //用于在运行时存储UI对象
    //private Dictionary<ControllerID, Controller> controllerDict;
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    private GuiFrameWrapper M_CurrentWrapper
    {
        get
        {
            return m_CurWrapper.GetComponent<GuiFrameWrapper>();
        }
    }
    public static GameManager Instance//单例
    {
        get;
        private set;
    }
    void Awake()
    {
        if (Instance == null){ Instance = this; }
        c_AchievementCtrl = AchievementController.Instance;
        c_ExamCtrl = FightController.Instance;
        c_FontCtrl = FontController.Instance;
        c_LayoutCtrl = LayoutController.Instance;
        c_MutiLanguageCtrl = MutiLanguageController.Instance;
        c_ResourceCtrl = ResourceController.Instance;
        c_SkinCtrl = SkinController.Instance;
        c_TextColorCtrl = TextColorController.Instance;
        m_CurLanguageID = LanguageID.Chinese;//后期需要进行判断PlayerPrefs，不然每次进来都是同一语言
        m_CurSkinID = SkinID.Default;//后期需要进行判断PlayerPrefs，不然每次进来都是同一皮肤
        m_CurLayoutID = LayoutID.Vertical;//后期需要进行判断PlayerPrefs，不然每次进来都是同一布局
    }

    void Start()
    {
        m_Root = GameObject.Find("Canvas");
        m_CurWrapper = GameObject.Find("StartFrame");
        //ActiveGui(GuiFrameID.StartFrame);
        Debug.Log(GetMutiLanguage("Text_00000"));
        Debug.Log(GetMutiLanguage("Text_00001"));
        Debug.Log(GetMutiLanguage("Text_00002"));
        Debug.Log(GetMutiLanguage("Text_00003"));
        Debug.Log(GetMutiLanguage("Text_00004"));
        Debug.Log(GetMutiLanguage("Text_00005"));
        Debug.Log(GetMutiLanguage("Text_00006"));
        Debug.Log(GetMutiLanguage("Text_00007"));

    }

    void Update()
    {
        if (m_Clock != null)
        {
            m_Clock.Update();
        }
        //if (M_CurrentWrapper.id == GuiFrameID.ExamFrame_V)
        //{
        //    ((FightFrameWrapper)M_CurrentWrapper).UpdateWrapper();
        //}

    }

    #region 公共方法
    /// <summary>
    /// 修改语言
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(LanguageID language)
    {
        if (IsActive(GuiFrameID.SetUpFrame))
        {
            m_CurLanguageID = language;
        }
    }
    public string GetMutiLanguage(string index)
    {
        return c_MutiLanguageCtrl.GetMutiLanguage(index, m_CurLanguageID);
    }
    public Font GetFont()
    {
        Object font = c_FontCtrl.GetFontResource(m_CurSkinID,m_CurLanguageID);
        return Instantiate(font) as Font;
    }
    public Sprite GetSprite(string index)
    {
        Object sprite = c_SkinCtrl.GetSpriteResource(m_CurSkinID, index);
        return Instantiate(sprite) as Sprite;
    }
    public Color GetColor(string index)
    {
        return c_TextColorCtrl.GetColorData(m_CurSkinID, index);
    }
    public RectTransform[] GetLayoutData()
    {
        return c_LayoutCtrl.GetLayoutData(m_CurLayoutID);
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
    //        Object reource = c_ResourceCtrl.GetGuiResource(id);
    //        if (reource == null)
    //        {
    //            Debug.Log("Can not load reousce:" + id.ToString());
    //            return;
    //        }
    //        m_CurWrapper = Instantiate(reource, m_Root.transform) as GameObject;
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
    //        GameObject tempWrapper = m_CurWrapper;
    //        m_CurWrapper = null;
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
            GameObject tempWrapper = m_CurWrapper;
            m_CurWrapper = null;
            Destroy(tempWrapper);
            Object reource = c_ResourceCtrl.GetGuiResource(to_ID);
            if (reource == null)
            {
                Debug.LogError("Can not load reousce:" + to_ID.ToString());
                return;
            }
            m_CurWrapper = Instantiate(reource, m_Root.transform) as GameObject;
        }
        else
        {
            Debug.LogError("Can not switch " + from_ID.ToString() + " to " + to_ID.ToString() + " !!");
        }
    }
    /// <summary>
    /// 获取PrefabItem
    /// </summary>
    /// <param name="name">Item名字</param>
    /// <returns></returns>
    public GameObject GetPrefabItem(string name)
    {
        Object resource = c_ResourceCtrl.GetItemResource(name);
        return Instantiate(resource) as GameObject;
        //return c_ResourceCtrl.GetItemResource(name); ;
    }
    #endregion

    #region 私有方法
    private bool IsActive(GuiFrameID id)
    {
        return M_CurrentWrapper.id == id;
    }


    #endregion

}
