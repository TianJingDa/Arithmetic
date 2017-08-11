using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementController : Controller 
{
    #region 单例
    private static AchievementController Instance
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
        base.id = ControllerID.AchievementController;
        InitAchievementData();
    }
    private void InitAchievementData()
    {

    }
}
