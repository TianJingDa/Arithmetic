using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StatisticsCtrl : Controller
{
    #region 单例
    private static StatisticsCtrl Instance
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
        base.id = ControllerID.StatisticsCtrl;
        InitStatisticsData();
    }


    private void InitStatisticsData()
    {

    }
}
public struct StatisticsInstance
{

}



