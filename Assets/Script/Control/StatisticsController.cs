using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StatisticsController  
{
    #region C#单例
    private static StatisticsController instance = null;
    private StatisticsController() { }
    public static StatisticsController Instance
    {
        get { return instance ?? (instance = new StatisticsController()); }
    }
    #endregion

    public void InitStatisticsData()
    {

    }
}
public class StatisticsInstance
{

}



