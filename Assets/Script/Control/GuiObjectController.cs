using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GuiObjectController  
{
    #region C#单例
    private static GuiObjectController instance = null;
    private GuiObjectController()
    {
        guiFrameWrapperDict = new Dictionary<GuiFrameID, GameObject>();
    }
    public static GuiObjectController Instance
    {
        get { return instance ?? (instance = new GuiObjectController()); }
    }
    #endregion

    private Dictionary<GuiFrameID, GameObject> guiFrameWrapperDict;//key：GuiFrameID，value：FrameWrapper
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
