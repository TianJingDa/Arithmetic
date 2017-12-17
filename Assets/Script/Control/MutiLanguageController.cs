using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public sealed class MutiLanguageController: Controller
{
    #region C#单例
    private static MutiLanguageController instance = null;
    private MutiLanguageController()
    {
        base.id = ControllerID.MutiLanguageController;
        InitLanguageData();
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
    }
    public static MutiLanguageController Instance
    {
        get{ return instance ?? (instance = new MutiLanguageController()); }
    }
    #endregion
    private Dictionary<string, string[]> mutiLanguageDict;  //存储多语言的字典，key：序号，value：文字

    /// <summary>
    /// 初始化多语言字典
    /// </summary>
    private void InitLanguageData()
    {
        string path = "Language/MutiLanguage";
        mutiLanguageDict = (Dictionary<string, string[]>)IOHelper.GetDataFromResources(path, typeof(Dictionary<string, string[]>));
    }
    /// <summary>
    /// 获取多语言
    /// </summary>
    /// <param name="index">序号</param>
    /// <returns>内容</returns>
    public string GetMutiLanguage(string index,LanguageID language)
    {
        string[] languageArray = null;
        mutiLanguageDict.TryGetValue(index, out languageArray);
        if (languageArray == null)
        {
            return index;
        }
        else
        {
            string text = languageArray[(int)language];
            if (text.Contains("\\n")) text = text.Replace("\\n", "\n");
            return text;
        }
    }
}
