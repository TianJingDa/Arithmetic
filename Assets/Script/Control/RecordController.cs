using System.Collections.Generic;
using UnityEngine;
using System.IO;

public sealed class RecordController : Controller 
{
    #region C#单例
    private static RecordController instance = null;
    private RecordController()
    {
        base.id = ControllerID.RecordController;
        saveDir = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
        fileFullName = saveDir + "/{0}.sav";
        InitRecordData();
        MyDebug.LogWhite("Loading Controller:" + id.ToString());
    }
    public static RecordController Instance
    {
        get { return instance ?? (instance = new RecordController()); }
    }
    #endregion

    private readonly string saveDir;
    private readonly string fileFullName;

    private void InitRecordData()
    {

    }

    public void SaveRecord(string toSave, string fileName)
    {
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
        string fullName = string.Format(fileFullName, fileName);
        CommonTool.SetData(fullName, toSave);
    }

    public List<SaveFileInstance> ReadAllRecords()
    {
        List<SaveFileInstance> recordList = new List<SaveFileInstance>();
        string[] fileNames = Directory.GetFiles(saveDir, "*.sav");
        string data;
        for (int i = 0; i < fileNames.Length; i++)
        {
            data = CommonTool.GetDataFromDataPath(fileNames[i]);
            if (string.IsNullOrEmpty(data)) continue;
            SaveFileInstance saveFileInstance = JsonUtility.FromJson<SaveFileInstance>(data);
            recordList.Add(saveFileInstance);
        }
        return recordList;
    }

    public SaveFileInstance ReadRecord(string fileName)
    {
        string fullName = string.Format(fileFullName, fileName);
        string data = CommonTool.GetDataFromDataPath(fullName);
        SaveFileInstance saveFileInstance = JsonUtility.FromJson<SaveFileInstance>(data);
        return saveFileInstance;
    }

    public void DeleteAllRecord()
    {
        string[] fileNames = Directory.GetFiles(saveDir, "*.sav");
        for(int i = 0; i < fileNames.Length; i++)
        {
            File.Delete(fileNames[i]);
        }
    }
    public void DeleteRecordWithoutAchievement(List<string> fileNameList)
    {
        string[] fileNames = Directory.GetFiles(saveDir, "*.sav");
        for (int i = 0; i < fileNames.Length; i++)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileNames[i]);
            if (fileNameList.Contains(fileName)) continue;
            File.Delete(fileNames[i]);
        }
    }
    public void DeleteRecordWithAchievement(List<string> fileNameList)
    {
        string[] fileNames = Directory.GetFiles(saveDir, "*.sav");
        for (int i = 0; i < fileNames.Length; i++)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileNames[i]);
            if (fileNameList.Contains(fileName)) File.Delete(fileNames[i]);
        }
    }


    public bool DeleteRecord(string fileName)
    {
        string fullName = string.Format(fileFullName, fileName);
        if (File.Exists(fullName))
        {
            File.Delete(fullName);
            MyDebug.LogGreen("Delete:" + fileName);
            return !File.Exists(fullName);
        }
        else
        {
            MyDebug.LogYellow("The file does not exist!!!");
            return false;
        }
    }
}
