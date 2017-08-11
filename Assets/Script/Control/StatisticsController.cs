using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StatisticsController : Controller
{
    #region 单例
    private static StatisticsController Instance
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
        base.id = ControllerID.StatisticsController;
        InitStatisticsData();
    }


    private void InitStatisticsData()
    {

    }
}
public struct StatisticsInstance
{

}



