using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏控制层
/// </summary>
public class GameManager : MonoBehaviour
{
    private GameObject root;
    private Clock clock = null;
    private Dictionary<ControllerID, Controller> controllerDict;
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
            controllerDict = new Dictionary<ControllerID, Controller>();
        }
    }

    void Start()
    {
        root = GameObject.Find("Canvas");
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
        if (clock != null)
        {
            clock.Update();
        }
    }

    #region 公共方法
    /// <summary>
    /// 修改语言
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(Language language)
    {
        if ((controllerDict[ControllerID.GuiObjectCtrl] as GuiObjectCtrl).IsRegister(GuiFrameID.SetUpFrame))
        {
            (controllerDict[ControllerID.MutiLanguageCtrl] as MutiLanguageCtrl).Language = language;
        }
    }
    public void RegisterController(ControllerID id, Controller ctrl)
    {
        if (!controllerDict.ContainsKey(id))
        {
            controllerDict.Add(id, ctrl);
        }
        else
        {
            Debug.Log("重复注册！");
        }
    }
    /// <summary>
    /// 注册时钟
    /// </summary>
    /// <param name="clock"></param>
    public void RegisterClock(Clock clock)
    {
        this.clock = null;
        System.GC.Collect();
        this.clock = clock;
    }
    /// <summary>
    /// 注销时钟
    /// </summary>
    public void UnRegisterClock()
    {
        this.clock = null;
        System.GC.Collect();
    }
    /// <summary>
    /// 激活GUI
    /// </summary>
    /// <param name="id"></param>
    public void ActiveGui(GuiFrameID id)
    {
        Object reource = (controllerDict[ControllerID.ResourceCtrl] as ResourceCtrl).GetResource(id);
        if (reource == null)
        {
            Debug.Log("Can not load reousce:" + id.ToString());
            return;
        }
        GameObject wrapper = Instantiate(reource, root.transform) as GameObject;
        (controllerDict[ControllerID.GuiObjectCtrl] as GuiObjectCtrl).RegisterGuiObject(id, wrapper);
    }
    /// <summary>
    /// 销毁GUI
    /// </summary>
    /// <param name="id"></param>
    public void DeActiveGui(GuiFrameID id)
    {
        GameObject guiObject = (controllerDict[ControllerID.GuiObjectCtrl] as GuiObjectCtrl).GetGuiObject(id);
        (controllerDict[ControllerID.GuiObjectCtrl] as GuiObjectCtrl).UnRegisterGuiObject(id);
        Destroy(guiObject);
        System.GC.Collect();
    }
    #endregion

    #region 私有方法
    //private  Controller ControllerInstance(ControllerID id)
    //{
    //    Controller ctrl = controllerDict[id];
    //    if(ctrl is MutiLanguageCtrl)
    //    {
    //        return (MutiLanguageCtrl)ctrl;
    //    }
    //    else if(ctrl is ResourceCtrl)
    //    {
    //        return (ResourceCtrl)ctrl;
    //    }
    //}

    #endregion

}
