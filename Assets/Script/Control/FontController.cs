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
        fontAssetDict = new Dictionary<SkinID, string[]>();
        InitFontData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static FontController Instance
    {
        get { return instance ?? (instance = new FontController()); }
    }
    #endregion
    private Dictionary<SkinID, string[]> fontAssetDict;//key：SkinID，value：资源路径

    private void InitFontData()
    {
        fontAssetDict.Add(SkinID.Default, new string[] { "Default/Chinese", "Default/English" });
        fontAssetDict.Add(SkinID.FreshGreen, new string[] { "FreshGreen/Chinese", "FreshGreen/English" });
        fontAssetDict.Add(SkinID.RosePink, new string[] { "RosePink/Chinese", "RosePink/English" });
        fontAssetDict.Add(SkinID.SkyBlue, new string[] { "SkyBlue/Chinese", "SkyBlue/English" });
    }
    public Object GetFontResource(SkinID sID,LanguageID lID)
    {
        Object resouce = Resources.Load(fontAssetDict[sID][(int)lID]);
        return resouce;
    }

}
