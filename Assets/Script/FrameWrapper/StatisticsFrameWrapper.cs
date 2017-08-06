using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsFrameWrapper : GuiFrameWrapper
{
    private static StatisticsFrameWrapper Instance//单例
    {
        get;
        set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GameController.Instance.guiFrameWrapperDict.Add(GuiFrameID.CategoryFrame, Instance);
    }

    void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

    void OnDestroy()
    {
        
    }

    public override void InitUI()
    {

    }

}
