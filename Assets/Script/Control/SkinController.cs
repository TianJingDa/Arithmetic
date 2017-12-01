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
        path = "Skin/{0}/";
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
    }
    public static SkinController Instance
    {
        get { return instance ?? (instance = new SkinController()); }
    }
    #endregion

    private string path;

    public Sprite GetSpriteResource(SkinID id,string index)
    {
        return null;
        GameObject resouce = Resources.Load<GameObject>(string.Format(path, id) + index);
        return resouce.GetComponent<Sprite>();
    }
}
