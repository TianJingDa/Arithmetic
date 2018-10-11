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
        mutiLanguageDict = new Dictionary<string, string[]>();
        TextAsset mutiLanguageAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
        if (mutiLanguageAsset == null)
        {
            Debug.Log("Load File Error!");
            return;
        }
        char[] charSeparators = new char[] { "\r"[0], "\n"[0] };
        string[] lineArray = mutiLanguageAsset.text.Split(charSeparators, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> lineList;
        for (int i = 0; i < lineArray.Length; i++)
        {
            lineList = new List<string>(lineArray[i].Split(','));
            mutiLanguageDict.Add(lineList[0], lineList.GetRange(1, lineList.Count - 1).ToArray());
        }
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
            if (text.Contains("\\u3000")) text = text.Replace("\\u3000", "\u3000");
            return text;
        }
    }
}
