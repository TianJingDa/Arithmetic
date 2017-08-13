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
        InitFontData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static FontController Instance
    {
        get { return instance ?? (instance = new FontController()); }
    }
    #endregion

    private void InitFontData()
    {

    }
}
