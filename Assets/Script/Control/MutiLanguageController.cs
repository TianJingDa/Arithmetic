using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public sealed class MutiLanguageController
{
    #region C#单例
    private static MutiLanguageController instance = null;
    private MutiLanguageController()
    {
        mutiLanguageDict = new Dictionary<string, string[]>();
    }
    public static MutiLanguageController Instance
    {
        get { return instance ?? (instance = new MutiLanguageController()); }
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
    /// <summary>
    /// 初始化多语言字典
    /// </summary>
    public void InitLanguageDict()
    {
        List<List<string>> dataList = LoadFile(Application.dataPath + "/Document", "MutiLanguage.csv");
        if(dataList.Count==0|| dataList == null)
        {
            Debug.Log("Load File Error!");
            return;
        }
        foreach (List<string> strArray in dataList)
        {
            mutiLanguageDict.Add(strArray[0], strArray.GetRange(1, strArray.Count - 1).ToArray());
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
        return languageArray[(int)language];
    }
    /// <summary>
    /// 加载多语言文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="fileName">文件名</param>
    private List<List<string>> LoadFile(string path,string fileName)
    {
        List<List<string>> dataList = new List<List<string>>();
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + fileName);
        }
        catch
        {
            Debug.Log("File can not be found:" + fileName);
            return null;
        }
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            dataList.Add(new List<string>(line.Split(',')));
        }
        sr.Close();
        sr.Dispose();
        return dataList;
    }
}
