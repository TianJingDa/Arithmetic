﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class FightController : Controller
{
    #region C#单例
    private static FightController instance = null;
    private FightController()
    {
        base.id = ControllerID.FightController;
        checkList = new List<List<int>>();
        workList = new List<List<int>>();
        dataBase = new List<DivisionDataBase>();
        InitFightData();
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
    }
    public static FightController Instance
    {
        get { return instance ?? (instance = new FightController()); }
    }
    #endregion

    private List<List<int>> workList;
    private List<List<int>> checkList;
    private List<DivisionDataBase> dataBase;

    private void InitFightData()
    {
        string data = CommonTool.GetDataFromResources("FightData/d_3_2List");
        DivisionDataBase d_3_2List = JsonUtility.FromJson<DivisionDataBase>(data);
        data = CommonTool.GetDataFromResources("FightData/d_3_3List");
        DivisionDataBase d_3_3List = JsonUtility.FromJson<DivisionDataBase>(data);
        data = CommonTool.GetDataFromResources("FightData/d_4_2List");
        DivisionDataBase d_4_2List = JsonUtility.FromJson<DivisionDataBase>(data);
        data = CommonTool.GetDataFromResources("FightData/d_4_3List");
        DivisionDataBase d_4_3List = JsonUtility.FromJson<DivisionDataBase>(data);

        dataBase.Add(d_3_2List);
        dataBase.Add(d_3_3List);
        dataBase.Add(d_4_2List);
        dataBase.Add(d_4_3List);
    }

    public void ResetList(SymbolID symbolID, DigitID digitID, OperandID operandID)
    {
        if (symbolID == SymbolID.Division)
        {
            workList.Clear();
            DivisionDataBase divisionDataBase = dataBase.Find(x => x.digitID == digitID && x.operandID == operandID);
            if (divisionDataBase != null)
            {
                List<DivisionQuestionList> qList = divisionDataBase.questionList;
                for (int i = 0; i < qList.Count; i++)
                {
                    List<int> questionList = new List<int>();
                    for (int j = 0; j < qList[i].questionList.Count; j++)
                    {
                        questionList.Add(qList[i].questionList[j]);
                    }
                    workList.Add(questionList);
                }
            }
            else
            {
                MyDebug.LogYellow("divisionDataBase == null! digitID = " + digitID + ", operandID:" + operandID);
            }
        }
        else
        {
            checkList.Clear();
        }
    }
    /// <summary>
    /// 获取试题
    /// </summary>
    /// <param name="symbolID"></param>
    /// <param name="digitID"></param>
    /// <param name="operandID"></param>
    /// <returns></returns>
    public List<int> GetQuestionInstance(SymbolID symbolID, DigitID digitID, OperandID operandID)
    {
        List<int> instance = new List<int>();
        switch (symbolID)
        {
            case SymbolID.Addition:
                instance = GetAdditionInstance(digitID, operandID);
                break;
            case SymbolID.Subtraction:
                instance = GetSubtractionInstance(digitID, operandID);
                break;
            case SymbolID.Multiplication:
                instance = GetMultiplicationInstance(digitID, operandID);
                break;
            case SymbolID.Division:
                instance = GetDivisionInstance(digitID, operandID);
                break;
        }
        return instance;
    }
    /// <summary>
    /// 加法
    /// </summary>
    /// <param name="digitID"></param>
    /// <param name="operandID"></param>
    /// <returns></returns>
    private List<int> GetAdditionInstance(DigitID digitID, OperandID operandID)
    {
        List<int> instance = null;
        int sum = 0;
        int min = (int)Mathf.Pow(10, (int)digitID + 1);
        int max = (int)Mathf.Pow(10, (int)digitID + 2);
        do
        {
            instance = GetInstance(min, max, (int)operandID);
            sum = GetSum(instance);
        }
        while (CanDividedByTen(instance) || HasInstance(instance) || IsRepeat(instance));
        checkList.Add(instance);
        instance = Shuffle(instance);
        instance.Add(sum);
        return instance;
    }
    /// <summary>
    /// 减法
    /// </summary>
    /// <param name="digitID"></param>
    /// <param name="operandID"></param>
    /// <returns></returns>
    private List<int> GetSubtractionInstance(DigitID digitID, OperandID operandID)
    {
        List<int> instance = null;
        int difference = 0;
        int min = (int)Mathf.Pow(10, (int)digitID + 1);
        int max = (int)Mathf.Pow(10, (int)digitID + 2);
        do
        {
            instance = GetInstance(min, max, (int)operandID);
        }
        while (CanDividedByTen(instance) || HasInstance(instance) || IsRepeat(instance) || !CanMinus(instance, min, out difference));
        checkList.Add(instance);
        int minuend = instance[0];
        instance.RemoveAt(0);
        instance = Shuffle(instance);
        instance.Insert(0, minuend);
        instance.Add(difference);
        return instance;
    }
    /// <summary>
    /// 乘法
    /// </summary>
    /// <param name="digitID"></param>
    /// <param name="operandID"></param>
    /// <returns></returns>
    private List<int> GetMultiplicationInstance(DigitID digitID, OperandID operandID)
    {
        List<int> instance = null;
        int product = 0;
        int min = (int)Mathf.Pow(10, (int)digitID);
        int max = (int)Mathf.Pow(10, (int)digitID + 2);
        do
        {
            instance = GetInstance(min + 1, max, (int)operandID);
            product = GetProduct(instance);
        }
        while (CanDividedByTen(instance) || HasInstance(instance) || IsRepeat(instance) || !CanMultiply(instance, min));
        checkList.Add(instance);
        instance = Shuffle(instance);
        instance.Add(product);
        return instance;
    }
    /// <summary>
    /// 除法
    /// </summary>
    /// <param name="digitID"></param>
    /// <param name="operandID"></param>
    /// <returns></returns>
    private List<int> GetDivisionInstance(DigitID digitID, OperandID operandID)
    {
        #region 原算法
        //int min = 0;
        //int max = 0;
        //if(digitID == DigitID.FourDigits && operandID == OperandID.TwoNumbers)
        //{
        //    min = 10;
        //    max = 1000;
        //}
        //else
        //{
        //    min = 1;
        //    max = 100;
        //}
        //
        //do
        //{
        //    instance = GetInstance(min + 1, max, (int)operandID);
        //}
        //while (CanDividedByTen(instance) || HasInstance(instance) || IsRepeat(instance) || !CanDevide(instance, digitID, out product));
        #endregion
        if (workList.Count <= 0) return null;
        int index = Random.Range(0, workList.Count);
        List<int> instance = workList[index];
        int product = GetProduct(instance);
        instance = Shuffle(instance);
        instance.Insert(0, product);
        workList.RemoveAt(index);
        return instance;
    }
    /// <summary>
    /// 随机生成一组数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private List<int> GetInstance(int min, int max, int number)
    {
        List<int> instance = new List<int>();
        int curInt = 0;
        while (number + 2 != 0)
        {
            curInt = Random.Range(min, max);
            instance.Add(curInt);
            number--;
        }
        instance.Sort();
        return instance;
    }
    /// <summary>
    /// 试题是否重复
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private bool HasInstance(List<int> instance)
    {
        bool has = false;
        for (int i = 0; i < checkList.Count; i++)
        {
            for (int j = 0; j < checkList[i].Count; j++)
            {
                if (checkList[i][j] != instance[j])
                {
                    has = false;
                    break;
                }
                has = true;
            }
            if (has) break;
        }
        return has;
    }
    /// <summary>
    /// 试题中是否有重复数字
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private bool IsRepeat(List<int> instance)
    {
        bool has = false;
        for (int i = 0; i < instance.Count; i++)
        {
            for (int j = i + 1; j < instance.Count; j++)
            {
                if (instance[i] == instance[j])
                {
                    has = true;
                    break;
                }
            }
        }
        return has;
    }
    /// <summary>
    /// 是否有被10整除的数
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private bool CanDividedByTen(List<int> instance)
    {
        int result = 1;
        for (int i = 0; i < instance.Count; i++)
        {
            result *= instance[i];
        }
        return result % 10 == 0;
    }
    /// <summary>
    /// 是否满足减法条件
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    private bool CanMinus(List<int> instance, int min, out int difference)
    {
        instance.Reverse();
        difference = instance[0];
        for (int i = 1; i < instance.Count; i++)
        {
            difference -= instance[i];
        }
        return (difference >= min && difference % 10 != 0);
    }
    /// <summary>
    /// 是否满足乘法条件
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    private bool CanMultiply(List<int> instance, int min)
    {
        return instance[0] > min && instance[1] > 10 * min;
    }
    /// <summary>
    /// 是否满足除法条件
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private bool CanDevide(List<int> instance, DigitID digitID, out int product)
    {
        product = 1;
        int max = (int)Mathf.Pow(10, (int)digitID + 1);
        for (int i = 0; i < instance.Count; i++)
        {
            product *= instance[i];
        }
        return product > max && product < max * 10;
    }
    /// <summary>
    /// 打乱排序
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private List<int> Shuffle(List<int> instance)
    {
        List<int> tempInstance = new List<int>(instance);
        List<int> result = new List<int>();
        int index = 0;
        while (tempInstance.Count != 0)
        {
            index = Random.Range(0, tempInstance.Count);
            result.Add(tempInstance[index]);
            tempInstance.RemoveAt(index);
        }
        return result;
    }
    /// <summary>
    /// 求和
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private int GetSum(List<int> instance)
    {
        int sum = 0;
        for (int i = 0; i < instance.Count; i++)
        {
            sum += instance[i];
        }
        return sum;
    }
    /// <summary>
    /// 求积
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private int GetProduct(List<int> instance)
    {
        int product = 1;
        for (int i = 0; i < instance.Count; i++)
        {
            product *= instance[i];
        }
        return product;
    }
}
[System.Serializable]
public class DivisionDataBase
{
    public DigitID digitID;
    public OperandID operandID;
    public List<DivisionQuestionList> questionList;

    public DivisionDataBase()
    {
        questionList = new List<DivisionQuestionList>();
    }
}
[System.Serializable]
public class DivisionQuestionList
{
    public List<int> questionList;

    public DivisionQuestionList()
    {
        questionList = new List<int>();
    }
}

