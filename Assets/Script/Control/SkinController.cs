using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : Controller 
{
    #region
    private static SkinController Instance
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
        base.id = ControllerID.SkinController;
        InitSkinData();
    }
    private void InitSkinData()
    {

    }
}
