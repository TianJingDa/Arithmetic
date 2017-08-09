using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏控制层
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject root;

    private MutiLanguageController mutiLanguageCtrl;//多语言控制器
    private ResourceController resourceCtrl;//资源控制器
    private GuiObjectController guiObjectCtrl;//GUI实例控制器
    private ExamController examCtrl;//考试控制器
    private StatisticsController statisticsCtrl;//统计数据控制器
    private TimeManager timeMgr;//时间控制器
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
        mutiLanguageCtrl = MutiLanguageController.Instance;
        resourceCtrl = ResourceController.Instance;
        guiObjectCtrl = GuiObjectController.Instance;
        examCtrl = ExamController.Instance;
        statisticsCtrl = StatisticsController.Instance;
        timeMgr = TimeManager.Instance;
    }

    void Start ()
    {
        mutiLanguageCtrl.InitLanguageDict();
        resourceCtrl.RegisterAsset();
        statisticsCtrl.InitStatisticsData();
        timeMgr.InitTimeMgr();
        ActiveGui(GuiFrameID.StartFrame);
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00000"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00001"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00002"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00003"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00004"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00005"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00006"));
        //Debug.Log(mutiLanguageCtrl.GetMutiLanguage("TJD_00007"));

    }

    void Update ()
    {
        timeMgr.Update();

    }

    #region 公共方法
    public void SetLanguage(Language language)
    {
        if (guiObjectCtrl.IsRegister(GuiFrameID.SetUpFrame))
        {
            mutiLanguageCtrl.Language = language;
        }
    }
    public void ActiveGui(GuiFrameID id)
    {
        Object reource = resourceCtrl.GetResource(id);
        if (reource == null)
        {
            Debug.Log("Can not load reousce:"+id.ToString());
            return;
        }
        GameObject wrapper = Instantiate(reource, root.transform) as GameObject;
        guiObjectCtrl.RegisterGuiObject(id, wrapper);
    }
    public void DeActiveGui(GuiFrameID id)
    {
        Destroy(guiObjectCtrl.GetGuiObject(id));
        guiObjectCtrl.UnRegisterGuiObject(id);
    }
    #endregion

    #region 私有方法


    #endregion

}
