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
        WriteFileName(achievementList);
    }
    public List<AchievementInstance> GetAchievementList(SymbolID symbolID)
    {
        int operatorTpye = (int)symbolID;
        return achievementList.FindAll(x => x.operatorTpye == operatorTpye);
    }
    public AchievementInstance GetAchievement(string achievementName)
    {
        return achievementList.Find(x => x.achievementName == achievementName);
    }
    public void ResetAchievement()
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            PlayerPrefs.DeleteKey(achievementList[i].achievementName);
        }
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

    private void WriteFileName(List<AchievementInstance> instanceList)
    {
        for(int i = 0; i < instanceList.Count; i++)
        {
            instanceList[i].fileName = PlayerPrefs.GetString(instanceList[i].achievementName, "");
        }
    }

}
