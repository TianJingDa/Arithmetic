using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementController : Controller 
{
    #region C#单例
    private static AchievementController instance = null;
    private AchievementController()
    {
        base.id = ControllerID.AchievementController;
        InitAchievementData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static AchievementController Instance
    {
        get { return instance ?? (instance = new AchievementController()); }
    }
    #endregion

    private void InitAchievementData()
    {

    }
}
