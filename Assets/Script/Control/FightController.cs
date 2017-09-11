using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FightController : Controller
{
    #region C#单例
    private static FightController instance = null;
    private FightController()
    {
        base.id = ControllerID.FightController;
        InitFightData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static FightController Instance
    {
        get { return instance ?? (instance = new FightController()); }
    }
    #endregion

    private List<List<int>> checkList;

    private void InitFightData()
    {

    }

    public void ResetCheckList()
    {
        checkList = new List<List<int>>();
    }

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
        int min = (int)Mathf.Pow(10, (int)digitID + 1);
        int max = (int)Mathf.Pow(10, (int)digitID + 2);
        instance = GetAdditionInstance(min, max, (int)operandID);
        while (CanDividedByTen(instance) || HasInstance(instance))
        {
            instance = GetAdditionInstance(min, max, (int)operandID);
        }
        checkList.Add(instance);
        return RandomList(instance);
    }
    private List<int> GetAdditionInstance(int min, int max, int number)
    {
        List<int> instance = new List<int>();
        int deltaInt = number + 1;
        int curInt = 0;
        int lastInt = min;
        while (number + 2 != 0)
        {
            curInt = Random.Range(lastInt + 1, max - deltaInt);
            instance.Add(curInt);
            lastInt = curInt;
            deltaInt--;
            number--;
        }
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

        checkList.Add(instance);
        return RandomList(instance);
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

        checkList.Add(instance);
        return RandomList(instance);
    }
    /// <summary>
    /// 除法
    /// </summary>
    /// <param name="digitID"></param>
    /// <param name="operandID"></param>
    /// <returns></returns>
    private List<int> GetDivisionInstance(DigitID digitID, OperandID operandID)
    {
        List<int> instance = null;

        checkList.Add(instance);
        return RandomList(instance);
    }
    /// <summary>
    /// 是否重复
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
    /// 打乱排序
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private List<int> RandomList(List<int> instance)
    {
        return instance;
    }
}

