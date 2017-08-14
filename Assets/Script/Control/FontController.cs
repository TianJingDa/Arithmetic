using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontController : Controller 
{
    #region C#单例
    private static FontController instance = null;
    private FontController()
    {
        base.id = ControllerID.FontController;
        fontAssetDict = new Dictionary<FontID, string>();
        InitFontData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static FontController Instance
    {
        get { return instance ?? (instance = new FontController()); }
    }
    #endregion
    private Dictionary<FontID, string> fontAssetDict;//key：FontID，value：资源路径

    private void InitFontData()
    {
        fontAssetDict.Add(FontID.FZSTK, "Font/FZSTK");
        fontAssetDict.Add(FontID.STKAITI, "Font/STKAITI");
        fontAssetDict.Add(FontID.STXINGKA, "Font/STXINGKA");
    }
    public Object GetFontResource(FontID id)
    {
        Object resouce = Resources.Load(fontAssetDict[id]);
        return resouce;
    }

}
