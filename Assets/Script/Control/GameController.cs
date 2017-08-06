using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏控制层
/// </summary>
public class GameController : MonoBehaviour
{
    public Language language = Language.Chinese;//默认语言：中文
    public Dictionary<GuiFrameID, GuiFrame> guiFrameDict = new Dictionary<GuiFrameID, GuiFrame>();//key：GuiFrameID，value：Frame
    public Dictionary<GuiFrameID, GuiFrameWrapper> guiFrameWrapperDict = new Dictionary<GuiFrameID, GuiFrameWrapper>();//key：GuiFrameID，value：FrameWrapper

    public static GameController Instance//单例
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
        MutiLanguage.InitDict();
        foreach (int key in MutiLanguage.mutiLanguageDict.Keys)
        {
            Debug.Log("KEY:" + key.ToString() + ",CONTENT:" + MutiLanguage.mutiLanguageDict[key][(int)Instance.language]);
        }
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
