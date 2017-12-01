using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using cn.sharesdk.unity3d;
using DG.Tweening;

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

    private int[]                                               m_AmountArray_Time;
    private int[]                                               m_AmountArray_Number;
    private string[]                                            m_SymbolArray;
    private float                                               m_TweenDuration = 0.5f;             //Tween动画持续时间
    private SaveFileInstance                                    m_SaveFileInstance;
    private GameObject                                          m_Root;                             //UI对象的根对象
    private GameObject                                          m_CurWrapper;                       //当前激活的GuiWrapper
    private CategoryInstance                                    m_CurCategoryInstance;              //当前试题选项
    private System.Action                                       m_CurAction;                        //用于刷新SaveFile和Achievement列表
    private System.Action                                       m_ShareAction;                      //用于分享时初始化用户名称
    private ShareSDK                                            m_ShareSDK;                         //用于分享成就和成绩

    //private Dictionary<GuiFrameID, GameObject>                  m_GuiObjectDict;                    //用于在运行时存储UI对象
    //private Dictionary<ControllerID, Controller> controllerDict;
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/

    public LanguageID CurLanguageID
    {
        get
        {
            int languageID = PlayerPrefs.GetInt("LanguageID", 0);
            //if (languageID == -1)
            //{
            //    switch (Application.systemLanguage)
            //    {
            //        case SystemLanguage.ChineseSimplified:
            //            languageID = 0;
            //            break;
            //        case SystemLanguage.English:
            //        default:
            //            languageID = 1;
            //            break;
            //    }
            //}
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
        private set
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
        private set
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
        private set
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
        private set
        {
            int newSaveFile = value ? 1 : 0;
            PlayerPrefs.SetInt("NewSaveFile", newSaveFile);
        }
    }
    public int[] AmountArray_Time
    {
        get { return m_AmountArray_Time; }
    }
    public int[] AmountArray_Number
    {
        get { return m_AmountArray_Number; }
    }
    public bool FinishAllAchievement
    {
        get
        {
            AchievementInstance achievement = c_AchievementCtrl.GetAllAchievements().Find(x => x.cInstance.symbolID >= 0
                                                                                            && string.IsNullOrEmpty(x.finishTime));
            return achievement == null;
        }
    }
    public bool IsFromCategory
    {
        get;
        set;
    }
    public string UserName
    {
        get
        {
            return PlayerPrefs.GetString("UserName", null);
        }
        set
        {
            PlayerPrefs.SetString("UserName", value);
        }
    }
    public string CurAchievementName
    {
        get;
        private set;
    }
    public string[] SymbolArray
    {
        get
        {
            return m_SymbolArray;
        }
    }
    public CanvasGroup CurCanvasGroup
    {
        get
        {
            return m_CurWrapper.GetComponent<CanvasGroup>();
        }
    }
    private string LastestAchievement
    {
        get
        {
            string lastestAchievementString = PlayerPrefs.GetString("LastestAchievement", "");
            if (string.IsNullOrEmpty(lastestAchievementString)) return "";
            char[] charSeparators = new char[] { ',' };
            string[] lastestAchievementArray = lastestAchievementString.Split(charSeparators, System.StringSplitOptions.RemoveEmptyEntries);
            if (lastestAchievementArray.Length == 0) return null;
            return lastestAchievementArray[lastestAchievementArray.Length - 1];
        }
        set
        {
            string lastestAchievementString = PlayerPrefs.GetString("LastestAchievement", "") + value + ",";
            PlayerPrefs.SetString("LastestAchievement", lastestAchievementString);
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
        //PlayerPrefs.DeleteKey("UserName");
        m_Root = GameObject.Find("UIRoot");
        m_CurWrapper = Instantiate(c_ResourceCtrl.GetGuiResource(GuiFrameID.StartFrame), m_Root.transform) as GameObject;
        m_AmountArray_Time = new int[] { 60, 180, 300 };//这里不应该直接写在代码里，但应该写在哪里？
        m_AmountArray_Number = new int[] { 30, 50, 100 };
        m_SymbolArray = new string[] { "＋", "－", "×", "÷" };
        m_ShareSDK = GetComponent<ShareSDK>();
        m_ShareSDK.shareHandler = OnShareResultHandler;
        InitShareIcon();

#if UNITY_EDITOR
        gameObject.AddComponent<Camera>();
#endif

        //ActiveGui(GuiFrameID.StartFrame);
    }

    #region 公共方法
    public string GetMutiLanguage(string index)
    {
        return c_MutiLanguageCtrl.GetMutiLanguage(index, CurLanguageID);
    }
    public Font GetFont()
    {
        return c_FontCtrl.GetFontResource(CurSkinID, CurLanguageID);
    }
    public Sprite GetSprite(string index)
    {
        return c_SkinCtrl.GetSpriteResource(CurSkinID, index);
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
                amount = m_AmountArray_Time[(int)m_CurCategoryInstance.amountID];
                break;
            case PatternID.Number:
                amount = m_AmountArray_Number[(int)m_CurCategoryInstance.amountID];
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
        string finishTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        float accuracy = CalculateAccuracy(resultList);
        List<QuentionInstance> qInstanceList = ConvertToInstanceList(resultList, symbol);
        //float meanTime = timeCost / resultList.Count;
        //string achievementName = CheckAchievement(meanTime, accuracy, finishTime);

        SaveFileInstance curSaveFileInstance = new SaveFileInstance();
        curSaveFileInstance.timeCost = timeCost;
        curSaveFileInstance.fileName = finishTime;
        curSaveFileInstance.accuracy = accuracy.ToString("f1");
        curSaveFileInstance.qInstancList = qInstanceList;
        //curSaveFileInstance.achievementName = achievementName;
        curSaveFileInstance.cInstance = m_CurCategoryInstance;

        m_SaveFileInstance = curSaveFileInstance;
        c_RecordCtrl.SaveRecord(curSaveFileInstance, finishTime);

        TotalGame++;
        TotalTime += timeCost;
    }

    public void SaveAchievement(List<List<int>> resultList, string symbol, float timeCost)
    {
        string finishTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        float accuracy = CalculateAccuracy(resultList);
        float meanTime = timeCost / resultList.Count;
        CurAchievementName = CheckAchievement(meanTime, accuracy, finishTime);

        List<QuentionInstance> qInstanceList = ConvertToInstanceList(resultList, symbol);
        SaveFileInstance curSaveFileInstance = new SaveFileInstance();
        curSaveFileInstance.timeCost = timeCost;
        curSaveFileInstance.fileName = finishTime;
        curSaveFileInstance.accuracy = accuracy.ToString("f1");
        curSaveFileInstance.qInstancList = qInstanceList;
        //curSaveFileInstance.achievementName = achievementName;
        curSaveFileInstance.cInstance = m_CurCategoryInstance;

        m_SaveFileInstance = curSaveFileInstance;

        TotalGame++;
        TotalTime += timeCost;
    }

    public List<SaveFileInstance> ReadAllRecords()
    {
        return c_RecordCtrl.ReadAllRecords();
    }
    public SaveFileInstance ReadRecord(string fileName)
    {
        return c_RecordCtrl.ReadRecord(fileName);
    }
    public void ResetSaveFile()
    {
        c_RecordCtrl.DeleteAllRecord();
        //c_AchievementCtrl.ResetAllAchievement();
    }
    //public void ResetSaveFileWithoutAchievement()
    //{
    //    List<string> fileNameList = c_AchievementCtrl.GetAllFileNameWithAchievement();
    //    c_RecordCtrl.DeleteRecordWithoutAchievement(fileNameList);
    //}
    public void DeleteRecord(string fileName)
    {
        if (c_RecordCtrl.DeleteRecord(fileName))
        {
            if (m_CurAction != null) m_CurAction();
            else MyDebug.LogYellow("UnRegister Function!");
            //if (!string.IsNullOrEmpty(achievementName))
            //{
            //    PlayerPrefs.DeleteKey(achievementName);
            //}
        }
        else
        {
            MyDebug.LogYellow("Delete File Fail!");
        }
    }
    public void DeleteAchievement(string achievementName)
    {
        if (!PlayerPrefs.HasKey(achievementName))
        {
            MyDebug.LogYellow("Wrong AchievementName!");
            return;
        }
        c_AchievementCtrl.DeleteAchievement(achievementName);
        AchievementInstance hiddenAchievement = c_AchievementCtrl.GetAllAchievements().Find(x => x.cInstance.symbolID == SymbolID.Hidden);
        if (PlayerPrefs.HasKey(hiddenAchievement.achievementName))
        {
            c_AchievementCtrl.DeleteAchievement(hiddenAchievement.achievementName);
        }
        if (m_CurAction != null) m_CurAction();
        else MyDebug.LogYellow("UnRegister Function!");
        //if (c_RecordCtrl.DeleteRecord(fileName))
        //{
        //    if (m_CurAction != null) m_CurAction();
        //    else MyDebug.LogYellow("UnRegister Function!");
        //}
        //else
        //{
        //    MyDebug.LogYellow("Delete File Fail!");
        //}
        string lastestAchievement = PlayerPrefs.GetString("LastestAchievement", "");
        if (lastestAchievement.Contains(achievementName))
        {
            lastestAchievement = lastestAchievement.Replace(achievementName + ",", "");
            PlayerPrefs.SetString("LastestAchievement", lastestAchievement);
        }
    }
    public List<AchievementInstance> GetAllAchievements()
    {
        return c_AchievementCtrl.GetAllAchievements();
    }
    public AchievementInstance GetAchievement(string achievementName)
    {
        return c_AchievementCtrl.GetAchievement(achievementName);
    }
    public AchievementInstance GetLastestAchievement()
    {
        if (string.IsNullOrEmpty(LastestAchievement)) return new AchievementInstance();
        return c_AchievementCtrl.GetAchievement(LastestAchievement);
    }
    public void ResetAchievement()
    {
        //List<string> fileNameList = c_AchievementCtrl.GetAllFileNameWithAchievement();
        //c_RecordCtrl.DeleteRecordWithAchievement(fileNameList);
        c_AchievementCtrl.ResetAllAchievement();
    }

    //public void InitShareInfo(PlatformType type, System.Action action)
    //{
    //    m_ShareAction = action;
    //    m_ShareSDK.GetUserInfo(type);
    //}    

    public void ShareImage(Rect mRect, PlatformType type)
    {
        string directoryPath = Application.persistentDataPath + "/ScreenShot";
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        string fileName = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string filePath = directoryPath + "/" + fileName + ".png";
        StartCoroutine(CaptureScreenShotByRect(mRect, filePath, type));
    }

    public void ShareUrl(PlatformType type)
    {
        ShareContent content = new ShareContent();
        if(type == PlatformType.WeChatMoments || type == PlatformType.WeChat)
        {
            content.SetImagePath("");
            content.SetTitle("test title");//多语言
            content.SetUrl("");
            content.SetShareType(ContentType.Webpage);
        }
        else if(type == PlatformType.SinaWeibo)
        {
            //content.SetText(text);//text是Url
            //content.SetImagePath(filePath);
        }
        m_ShareSDK.ShareContent(type, content);
    }

    /// <summary>
    /// GuiWrapper切换，无动画
    /// </summary>
    /// <param name="targetID"></param>
    public void SwitchWrapper(GuiFrameID targetID)
    {
        Destroy(m_CurWrapper);
        Object reource = c_ResourceCtrl.GetGuiResource(targetID);
        if (reource == null)
        {
            MyDebug.LogYellow("Can not load reousce:" + targetID.ToString());
            return;
        }
        m_CurWrapper = Instantiate(reource, m_Root.transform) as GameObject;
    }
    /// <summary>
    /// GuiWrapper切换，有移动动画
    /// </summary>
    /// <param name="targetID"></param>
    /// <param name="mID"></param>
    /// <param name="isIn"></param>
    public void SwitchWrapper(GuiFrameID targetID, MoveID mID, bool isIn)
    {
        m_Root.GetComponent<GraphicRaycaster>().enabled = false;
        Object reource = c_ResourceCtrl.GetGuiResource(targetID);
        if (reource == null)
        {
            MyDebug.LogYellow("Can not load reousce: " + targetID.ToString());
            return;
        }
        GameObject targetWrapper = Instantiate(reource, m_Root.transform) as GameObject;
        if (isIn)
        {
            targetWrapper.transform.DOLocalMoveX(Screen.width * (int)mID, m_TweenDuration, true).
                                    From().
                                    SetEase(Ease.OutQuint).
                                    OnComplete(() => TweenComplete(targetWrapper));
        }
        else
        {
            targetWrapper.transform.SetAsFirstSibling();
            m_CurWrapper.transform.DOLocalMoveX(Screen.width * (int)mID, m_TweenDuration, true).
                                   SetEase(Ease.OutQuint).
                                   OnComplete(() => TweenComplete(targetWrapper));
        }
    }
    /// <summary>
    /// GuiWrapper切换，有缩放动画
    /// </summary>
    /// <param name="targetID"></param>
    /// <param name="isIn"></param>
    public void SwitchWrapper(GuiFrameID targetID, bool isIn)
    {
        m_Root.GetComponent<GraphicRaycaster>().enabled = false;
        Object reource = c_ResourceCtrl.GetGuiResource(targetID);
        if (reource == null)
        {
            MyDebug.LogYellow("Can not load reousce: " + targetID.ToString());
            return;
        }
        GameObject targetWrapper = Instantiate(reource, m_Root.transform) as GameObject;
        if (isIn)
        {
            targetWrapper.transform.DOScale(Vector3.zero, m_TweenDuration).
                                    From().
                                    SetEase(Ease.OutQuint).
                                    OnComplete(() => TweenComplete(targetWrapper));
        }
        else
        {
            targetWrapper.transform.SetAsFirstSibling();
            m_CurWrapper.transform.DOScale(Vector3.zero, m_TweenDuration).
                                   SetEase(Ease.OutQuint).
                                   OnComplete(() => TweenComplete(targetWrapper));
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
    private string CheckAchievement(float meanTime, float accuracy, string finishTime)
    {
        string achievementName = "";
        List<AchievementInstance> achievementList = c_AchievementCtrl.GetAchievementUnFinish();
        for(int i = 0; i < achievementList.Count; i++)
        {
            if(achievementList[i].cInstance.Equals(m_CurCategoryInstance)
            && achievementList[i].accuracy <= accuracy
            && achievementList[i].meanTime >= meanTime)
            {
                achievementName = achievementList[i].achievementName;
                break;
            }
        }
        if (!string.IsNullOrEmpty(achievementName))
        {
            PlayerPrefs.SetString(achievementName, finishTime);
            PlayerPrefs.SetInt(achievementName + "Star", 3);
            c_AchievementCtrl.WriteFinishTime(achievementName, finishTime, 3);
            LastestAchievement = achievementName;
        }
        else if(FinishAllAchievement && accuracy <= 0)
        {
            AchievementInstance hiddenAchievement = c_AchievementCtrl.GetAllAchievements().Find(x => x.cInstance.symbolID == SymbolID.Hidden);
            PlayerPrefs.SetString(hiddenAchievement.achievementName, finishTime);
            PlayerPrefs.SetInt(hiddenAchievement.achievementName + "Star", 3);
            c_AchievementCtrl.WriteFinishTime(hiddenAchievement.achievementName, finishTime, 3);
            LastestAchievement = hiddenAchievement.achievementName;
        }
        return achievementName;
    }
    /// <summary>
    /// 截屏
    /// </summary>
    /// <param name="mRect"></param>
    /// <param name="filePath"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator CaptureScreenShotByRect(Rect mRect, string filePath, PlatformType type)
    {
        //等待渲染线程结束
        yield return new WaitForEndOfFrame();
        //初始化Texture2D
        Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据
        mTexture.ReadPixels(mRect, 0, 0,false);
        //应用
        mTexture.Apply(false);
        //将图片信息编码为字节信息
        byte[] bytes = mTexture.EncodeToPNG();
        //保存
        File.WriteAllBytes(filePath, bytes);

        ShareImage(filePath, type);
    }

    private void ShareImage(string filePath, PlatformType type)
    {
        ShareContent content = new ShareContent();
        if(type == PlatformType.WeChatMoments || type == PlatformType.WeChat)
        {
            content.SetImagePath(filePath);
            content.SetShareType(ContentType.Image);
        }
        else if(type == PlatformType.SinaWeibo)
        {
            //content.SetText(text);
            //content.SetImagePath(filePath);
        }
        m_ShareSDK.ShareContent(type, content);
    }

    /// <summary>
    /// ShareSDK分享回调
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="result"></param>
    private void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            //print("share successfully - share result :");
            //print(MiniJSON.jsonEncode(result));
            //Hide Share Panel!
        }
        else if (state == ResponseState.Fail)
        {
            #if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
            #elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
            #endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    private void InitShareIcon()
    {
        string path = Application.persistentDataPath + "/Image/ShareIcon.png";
        if (!File.Exists(path)) StartCoroutine(AssetHelper.CopyImage("ShareIcon.png"));
    }

    /// <summary>  
    /// Load Image from Persistent folder  
    /// </summary>  
    /// <param name="path"></param>  
    /// <param name="image"></param>  
    private void LoadImageFromPersistent(string path, Image image)
    {
        path = AssetHelper.GetPersistentPathForWWW() + path;
        StartCoroutine(AssetHelper.LoadImage(path, image));
    }

    /// <summary>  
    /// Load Image from Streaming folder  
    /// </summary>  
    /// <param name="path"></param>  
    /// <param name="image"></param>  
    private void LoadImageFromStreaming(string path, Image image)
    {
        path = AssetHelper.GetStreamingPathForWWW() + path;
        StartCoroutine(AssetHelper.LoadImage(path, image));
    }

    private void TweenComplete(GameObject targetWrapper)
    {
        Destroy(m_CurWrapper);
        m_CurWrapper = targetWrapper;
        m_Root.GetComponent<GraphicRaycaster>().enabled = true;
    }
    #endregion

}
