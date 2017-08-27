using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock 
{
    private Text timeLabel;
	public Clock(Text timeLabel)
    {
        this.timeLabel = timeLabel;
    }

	public void Update () 
	{
		
	}
}
[Serializable]
public class SaveFileInstance
{
    public string title;
}
[Serializable]
public class AchievementInstance
{
    public string title;
    public string detail;
}
[Serializable]
public class QuentionInstance
{
    public string title;
}



