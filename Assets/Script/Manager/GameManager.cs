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
    private const float                                         TimeOut = 1f;
	private const string                                        VisitorURL = "http://47.105.77.226:8091/register";

    private MutiLanguageController                              c_MutiLanguageCtrl;
    private ResourceController                                  c_ResourceCtrl;
    private FightController                                     c_FightCtrl;
    private AchievementController                               c_AchievementCtrl;
    private SkinController                                      c_SkinCtrl;
    private LayoutController                                    c_LayoutCtrl;
    private FontController                                      c_FontCtrl;
    private TextColorController                                 c_TextColorCtrl;
    private RecordController                                    c_RecordCtrl;
    private RankController                                      c_RankController;

    private int[]                                               m_AmountArray_Time;
    private int[]                                               m_AmountArray_Number;
    private string[]                                            m_SymbolArray;
    private bool                                                m_IsCentral;
    private bool                                                m_IsLogining;
    private float                                               m_TweenDuration = 0.5f;             //Tween动画持续时间
    private GameObject                                          m_Root;                             //UI对象的根对象
    private Stack<GuiFrameWrapper>                              m_GuiFrameStack;                    //当前激活的GuiWrapper
    private CategoryInstance                                    m_CurCategoryInstance;              //当前试题选项
    private System.Action                                       m_CurAction;                        //用于刷新SaveFile和Achievement列表
    private System.Action                                       m_ShareAction;                      //用于分享时初始化用户名称
    private ShareSDK                                            m_ShareSDK;                         //用于分享成就和成绩

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
    public KeyboardID CurKeyboardID
    {
        get
        {
            int keyboardID = PlayerPrefs.GetInt("KeyboardID", 0);
            return (KeyboardID)keyboardID;
        }
        set
        {
            int keyboardID = (int)value;
            PlayerPrefs.SetInt("KeyboardID", keyboardID);
        }
    }
    public CategoryInstance CurCategoryInstance
    {
		get
		{
			return m_CurCategoryInstance;
		}
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
    public SaveFileInstance CurSaveFileInstance { get; set; }

    public AchievementInstance CurAchievementInstance { get; set; }

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
    /// <summary>
    /// 用于区分三种竞赛方式：成就、自由、蓝牙 
    /// </summary>
	public GuiFrameID CompetitionGUI
    {
        get;
        set;
    }
    //获取栈顶第二个元素的ID
    public GuiFrameID LastGUI
    {
        get
        {
            GuiFrameID id = GuiFrameID.None;
            lock (m_GuiFrameStack)
            {
                GuiFrameWrapper wrapper = m_GuiFrameStack.Pop();
                if (m_GuiFrameStack.Count > 0)
                {
                    id = m_GuiFrameStack.Peek().id;
                }
                m_GuiFrameStack.Push(wrapper);
            }
            return id;
        }
    }

    public GuiFrameID CurGUI
    {
        get
        {
            GuiFrameID id = GuiFrameID.None;
            if (m_GuiFrameStack.Count > 0)
            {
                id = m_GuiFrameStack.Peek().id;
            }
            return id;
        }
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

	public string Token
	{
		get
		{
			return PlayerPrefs.GetString("Token", null);
		}
		private set
		{
			PlayerPrefs.SetString("Token", value);
		}
	}

	public string UserID
	{
		get
		{
			return PlayerPrefs.GetString("UserID", null);
		}
		private set
		{
			PlayerPrefs.SetString("UserID", value);
		}
	}
		
	public bool IsLogin
	{
		get
		{
			bool hasToken = !string.IsNullOrEmpty(Token);
			bool hasUserID = !string.IsNullOrEmpty(UserID);
			return hasToken && hasUserID;
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
            CanvasGroup canvas = null;
            if (m_GuiFrameStack.Count > 0)
            {
                GuiFrameWrapper gui = m_GuiFrameStack.Peek();
                canvas = gui.GetComponent<CanvasGroup>();
            }
            return canvas;
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

    private bool ResetUserName
    {
        get
        {
            int reset = PlayerPrefs.GetInt("ResetUserName", 0);
            return reset > 0;
        }
        set
        {
            int reset = value ? 1 : 0;
            PlayerPrefs.SetInt("ResetUserName", reset);
        }
    }

    public string ServiceUUID{ get; set;}

	public string ReadUUID{ get; set;}

	public string WriteUUID{ get; set;}

	public PeripheralInstance CurPeripheralInstance { get; set; }

    public CommonTipInstance CurCommonTipInstance { get; set; }

    public ShareInstance CurShareInstance { get; set; }

    public System.Action<BluetoothMessage> BLESendMessage { get; set; }

    public System.Action<BluetoothMessage> BLEReceiveMessage { get; set; }

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
        c_RankController        = RankController.Instance;
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        m_Root = GameObject.Find("UIRoot");
        m_GuiFrameStack = new Stack<GuiFrameWrapper>();
        m_AmountArray_Time = new int[] { 60, 180, 300 };//这里不应该直接写在代码里，但应该写在哪里？
        m_AmountArray_Number = new int[] { 10, 30, 50 };
        m_SymbolArray = new string[] { "＋", "－", "×", "÷" };
        m_ShareSDK = GetComponent<ShareSDK>();
        m_ShareSDK.shareHandler = OnShareResultHandler;
        InitShareIcon();
        ResetUserNameOnce();
        if (!IsLogin) StartSilentLogin();
        SwitchWrapper(GuiFrameID.StartFrame, true);
#if UNITY_EDITOR
        gameObject.AddComponent<Camera>();
#endif
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
    public void SaveRecord(List<List<int>> resultList, string symbol, float timeCost, bool isAchievement, bool isBluetooth)
    {
        SaveFileInstance curSaveFileInstance = new SaveFileInstance();

        curSaveFileInstance.isUpload = false;

        curSaveFileInstance.timeCost = timeCost;

        string finishTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        curSaveFileInstance.fileName = finishTime;

        float accuracy = CalculateAccuracy(resultList);
        curSaveFileInstance.accuracy = accuracy;

        List<QuestionInstance> qInstanceList = ConvertToInstanceList(resultList, symbol);
        curSaveFileInstance.qInstancList = qInstanceList;

        CurAchievementName = "";
        if (isAchievement)
        {
            CurAchievementName = CheckAchievement(timeCost / resultList.Count, accuracy, finishTime);
        }
        curSaveFileInstance.achievementName = CurAchievementName;

        curSaveFileInstance.cInstance = m_CurCategoryInstance;

        if (isBluetooth)
        {
            curSaveFileInstance.opponentName = CurPeripheralInstance.name;
        }

        CurSaveFileInstance = curSaveFileInstance;
        string toSave = JsonUtility.ToJson(curSaveFileInstance);
        c_RecordCtrl.SaveRecord(toSave, finishTime);

        TotalGame++;
        TotalTime += timeCost;
    }

    public void EditRecord(SaveFileInstance instance)
    {
        string toSave = JsonUtility.ToJson(instance);
        c_RecordCtrl.SaveRecord(toSave, instance.fileName);
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
        c_AchievementCtrl.ResetAllAchievement();
    }
    public void ResetSaveFileWithoutAchievement()
    {
        List<string> fileNameList = c_AchievementCtrl.GetAllFileNameWithAchievement();
        c_RecordCtrl.DeleteRecordWithoutAchievement(fileNameList);
    }
    public void DeleteRecord(string fileName, string achievementName)
    {
        if (c_RecordCtrl.DeleteRecord(fileName))
        {
            if (m_CurAction != null) m_CurAction();
            else MyDebug.LogYellow("UnRegister Function!");
            if (!string.IsNullOrEmpty(achievementName))
            {
                c_AchievementCtrl.DeleteAchievement(achievementName);

                string lastestAchievement = PlayerPrefs.GetString("LastestAchievement", "");
                if (lastestAchievement.Contains(achievementName))
                {
                    lastestAchievement = lastestAchievement.Replace(achievementName + ",", "");
                    PlayerPrefs.SetString("LastestAchievement", lastestAchievement);
                }
            }
        }
        else
        {
            MyDebug.LogYellow("Delete File Fail!");
        }
    }
    public void DeleteAchievement(string fileName, string achievementName)
    {
        if (!PlayerPrefs.HasKey(achievementName))
        {
            MyDebug.LogYellow("Wrong AchievementName!");
            return;
        }
        c_AchievementCtrl.DeleteAchievement(achievementName);
        if (c_RecordCtrl.DeleteRecord(fileName))
        {
            if (m_CurAction != null) m_CurAction();
            else MyDebug.LogYellow("UnRegister Function!");
        }
        else
        {
            MyDebug.LogYellow("Delete File Fail!");
        }
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
    public AchievementInstance GetAchievement(string achievementName = "")
    {
        if (string.IsNullOrEmpty(achievementName)) achievementName = CurAchievementName;
        return c_AchievementCtrl.GetAchievement(achievementName);
    }
    public AchievementInstance GetLastestAchievement()
    {
        if (string.IsNullOrEmpty(LastestAchievement)) return new AchievementInstance();
        return c_AchievementCtrl.GetAchievement(LastestAchievement);
    }
    public void ResetAchievement()
    {
        List<string> fileNameList = c_AchievementCtrl.GetAllFileNameWithAchievement();
        c_RecordCtrl.DeleteRecordWithAchievement(fileNameList);
        c_AchievementCtrl.ResetAllAchievement();
        PlayerPrefs.DeleteKey("LastestAchievement");
    }

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
        string title = GetMutiLanguage("Text_00072");
        string description = GetMutiLanguage("Text_00073");
        ShareContent content = new ShareContent();
        content.SetImagePath(Application.persistentDataPath + "/Image/ShareIcon.png");
        if (type == PlatformType.WeChatMoments || type == PlatformType.WeChat)
        {
            content.SetText(description);
            content.SetTitle(title);
            content.SetUrl("https://www.taptap.com/app/78306");
            content.SetShareType(ContentType.Webpage);
        }
        else if(type == PlatformType.SinaWeibo)
        {
            content.SetText(title + "https://www.taptap.com/app/78306");//text是Url
        }
        m_ShareSDK.ShareContent(type, content);
    }

    /// <summary>
    /// GuiWrapper切换，无动画
    /// </summary>
    /// <param name="targetID"></param>
    public void SwitchWrapper(GuiFrameID targetID, bool isAdd = false)
    {
        if (!isAdd)
        {
            if (m_GuiFrameStack.Count > 0)
            {
                GuiFrameWrapper oldGui = m_GuiFrameStack.Pop();
                if (oldGui) Destroy(oldGui.gameObject);
            }
            if (m_GuiFrameStack.Count > 0)
            {
                GuiFrameWrapper oldGui = m_GuiFrameStack.Peek();
                if (oldGui) oldGui.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else
        {
            if (m_GuiFrameStack.Count > 0)
            {
                GuiFrameWrapper oldGui = m_GuiFrameStack.Peek();
                if (oldGui) oldGui.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        if (targetID != GuiFrameID.None)
        {
            Object reource = c_ResourceCtrl.GetGuiResource(targetID);
            if (reource == null)
            {
                MyDebug.LogYellow("Can not load reousce:" + targetID.ToString());
                return;
            }
            GameObject newGui = Instantiate(reource, m_Root.transform) as GameObject;
            m_GuiFrameStack.Push(newGui.GetComponent<GuiFrameWrapper>());
        }
    }
    /// <summary>
    /// GuiWrapper切换，有移动动画
    /// </summary>
    /// <param name="targetID"></param>
    /// <param name="mID"></param>
    /// <param name="isIn"></param>
    public void SwitchWrapperWithMove(GuiFrameID targetID, MoveID mID, bool isIn)
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
            GuiFrameWrapper topGui = null;
            if (m_GuiFrameStack.Count > 0) topGui = m_GuiFrameStack.Peek();
            if(topGui) topGui.transform.DOLocalMoveX(Screen.width * (int)mID, m_TweenDuration, true).
                                   SetEase(Ease.OutQuint).
                                   OnComplete(() => TweenComplete(targetWrapper));
        }
    }
    /// <summary>
    /// GuiWrapper切换，有缩放动画
    /// </summary>
    /// <param name="targetID"></param>
    /// <param name="isIn"></param>
    public void SwitchWrapperWithScale(GuiFrameID targetID, bool isIn)
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
            GuiFrameWrapper topGui = null;
            if (m_GuiFrameStack.Count > 0) topGui = m_GuiFrameStack.Peek();
            if (topGui) topGui.transform.DOScale(Vector3.zero, m_TweenDuration).
                                   SetEase(Ease.OutQuint).
                                   OnComplete(() => TweenComplete(targetWrapper));
        }
    }

    /// <summary>
    /// 获取PrefabItem
    /// </summary>
    /// <param name="name">Item名字</param>
    /// <returns></returns>
    public GameObject GetPrefabItem(GuiItemID id)
    {
        Object resource = c_ResourceCtrl.GetItemResource(id);
        return Instantiate(resource) as GameObject;
    }


    public void CentralReceiveMessage(string address, string characteristic, byte[] bytes)
    {
        MyDebug.LogGreen("CentralReceiveMessage");

        BluetoothMessage msg = new BluetoothMessage(bytes);
        if (msg == null)
        {
            MyDebug.LogYellow("CentralReceiveMessage: Message is NULL!");
            return;
        }

        MyDebug.LogGreen("Index:" + msg.index + ", Result:" + msg.result + ", name:" + msg.name);

        if (msg.index == 0)
        {
            Random.InitState(msg.result);
            CompetitionGUI = GuiFrameID.BluetoothFrame;
            SwitchWrapper(GuiFrameID.BluetoothFightFrame);
        }
        else
        {
            if (BLEReceiveMessage != null) BLEReceiveMessage(msg);
        }
    }

    public void PeripheralReceiveMessage(string UUID, byte[] bytes)
    {
        MyDebug.LogGreen("PeripheralReceiveMessage");
        BluetoothMessage msg = new BluetoothMessage(bytes);
        if(msg == null)
        {
            MyDebug.LogYellow("PeripheralReceiveMessage: Message is NULL!");
            return;
        }

        MyDebug.LogGreen("Index:" + msg.index + ", Result:" + msg.result + ", name:" + msg.name);

        if (msg.index == 0)
        {
            Random.InitState(msg.result);
            CurPeripheralInstance = new PeripheralInstance("", msg.name);
            SetSendMessageFunc(false);
            BLESendMessage(msg);
            CompetitionGUI = GuiFrameID.BluetoothFrame;
            SwitchWrapper(GuiFrameID.BluetoothFightFrame);
        }
        else
        {
            if (BLEReceiveMessage != null) BLEReceiveMessage(msg);
        }
    }

    public void SetSendMessageFunc(bool isCentral)
    {
        m_IsCentral = isCentral;
        if (isCentral) BLESendMessage = CentralSendMessage;
        else BLESendMessage = PeripheralSendMessage;
    }

    public void OnBluetoothFightFinish()
    {
        MyDebug.LogGreen("OnBluetoothFightFinish");
        if (m_IsCentral)
        {
            MyDebug.LogGreen("OnBluetoothFightFinish:Central");
            BluetoothLEHardwareInterface.UnSubscribeCharacteristic(CurPeripheralInstance.address, 
                                                                   ServiceUUID, 
                                                                   ReadUUID, 
                                                                   (characteristic) => 
                {
                    MyDebug.LogGreen("UnSubscribeCharacteristic Success :" + characteristic);
                });

            BluetoothLEHardwareInterface.DisconnectPeripheral(CurPeripheralInstance.address,
                (disconnectAddress) =>
                {
                    MyDebug.LogGreen("DisconnectPeripheral Success:" + disconnectAddress);
                });

        }
        else
        {
            MyDebug.LogGreen("OnBluetoothFightFinish:Peripheral");
            BluetoothLEHardwareInterface.StopAdvertising(() =>
            {
                MyDebug.LogGreen("Stop Advertising!");
            });
        }

        BluetoothLEHardwareInterface.RemoveCharacteristics();
        BluetoothLEHardwareInterface.RemoveServices();

        BluetoothLEHardwareInterface.DeInitialize(() =>
        {
            MyDebug.LogGreen("DeInitialize Success!");
        });
        BluetoothLEHardwareInterface.BluetoothEnable(false);
    }

	public void DownloadRecord(CategoryInstance instance, System.Action<ArrayList> OnSucceed,System.Action<string> OnFail)
    {
		StartCoroutine(c_RankController.DownloadRecord(instance, OnSucceed, OnFail));
    }

	public void UploadRecord(WWWForm form, System.Action<string> OnSucceed, System.Action<string> OnFail)
    {
        StartCoroutine(c_RankController.UploadRecord(form, OnSucceed, OnFail));
    }

	public void GetRankDetail(WWWForm form, System.Action<string> OnSucceed, System.Action<string> OnFail)
	{
		StartCoroutine(c_RankController.GetRankDetail(form, OnSucceed, OnFail));
	}

    public void StartSilentLogin(System.Action OnSucceed = null, System.Action<string> OnFail = null)
	{
        if (m_IsLogining)
        {
            return;
        }
        m_IsLogining = true;
        StartCoroutine(SilentLogin(OnSucceed, OnFail));
	}

    #endregion

    #region 私有方法

    private void CentralSendMessage(BluetoothMessage message)
    {
        MyDebug.LogGreen("CentralSendMessage");
        MyDebug.LogGreen("index:" + message.index);
        MyDebug.LogGreen("result:" + message.result);
        MyDebug.LogGreen("name:" + message.name);
        MyDebug.LogGreen("Length:" + message.data.Length);
        BluetoothLEHardwareInterface.WriteCharacteristic(CurPeripheralInstance.address, ServiceUUID, WriteUUID, message.data, message.data.Length, true, (characteristicUUID) =>
        {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }

    private void PeripheralSendMessage(BluetoothMessage message)
    {
        MyDebug.LogGreen("PeripheralSendMessage");
        MyDebug.LogGreen("index:" + message.index);
        MyDebug.LogGreen("result:" + message.result);
        MyDebug.LogGreen("name:" + message.name);
        MyDebug.LogGreen("Length:" + message.data.Length);
        BluetoothLEHardwareInterface.UpdateCharacteristicValue(ReadUUID, message.data, message.data.Length);
    }

    private float CalculateAccuracy(List<List<int>> resultList)
    {
        List<List<int>> rightList = resultList.FindAll(x => x[x.Count - 1] == x[x.Count - 2]);
        float accuracy = (float)rightList.Count * 100 / resultList.Count;
        return accuracy; 
    }
    private List<QuestionInstance> ConvertToInstanceList(List<List<int>> resultList, string symbol)
    {
        List<QuestionInstance> qInstanceList = new List<QuestionInstance>();
        string count = resultList.Count.ToString();
        for (int i = 0; i < resultList.Count; i++)
        {
            QuestionInstance questionInstance = new QuestionInstance();
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
        int star = 0;
        List<AchievementInstance> achievementList = c_AchievementCtrl.GetAchievementUnFinish();
        for(int i = 0; i < achievementList.Count; i++)
        {
            if(achievementList[i].cInstance.Equals(m_CurCategoryInstance))
            {
                achievementName = achievementList[i].achievementName;
#if UNITY_EDITOR || SHOW_DEBUG
                star = 3;
#else
                if (achievementList[i].accuracy <= accuracy && achievementList[i].meanTime >= meanTime)
                {
                    star = 3;
                }
                else if(achievementList[i].accuracy -5 <= accuracy && achievementList[i].meanTime * 1.1 >= meanTime)
                {
                    star = 2;
                }
                else if(achievementList[i].accuracy - 10 <= accuracy && achievementList[i].meanTime * 1.2 >= meanTime)
                {
                    star = 1;
                }
#endif
                break;
            }
        }
        switch (star)
        {
            case 3:
                PlayerPrefs.SetString(achievementName, finishTime);
                PlayerPrefs.SetInt(achievementName + "Star", 3);
                c_AchievementCtrl.WriteFinishTime(achievementName, finishTime, 3);
                LastestAchievement = achievementName;
                break;
            case 2:
            case 1:
                PlayerPrefs.SetInt(achievementName + "Star", star);
                c_AchievementCtrl.WriteFinishTime(achievementName, "", star);
                break;
            default:
                break;
        }

        //else if(FinishAllAchievement && accuracy <= 0)
        //{
        //    AchievementInstance hiddenAchievement = c_AchievementCtrl.GetAllAchievements().Find(x => x.cInstance.symbolID == SymbolID.Hidden);
        //    PlayerPrefs.SetString(hiddenAchievement.achievementName, finishTime);
        //    PlayerPrefs.SetInt(hiddenAchievement.achievementName + "Star", 3);
        //    c_AchievementCtrl.WriteFinishTime(hiddenAchievement.achievementName, finishTime, 3);
        //    LastestAchievement = hiddenAchievement.achievementName;
        //}
        return star == 3 ? achievementName : "";
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
        content.SetImagePath(filePath);
        if (type == PlatformType.WeChatMoments || type == PlatformType.WeChat)
        {
            content.SetShareType(ContentType.Image);
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
        if (m_GuiFrameStack.Count > 0)
        {
            GuiFrameWrapper oldGui = m_GuiFrameStack.Pop();
            if (oldGui) Destroy(oldGui.gameObject);
        }
        m_GuiFrameStack.Push(targetWrapper.GetComponent<GuiFrameWrapper>());
        m_Root.GetComponent<GraphicRaycaster>().enabled = true;
    }

    /// <summary>
    /// 为了兼容之前版本中已经起名的玩家，需要清除用户名，重新起名
    /// </summary>
    private void ResetUserNameOnce()
    {
        if (!ResetUserName)
        {
            UserName = null;
            ResetUserName = true;
        }
    }

    /// <summary>
    /// 获取游客信息
    /// </summary>
    /// <returns></returns>
    private IEnumerator SilentLogin(System.Action OnSucceed, System.Action<string> OnFail)
    {
        WWW www = new WWW(VisitorURL);

        float responseTime = 0;
        while (!www.isDone && responseTime < TimeOut)
        {
            responseTime += Time.deltaTime;
            yield return www;
        }

        if (www.isDone)
        {
			LoginResponse response = JsonUtility.FromJson<LoginResponse>(www.text);
			if (response != null)
			{
				if (response.code == 200)
				{
                    m_IsLogining = false;
					Token = response.token;
					UserID = response.data.id;
                    if (OnSucceed != null)
                    {
                        OnSucceed();
                    }
                    yield break;
				}
				else
				{
					MyDebug.LogYellow("Silent Login Fail:" + response.code);
				}
			}
			else
			{
				MyDebug.LogYellow("Silent Login Fail: Message Is Not Response!");
			}
        }
        else
        {
            MyDebug.LogYellow("Silent Login Fail Fail: " + www.error);
        }

        m_IsLogining = false;
        string message = GetMutiLanguage("Text_20066");
        if (OnFail != null)
        {
            OnFail(message);
        }
    }

	[System.Serializable]
	private class LoginResponse
	{
		public int code;
		public string errmsg;
		public string token;
		public LoginData data;

	}

	[System.Serializable]
	private class LoginData
	{
		public string id;
		public string name;
	}

    #endregion

}
