using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCtrl : Controller 
{
    #region 单例
    private static AchievementCtrl Instance
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
    protected override void InitController()
    {
        base.id = ControllerID.AchievementCtrl;
        InitAchievementData();
    }
    private void InitAchievementData()
    {

    }
}
