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
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static AchievementController Instance
    {
        get { return instance ?? (instance = new AchievementController()); }
    }
    #endregion

    private List<AchievementInstance> achievementList;

    private void InitAchievementData()
    {
        achievementList = (List<AchievementInstance>)IOHelper.GetData(Application.dataPath + "/Resources/Achievement/Achievement.ach", typeof(List<AchievementInstance>));
        WriteAllFileName(achievementList);
    }
    public List<AchievementInstance> GetAllAchievements()
    {
        return achievementList;
    }
    public AchievementInstance GetAchievement(string achievementName)
    {
        return achievementList.Find(x => x.achievementName == achievementName);
    }
    public List<AchievementInstance> GetAchievementWithoutFileName()
    {
        return achievementList.FindAll(x => x.cInstance.symbolID >= 0 && string.IsNullOrEmpty(x.fileName));
    }
    public void ResetAllAchievement()
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            PlayerPrefs.DeleteKey(achievementList[i].achievementName);
            achievementList[i].fileName = "";
        }
    }
    public void DeleteAchievement(string achievementName)
    {
        PlayerPrefs.DeleteKey(achievementName);
        WriteFileName(achievementName);
    }
    public List<string> GetAllFileNameWithAchievement()
    {
        List<string> fileNameList = new List<string>();
        for(int i = 0;i< achievementList.Count; i++)
        {
            string fileName = PlayerPrefs.GetString(achievementList[i].achievementName, "");
            if (!string.IsNullOrEmpty(fileName)) fileNameList.Add(fileName);
        }
        return fileNameList;
    }
    public void WriteFileName(string achievementName, string fileName = "")
    {
        AchievementInstance instance = achievementList.Find(x => x.achievementName == achievementName);
        if (instance != null) instance.fileName = fileName;
    }
    private void WriteAllFileName(List<AchievementInstance> instanceList)
    {
        for(int i = 0; i < instanceList.Count; i++)
        {
            instanceList[i].fileName = PlayerPrefs.GetString(instanceList[i].achievementName, "");
        }
    }

}
