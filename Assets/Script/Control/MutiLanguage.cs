using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutiLanguage
{
    public static Dictionary<int, string[]> mutiLanguageDict;//存储多语言的字典，key：序号，value：文字
    /// <summary>
    /// 初始化多语言字典
    /// </summary>
    public static void InitDict()
    {
        mutiLanguageDict.Add(0, new string[] { "速算是指利用数与数之间的特殊关系进行较快的加减乘除运算", "" });
    }
}
