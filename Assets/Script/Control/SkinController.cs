using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : Controller 
{
    #region C#单例
    private static SkinController instance = null;
    private SkinController()
    {
        base.id = ControllerID.SkinController;
        skinDict = new Dictionary<string, string[]>();
        InitSkinData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static SkinController Instance
    {
        get { return instance ?? (instance = new SkinController()); }
    }
    #endregion
    private Dictionary<string, string[]> skinDict;  //存储多语言的字典，key：序号，value：文字
    private void InitSkinData()
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
            skinDict.Add(lineList[0], lineList.GetRange(1, lineList.Count - 1).ToArray());
        }
    }
}
