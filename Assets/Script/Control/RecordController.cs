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

    //File.Delete(string path) 用于删除文件，未确定！！！

    public void SaveRecord(object obj, string fileName)
    {
        CreateDirectory(saveDir);
        string fullName = fileFullName.Replace("{0}", fileName);
        SetData(fullName, obj);
    }

    public List<SaveFileInstance> ReadAllRecord()
    {
        List<SaveFileInstance> recordList = new List<SaveFileInstance>();
        string[] fileNames = Directory.GetFiles(saveDir, "*.sav");
        for (int i = 0; i < fileNames.Length; i++)
        {
            SaveFileInstance saveFileInstance = (SaveFileInstance)GetData(fileNames[i], typeof(SaveFileInstance));
            recordList.Add(saveFileInstance);
        }
        return recordList;
    }

    public SaveFileInstance ReadRecord(string fileName)
    {
        string fullName = fileFullName.Replace("{0}",fileName);
        SaveFileInstance saveFileInstance = (SaveFileInstance)GetData(fullName, typeof(SaveFileInstance));
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

    /// <summary>
    /// 创建一个文件夹
    /// </summary>
    private void CreateDirectory(string fileName)
    {
        //文件夹存在则返回
        if (Directory.Exists(fileName))
            return;
        Directory.CreateDirectory(fileName);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="pObject"></param>
    private void SetData(string fileName, object pObject)
    {
        //将对象序列化为字符串
        string toSave = SerializeObject(pObject);
        //对字符串进行加密,32位加密密钥
        toSave = RijndaelEncrypt(toSave, "19861011198610121956121919570908");
        StreamWriter streamWriter = File.CreateText(fileName);
        streamWriter.Write(toSave);
        streamWriter.Close();
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="pType"></param>
    /// <returns></returns>
    private object GetData(string fileName, Type pType)
    {
        StreamReader streamReader = File.OpenText(fileName);
        string data = streamReader.ReadToEnd();
        //对数据进行解密，32位解密密钥
        data = RijndaelDecrypt(data, "19861011198610121956121919570908");
        streamReader.Close();
        return DeserializeObject(data, pType);
    }

    /// <summary>
    /// Rijndael加密算法
    /// </summary>
    /// <param name="pString">待加密的明文</param>
    /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
    /// <param name="iv">iv向量,长度为128（byte[16])</param>
    /// <returns></returns>
    private string RijndaelEncrypt(string pString, string pKey)
    {
        //密钥
        byte[] keyArray = Encoding.UTF8.GetBytes(pKey);
        //待加密明文数组
        byte[] toEncryptArray = Encoding.UTF8.GetBytes(pString);

        //Rijndael解密算法
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        //返回加密后的密文
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// ijndael解密算法
    /// </summary>
    /// <param name="pString">待解密的密文</param>
    /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
    /// <param name="iv">iv向量,长度为128（byte[16])</param>
    /// <returns></returns>
    private string RijndaelDecrypt(string pString, string pKey)
    {
        //解密密钥
        byte[] keyArray = Encoding.UTF8.GetBytes(pKey);
        //待解密密文数组
        byte[] toEncryptArray = Convert.FromBase64String(pString);

        //Rijndael解密算法
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        //返回解密后的明文
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Encoding.UTF8.GetString(resultArray);
    }

    /// <summary>
    /// 将一个对象序列化为字符串
    /// </summary>
    /// <returns>The object.</returns>
    /// <param name="pObject">对象</param>
    /// <param name="pType">对象类型</param>
    private string SerializeObject(object pObject)
    {
        //序列化后的字符串
        string serializedString = string.Empty;
        //使用Json.Net进行序列化
        serializedString = JsonConvert.SerializeObject(pObject);
        return serializedString;
    }

    /// <summary>
    /// 将一个字符串反序列化为对象
    /// </summary>
    /// <returns>The object.</returns>
    /// <param name="pString">字符串</param>
    /// <param name="pType">对象类型</param>
    private object DeserializeObject(string pString, Type pType)
    {
        //反序列化后的对象
        object deserializedObject = null;
        //使用Json.Net进行反序列化
        deserializedObject = JsonConvert.DeserializeObject(pString, pType);
        return deserializedObject;
    }

}