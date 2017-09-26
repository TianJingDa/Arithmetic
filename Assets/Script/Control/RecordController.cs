using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

public sealed class RecordController : Controller 
{
    #region C#单例
    private static RecordController instance = null;
    private RecordController()
    {
        base.id = ControllerID.RecordController;
        InitRecordData();
        Debug.Log("Loading Controller:" + id.ToString());
    }
    public static RecordController Instance
    {
        get { return instance ?? (instance = new RecordController()); }
    }
    #endregion

    private string saveDir;
    private string fileFullName;

    private void InitRecordData()
    {
        saveDir = Application.persistentDataPath + "/Save";
        fileFullName = saveDir + "/{0}.sav";
    }

    public void SaveRecord(object obj, string fileName)
    {
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
        string fullName = fileFullName.Replace("{0}", fileName);
        IOHelper.SetData(fullName, obj);
    }

    public List<SaveFileInstance> ReadAllRecord()
    {
        List<SaveFileInstance> recordList = new List<SaveFileInstance>();
        string[] fileNames = Directory.GetFiles(saveDir, "*.sav");
        for (int i = 0; i < fileNames.Length; i++)
        {
            SaveFileInstance saveFileInstance = (SaveFileInstance)IOHelper.GetData(fileNames[i], typeof(SaveFileInstance));
            recordList.Add(saveFileInstance);
        }
        return recordList;
    }

    public SaveFileInstance ReadRecord(string fileName)
    {
        string fullName = fileFullName.Replace("{0}",fileName);
        SaveFileInstance saveFileInstance = (SaveFileInstance)IOHelper.GetData(fullName, typeof(SaveFileInstance));
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

    public bool DeleteRecord(string fileName)
    {
        string fullName = fileFullName.Replace("{0}", fileName);
        if (File.Exists(fullName))
        {
            File.Delete(fullName);
            return !File.Exists(fullName);
        }
        else
        {
            MyDebug.LogYellow("The file does not exist!!!");
            return false;
        }
    }
}
