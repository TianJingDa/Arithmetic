using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementController : Controller 
{
    #region C#单例
    private static AchievementController instance = null;
    private AchievementController()
    {
        base.id = ControllerID.AchievementController;
        achievementDict = new Dictionary<SymbolID, List<AchievementInstance>>();
        InitAchievementData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static AchievementController Instance
    {
        get { return instance ?? (instance = new AchievementController()); }
    }
    #endregion

    private Dictionary<SymbolID, List<AchievementInstance>> achievementDict;

    private void InitAchievementData()
    {
        List<AchievementInstance> addition          = (List<AchievementInstance>)IOHelper.GetData(Application.dataPath + "/Resources/Achievement/AdditionAchievement.ach", typeof(List<AchievementInstance>));
        List<AchievementInstance> subtraction       = (List<AchievementInstance>)IOHelper.GetData(Application.dataPath + "/Resources/Achievement/SubtractionAchievement.ach", typeof(List<AchievementInstance>));
        List<AchievementInstance> multiplication    = (List<AchievementInstance>)IOHelper.GetData(Application.dataPath + "/Resources/Achievement/MultiplicationAchievement.ach", typeof(List<AchievementInstance>));
        List<AchievementInstance> division          = (List<AchievementInstance>)IOHelper.GetData(Application.dataPath + "/Resources/Achievement/DivisionAchievement.ach", typeof(List<AchievementInstance>));
        achievementDict.Add(SymbolID.Addition, addition);
        achievementDict.Add(SymbolID.Subtraction, subtraction);
        achievementDict.Add(SymbolID.Multiplication, multiplication);
        achievementDict.Add(SymbolID.Division, division);
    }
    public List<AchievementInstance> GetAchievementList(SymbolID symbolID)
    {
        return achievementDict[symbolID];
    }
    public Dictionary<SymbolID, List<AchievementInstance>> GetAchievementDict()
    {
        return achievementDict;
    }

}
