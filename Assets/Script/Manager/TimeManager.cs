using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager 
{
    #region C#单例
    private static TimeManager instance = null;
    private TimeManager() { }
    public static TimeManager Instance
    {
        get { return instance ?? (instance = new TimeManager()); }
    }
    #endregion
    public void InitTimeMgr()
    {

    }

    public void Update () 
	{
		
	}
}
