﻿using System.Collections;
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
        saveDir = Application.persistentDataPath + "/Save";
        fileFullName = saveDir + "/{0}.sav";
        InitRecordData();
        Debug.Log("Loading Controller:" + id.ToString());
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

    public void SaveRecord(object obj, string fileName)
    {
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
        string fullName = string.Format(fileFullName, fileName);
        IOHelper.SetData(fullName, obj);
    }

    public List<SaveFileInstance> ReadAllRecords()
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
        string fullName = string.Format(fileFullName, fileName);
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
