using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : Controller 
{
    #region C#单例
    private static SkinController instance = null;
    private SkinController()
    {
        base.id = ControllerID.SkinController;
        spriteDict = new Dictionary<string, Object>();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static SkinController Instance
    {
        get { return instance ?? (instance = new SkinController()); }
    }
    #endregion
    private Dictionary<string, Object> spriteDict;//key：string，value：sprite预设体

    public Object GetSpriteResource(SkinID id,string index)
    {
        string path = "";
        switch (id)
        {
            case SkinID.Default:
                path = "Skin/Default/";
                break;
            case SkinID.FreshGreen:
                path = "Skin/FreshGreen/";
                break;
            case SkinID.RosePink:
                path = "Skin/RosePink/";
                break;
            case SkinID.SkyBlue:
                path = "Skin/SkyBlue/";
                break;
            default:
                Debug.LogError("Unknow id:" + id.ToString());
                break;
        }
        if (path == "") return null;
        Object resouce = null;
        if (spriteDict.ContainsKey(index))
        {
            resouce = spriteDict[index];
        }
        else
        {
            resouce = Resources.Load(path + index);
            spriteDict.Add(index, resouce);
        }
        return resouce;
    }
    //切换SkinID的时候需要清空spriteDict！！！
    public void ClearSpriteDict()
    {
        spriteDict.Clear();
    }

}
