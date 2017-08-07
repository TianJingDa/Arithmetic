using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MutiLanguageController
{
    #region C#单例
    private static MutiLanguageController instance = null;
    private MutiLanguageController() { }
    public static MutiLanguageController Instance
    {
        get { return instance ?? (instance = new MutiLanguageController()); }
    }
    #endregion
    private Language language = Language.Chinese;//默认语言：中文
    private Dictionary<int, string[]> mutiLanguageDict = new Dictionary<int, string[]>();//存储多语言的字典，key：序号，value：文字

    public Language Language
    {
        set
        {
            language = value;
        }
    }
    /// <summary>
    /// 初始化多语言字典
    /// </summary>
    public void InitLanguageDict()
    {

    }
    public string GetMutiLanguage(int index)
    {
        string language = null;
        return language;
    }
}
