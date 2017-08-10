using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GuiObjectCtrl : Controller  
{
    #region 单例
    private static GuiObjectCtrl Instance
    {
        get;
        set;
    }
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            InitController();
            GameManager.Instance.RegisterController(base.id, Instance);
        }
    }
    #endregion

    private Dictionary<GuiFrameID, GameObject> guiFrameWrapperDict;//key：GuiFrameID，value：FrameWrapper
    protected override void InitController()
    {
        base.id = ControllerID.GuiObjectCtrl;
        guiFrameWrapperDict = new Dictionary<GuiFrameID, GameObject>();
    }
    public void RegisterGuiObject(GuiFrameID id, GameObject wrapper)
    {
        if (!guiFrameWrapperDict.ContainsKey(id))
        {
            guiFrameWrapperDict.Add(id, wrapper);
        }
        else
        {
            Debug.Log("重复注册：" + id.ToString());
        }
    }
    public void UnRegisterGuiObject(GuiFrameID id)
    {
        if (guiFrameWrapperDict.ContainsKey(id))
        {
            guiFrameWrapperDict.Remove(id);
        }
        else
        {
            Debug.Log("重复注销：" + id.ToString());
        }
    }

    public GameObject GetGuiObject(GuiFrameID id)
    {
        GameObject wrapper = null;
        guiFrameWrapperDict.TryGetValue(id, out wrapper);
        return wrapper;
    }
    public bool IsRegister(GuiFrameID id)
    {
        return guiFrameWrapperDict.ContainsKey(id);
    }
}
