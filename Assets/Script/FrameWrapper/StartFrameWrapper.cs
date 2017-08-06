using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFrameWrapper : GuiFrameWrapper
{
    public static StartFrameWrapper Instance//单例
    {
        get;
        private set;
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

    public override void InitUI()
    {

    }

}
