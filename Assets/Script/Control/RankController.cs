using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankController : Controller
{
	#region C#单例
	private static RankController instance = null;
	private RankController()
	{
		base.id = ControllerID.RankController;
		InitRankData();
		MyDebug.LogWhite("Loading Controller:" + id.ToString());
	}
	public static RankController Instance
	{
		get { return instance ?? (instance = new RankController()); }
	}
    #endregion

    private const float TimeOut = 1f;
    private const string DownloadURL = "";
    private const string UploadURL = "";

    private void InitRankData()
	{
		
	}

    /// <summary>
    /// 拉取排行榜信息
    /// </summary>
    /// <param name="form"></param>
    /// <param name="OnSucceed"></param>
    /// <returns></returns>
    public IEnumerator DownloadData(WWWForm form, Action<ArrayList> OnSucceed)
    {
        WWW www = new WWW(DownloadURL, form);

        float responseTime = 0;
        while (!www.isDone && responseTime < TimeOut)
        {
            responseTime += Time.deltaTime;
            yield return www;
        }

        string message = "";
        if (www.isDone)
        {
            DownloadDataResponse response = JsonUtility.FromJson<DownloadDataResponse>(www.text);
            if (response != null)
            {
                if (response.error == 0)
                {
                    MyDebug.LogGreen("Get Rank Data Succeed!");
                    if (OnSucceed != null)
                    {
                        ArrayList dataList = new ArrayList(response.instances);
                        OnSucceed(dataList);
                    }
                    yield break;
                }
                else
                {
                    MyDebug.LogYellow("Get Rank Data Fail:" + response.error);
                    message = GameManager.Instance.GetMutiLanguage("Text_20066");
                }
            }
            else
            {
                MyDebug.LogYellow("Get Rank Data: Message Is Not Response!");
                message = GameManager.Instance.GetMutiLanguage("Text_20066");
            }
        }
        else
        {
            MyDebug.LogYellow("Get Rank Data Fail: Long Time!");
            message = GameManager.Instance.GetMutiLanguage("Text_20067");
        }
        GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
        GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
    }

    /// <summary>
    /// 上传排行榜信息
    /// </summary>
    /// <returns></returns>
    public IEnumerator UploadData(WWWForm form, Action OnSucceed)
    {
        WWW www = new WWW(UploadURL, form);

        float responseTime = 0;
        while (!www.isDone && responseTime < TimeOut)
        {
            responseTime += Time.deltaTime;
            yield return www;
        }

        string message = "";
        if (www.isDone)
        {
            UploadDataResponse response = JsonUtility.FromJson<UploadDataResponse>(www.text);
            if (response != null)
            {
                if (response.error == 0)
                {
                    MyDebug.LogGreen("Upload Rank Data Succeed!");
                    message = GameManager.Instance.GetMutiLanguage("Text_20068");
                    message = string.Format(message, response.index);
                    if (OnSucceed != null)
                    {
                        OnSucceed();
                    }
                }
                else if (response.error == 1)
                {
                    MyDebug.LogYellow("Upload Rank Data Fail:" + response.error);
                    message = GameManager.Instance.GetMutiLanguage("Text_20069");
                }
                else if (response.error == 2)
                {
                    MyDebug.LogYellow("Upload Rank Data Fail:" + response.error);
                    message = GameManager.Instance.GetMutiLanguage("Text_20070");
                }
                else
                {
                    MyDebug.LogYellow("Upload Rank Data Fail:" + response.error);
                    message = GameManager.Instance.GetMutiLanguage("Text_20066");
                }
            }
            else
            {
                MyDebug.LogYellow("Upload Rank Data: Message Is Not Response!");
                message = GameManager.Instance.GetMutiLanguage("Text_20066");
            }
        }
        else
        {
            MyDebug.LogYellow("Upload Rank Data Fail: Long Time!");
            message = GameManager.Instance.GetMutiLanguage("Text_20067");
        }
        GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
        GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
    }

    [Serializable]
    private class DownloadDataResponse
    {
        public int error;
        public List<RankInstance> instances;
    }

    [Serializable]
    private class UploadDataResponse
    {
        public int error;//0:成功,1:重复上传,2:未上榜,3:上传失败
        public int index;
    }
}
