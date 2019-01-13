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
        InitAchievementData();
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
    }
    public static AchievementController Instance
    {
        get { return instance ?? (instance = new AchievementController()); }
    }
    #endregion

    private List<AchievementInstance> achievementList;

    private void InitAchievementData()
    {
        string data = CommonTool.GetDataFromResources("Achievement/Achievement");
        achievementList = JsonHelper.FromListJson<AchievementInstance>(data);
        WriteAllFinishTime(achievementList);
    }
    public List<AchievementInstance> GetAllAchievements()
    {
        return achievementList;
    }
    public AchievementInstance GetAchievement(string achievementName)
    {
        return achievementList.Find(x => x.achievementName == achievementName);
    }
    public List<AchievementInstance> GetAchievementUnFinish()
    {
        return achievementList.FindAll(x => x.cInstance.symbolID >= 0 && string.IsNullOrEmpty(x.finishTime));
    }
    public void ResetAllAchievement()
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            PlayerPrefs.DeleteKey(achievementList[i].achievementName);
            PlayerPrefs.DeleteKey(achievementList[i].achievementName + "Star");
            achievementList[i].star = 0;
            achievementList[i].finishTime = "";
        }
    }
    public void DeleteAchievement(string achievementName)
    {
        PlayerPrefs.DeleteKey(achievementName);
        PlayerPrefs.DeleteKey(achievementName + "Star");
        WriteFinishTime(achievementName, "", 0);
    }

    public List<string> GetAllFileNameWithAchievement()
    {
        List<string> fileNameList = new List<string>();
        for (int i = 0; i < achievementList.Count; i++)
        {
            string fileName = PlayerPrefs.GetString(achievementList[i].achievementName, "");
            if (!string.IsNullOrEmpty(fileName)) fileNameList.Add(fileName);
        }
        return fileNameList;
    }

    public void WriteFinishTime(string achievementName, string finishTime, int star)
    {
        AchievementInstance instance = achievementList.Find(x => x.achievementName == achievementName);
        if (instance != null)
        {
            instance.finishTime = finishTime;
            instance.star = star;
        } 
    }
    private void WriteAllFinishTime(List<AchievementInstance> instanceList)
    {
        for(int i = 0; i < instanceList.Count; i++)
        {
            instanceList[i].finishTime = PlayerPrefs.GetString(instanceList[i].achievementName, "");
            instanceList[i].star = PlayerPrefs.GetInt(instanceList[i].achievementName + "Star", 0);
        }
    }

}
