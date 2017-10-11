using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏控制层
/// </summary>
public class GameManager : MonoBehaviour
{
    private MutiLanguageController                              c_MutiLanguageCtrl;
    private ResourceController                                  c_ResourceCtrl;
    private FightController                                     c_FightCtrl;
    private AchievementController                               c_AchievementCtrl;
    private SkinController                                      c_SkinCtrl;
    private LayoutController                                    c_LayoutCtrl;
    private FontController                                      c_FontCtrl;
    private TextColorController                                 c_TextColorCtrl;
    private RecordController                                    c_RecordCtrl;

    private int[]                                               m_AmountArray;
    private string[]                                            m_SymbolArray;
    private SaveFileInstance                                    m_SaveFileInstance;
    private GameObject                                          m_Root;                             //UI对象的根对象
    private GameObject                                          m_CurWrapper;                       //当前激活的GuiWrapper
    private CategoryInstance                                    m_CurCategoryInstance;              //当前试题选项
    private System.Action                                       m_CurAction;                        //用于刷新SaveFile和Achievement列表

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

    public LanguageID CurLanguageID
    {
        get
        {
            int languageID = PlayerPrefs.GetInt("LanguageID", -1);
            if (languageID == -1)
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.ChineseSimplified:
                        languageID = 0;
                        break;
                    case SystemLanguage.English:
                    default:
                        languageID = 1;
                        break;
                }
            }
            return (LanguageID)languageID;
        }
        set
        {
            int languageID = (int)value;
            PlayerPrefs.SetInt("LanguageID", languageID);
        }
    }
    public SkinID CurSkinID
    {
        get
        {
            int skinID = PlayerPrefs.GetInt("SkinID", 0);
            return (SkinID)skinID;
        }
        set
        {
            int skinID = (int)value;
            PlayerPrefs.SetInt("SkinID", skinID);
        }
    }
    public LayoutID CurLayoutID
    {
        get
        {
            int layoutID = PlayerPrefs.GetInt("LayoutID", 0);
            return (LayoutID)layoutID;
        }
        set
        {
            int layout = (int)value;
            PlayerPrefs.SetInt("LayoutID", layout);
        }
    }
    public HandednessID CurHandednessID
    {
        get
        {
            int handednessID = PlayerPrefs.GetInt("HandednessID", 0);
            return (HandednessID)handednessID;
        }
        set
        {
            int handednessID = (int)value;
            PlayerPrefs.SetInt("HandednessID", handednessID);
        }
    }
    public CategoryInstance CurCategoryInstance
    {
        set
        {
            m_CurCategoryInstance = value;
            MyDebug.LogGreen(m_CurCategoryInstance.patternID);
            MyDebug.LogGreen(m_CurCategoryInstance.amountID);
            MyDebug.LogGreen(m_CurCategoryInstance.symbolID);
            MyDebug.LogGreen(m_CurCategoryInstance.digitID);
            MyDebug.LogGreen(m_CurCategoryInstance.operandID);
        }
    }
    public SaveFileInstance CurSaveFileInstance
    {
        get { return m_SaveFileInstance; }
    }
    public System.Action CurAction
    {
        set { m_CurAction = value; }
    }
    public float TotalTime
    {
        get
        {
            float totalTime = PlayerPrefs.GetFloat("TotalTime", 0);
            return totalTime;
        }
        set
        {
            float totalTime = value;
            PlayerPrefs.SetFloat("TotalTime", totalTime);
        }
    }
    public int TotalGame
    {
        get
        {
            int totalGame = PlayerPrefs.GetInt("TotalGame", 0);
            return totalGame;
        }
        set
        {
            int totalGame = value;
            PlayerPrefs.SetInt("TotalGame", totalGame);
        }
    }
    public bool NewAchievement
    {
        get
        {
            int newAchievement = PlayerPrefs.GetInt("NewAchievement", 0);
            return newAchievement != 0;
        }
        set
        {
            int newAchievement = value ? 1 : 0;
            PlayerPrefs.SetInt("NewAchievement", newAchievement);
        }
    }
    public bool NewSaveFile
    {
        get
        {
            int newSaveFile = PlayerPrefs.GetInt("NewSaveFile", 0);
            return newSaveFile != 0;
        }
        set
        {
            int newSaveFile = value ? 1 : 0;
            PlayerPrefs.SetInt("NewSaveFile", newSaveFile);
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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;

        c_AchievementCtrl       = AchievementController.Instance;
        c_FightCtrl             = FightController.Instance;
        c_FontCtrl              = FontController.Instance;
        c_LayoutCtrl            = LayoutController.Instance;
        c_MutiLanguageCtrl      = MutiLanguageController.Instance;
        c_ResourceCtrl          = ResourceController.Instance;
        c_SkinCtrl              = SkinController.Instance;
        c_TextColorCtrl         = TextColorController.Instance;
        c_RecordCtrl            = RecordController.Instance;
    }

    void Start()
    {
        m_Root = GameObject.Find("UIRoot");
        m_CurWrapper = Instantiate(c_ResourceCtrl.GetGuiResource(GuiFrameID.StartFrame), m_Root.transform) as GameObject;
        m_AmountArray = new int[] { 3, 5, 10 };
        m_SymbolArray = new string[] { "＋", "－", "×", "÷" };
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

    #region 公共方法
    public string GetMutiLanguage(string index)
    {
        return c_MutiLanguageCtrl.GetMutiLanguage(index, CurLanguageID);
    }
    public Font GetFont()
    {
        Object font = c_FontCtrl.GetFontResource(CurSkinID,CurLanguageID);
        return Instantiate(font) as Font;
    }
    public Sprite GetSprite(string index)
    {
        Object sprite = c_SkinCtrl.GetSpriteResource(CurSkinID, index);
        return Instantiate(sprite) as Sprite;
    }
    public Color GetColor(string index)
    {
        return c_TextColorCtrl.GetColorData(CurSkinID, index);
    }
    public Dictionary<string, MyRectTransform> GetLayoutData()
    {
        return c_LayoutCtrl.GetLayoutData(CurLayoutID, CurHandednessID);
    }
    public void ResetList()
    {
        c_FightCtrl.ResetList(m_CurCategoryInstance.symbolID, m_CurCategoryInstance.digitID, m_CurCategoryInstance.operandID);
    }
    public List<int> GetQuestionInstance()
    {
        return c_FightCtrl.GetQuestionInstance(m_CurCategoryInstance.symbolID, m_CurCategoryInstance.digitID, m_CurCategoryInstance.operandID);
    }
    public void GetFightParameter(out string pattern, out float amount, out string symbol)
    {
        pattern = m_CurCategoryInstance.patternID.ToString();
        switch (m_CurCategoryInstance.patternID)
        {
            case PatternID.Time:
                amount = m_AmountArray[(int)m_CurCategoryInstance.amountID] * 60;
                break;
            case PatternID.Number:
                amount = m_AmountArray[(int)m_CurCategoryInstance.amountID] * 10;
                break;
            default:
                amount = -1;
                MyDebug.LogYellow("Can not get AMOUNT!");
                break;
        }
        symbol = m_SymbolArray[(int)m_CurCategoryInstance.symbolID];
    }
    public void SaveRecord(List<List<int>> resultList, string symbol,float timeCost)
    {
        string fileName = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        float accuracy = CalculateAccuracy(resultList);
        List<QuentionInstance> qInstanceList = ConvertToInstanceList(resultList, symbol);
        string achievementKey = CheckAchievement(timeCost, resultList.Count, accuracy);

        SaveFileInstance curSaveFileInstance = new SaveFileInstance();
        curSaveFileInstance.timeCost = timeCost;
        curSaveFileInstance.fileName = fileName;
        curSaveFileInstance.accuracy = accuracy.ToString("f1");
        curSaveFileInstance.qInstancList = qInstanceList;
        curSaveFileInstance.achievementKey = achievementKey;
        CurSaveFileInstance.cInstance = m_CurCategoryInstance;

        m_SaveFileInstance = curSaveFileInstance;
        c_RecordCtrl.SaveRecord(curSaveFileInstance, fileName);
    }
    
    public List<SaveFileInstance> ReadRecord()
    {
        return c_RecordCtrl.ReadAllRecord();
    }
    public void ResetSaveFile()
    {
        c_RecordCtrl.DeleteAllRecord();
    }
    public void DeleteRecord(string fileName)
    {
        if (c_RecordCtrl.DeleteRecord(fileName))
        {
            if (m_CurAction != null) m_CurAction();
            else MyDebug.LogYellow("UnRegister Function!");
        }
        else
        {
            MyDebug.LogYellow("Delete File Fail!");
        }
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
                MyDebug.LogYellow("Can not load reousce:" + to_ID.ToString());
                return;
            }
            m_CurWrapper = Instantiate(reource, m_Root.transform) as GameObject;
        }
        else
        {
            MyDebug.LogYellow("Can not switch " + from_ID.ToString() + " to " + to_ID.ToString() + " !!");
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
    private float CalculateAccuracy(List<List<int>> resultList)
    {
        List<List<int>> rightList = resultList.FindAll(x => x[x.Count - 1] == x[x.Count - 2]);
        float accuracy = (float)rightList.Count * 100 / resultList.Count;
        return accuracy;
    }
    private List<QuentionInstance> ConvertToInstanceList(List<List<int>> resultList, string symbol)
    {
        List<QuentionInstance> qInstanceList = new List<QuentionInstance>();
        string count = resultList.Count.ToString();
        for (int i = 0; i < resultList.Count; i++)
        {
            QuentionInstance questionInstance = new QuentionInstance();
            questionInstance.index = (i + 1).ToString().PadLeft(count.Length, '0');
            questionInstance.symbol = symbol;
            questionInstance.instance = resultList[i];
            qInstanceList.Add(questionInstance);
        }
        return qInstanceList;
    }
    private string CheckAchievement(float timeCost, int instanceCount, float accuracy)
    {
        string achievementKey = "";
        return achievementKey;
    }
    #endregion

}
