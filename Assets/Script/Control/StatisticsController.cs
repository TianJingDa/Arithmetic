using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StatisticsController : Controller
{
    #region C#单例
    private static StatisticsController instance = null;
    private StatisticsController()
    {
        base.id = ControllerID.StatisticsController;
        InitStatisticsData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static StatisticsController Instance
    {
        get { return instance ?? (instance = new StatisticsController()); }
    }
    #endregion
    private void InitStatisticsData()
    {

    }
}
public struct StatisticsInstance
{

}



