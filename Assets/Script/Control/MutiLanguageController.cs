using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public sealed class MutiLanguageController: Controller
{
    #region 单例
    private static MutiLanguageController Instance
    {
        get;
        set;
    }
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            InitController();
            SendMessage("RegisterController", Instance, SendMessageOptions.RequireReceiver);
        }
    }
    #endregion
    private Language language = Language.Chinese;//默认语言：中文
    private Dictionary<string, string[]> mutiLanguageDict;//存储多语言的字典，key：序号，value：文字

    public Language Language
    {
        set
        {
            language = value;
        }
    }
    protected override void InitController()
    {
        base.id = ControllerID.MutiLanguageController;
        mutiLanguageDict = new Dictionary<string, string[]>();
        InitLanguageData();
    }
    /// <summary>
    /// 初始化多语言字典
    /// </summary>
    private void InitLanguageData()
    {
        TextAsset mutiLanguageAsset = Resources.Load("Language/MutiLanguage", typeof(TextAsset)) as TextAsset;
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
    public string GetMutiLanguage(string index)
    {
        string[] languageArray = null;
        mutiLanguageDict.TryGetValue(index, out languageArray);
        if (languageArray == null)
        {
            return index;
        }
        return languageArray[(int)language];
    }
}
