using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontController : Controller 
{
    #region 单例
    private static FontController Instance
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
            SendMessage("RegisterController", Instance, SendMessageOptions.RequireReceiver);
        }
    }
    #endregion
    protected override void InitController()
    {
        base.id = ControllerID.FontController;
        InitFontData();
    }
    private void InitFontData()
    {

    }
}
