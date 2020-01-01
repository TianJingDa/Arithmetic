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

	private const float RefreshInterval = 60f;
    private const float TimeOut = 1f;
    private const string DownloadURL = "";
    private const string UploadURL = "";

	private Dictionary<CategoryInstance, DateTime> lastRefreshTimeDict;
	private Dictionary<CategoryInstance, List<RankInstance>> rankDataDict;


    private void InitRankData()
	{
		lastRefreshTimeDict = new Dictionary<CategoryInstance, DateTime>();
		rankDataDict = new Dictionary<CategoryInstance, List<RankInstance>>();
	}

    /// <summary>
    /// 拉取排行榜信息
    /// </summary>
    /// <param name="form"></param>
    /// <param name="OnSucceed"></param>
    /// <returns></returns>
	public IEnumerator DownloadData(CategoryInstance instance, Action<ArrayList> OnSucceed, Action<string> OnFail)
    {
		if(!CanRefreshRankData(instance))
		{
			List<RankInstance> instances;
			rankDataDict.TryGetValue(instance, out instances);
			if(instances != null && instances.Count > 0)
			{
				if(OnSucceed != null)
				{
					ArrayList dataList = new ArrayList(instances);
					OnSucceed(dataList);
				}
			}
			else
			{
				if(OnFail != null)
				{
					string msg = GameManager.Instance.GetMutiLanguage("Text_20071");
					OnFail(msg);
				}
			}
			yield break;
		}

		WWWForm form = new WWWForm();
		form.AddField("pattern", (int)instance.patternID);
		form.AddField("amount", (int)instance.amountID);
		form.AddField("symbol", (int)instance.symbolID);
		form.AddField("digit", (int)instance.digitID);
		form.AddField("operand", (int)instance.operandID);

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
                    MyDebug.LogGreen("Download Rank Data Succeed!");
					lastRefreshTimeDict[instance] = DateTime.Now;
					if(response.instances != null && response.instances.Count > 0)
					{
						rankDataDict[instance] = response.instances;
						if(OnSucceed != null)
						{
							ArrayList dataList = new ArrayList(response.instances);
							OnSucceed(dataList);
						}
						yield break;
					}
					else
					{
						message = GameManager.Instance.GetMutiLanguage("Text_20071");
					}
                }
                else
                {
					MyDebug.LogYellow("Download Rank Data Fail:" + response.error);
                    message = GameManager.Instance.GetMutiLanguage("Text_20066");
                }
            }
            else
            {
				MyDebug.LogYellow("Download Rank Data Fail: Message Is Not Response!");
                message = GameManager.Instance.GetMutiLanguage("Text_20066");
            }
        }
        else
        {
			MyDebug.LogYellow("Download Rank Data Fail: Long Time!");
            message = GameManager.Instance.GetMutiLanguage("Text_20067");
        }

		if(OnFail != null)
		{
			OnFail(message);
		}
    }

	private bool CanRefreshRankData(CategoryInstance instance)
	{
		DateTime lastTime;
		bool hasLastTime = lastRefreshTimeDict.TryGetValue(instance, out lastTime);
		if(hasLastTime)
		{
			TimeSpan ts = DateTime.Now - lastTime;
			return ts.TotalSeconds > RefreshInterval;
		}

		return true;
	}

    /// <summary>
    /// 上传排行榜信息
    /// </summary>
    /// <returns></returns>
	public IEnumerator UploadData(WWWForm form, Action<string> OnFinished)
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
                MyDebug.LogYellow("Upload Rank Data Fail: Message Is Not Response!");
                message = GameManager.Instance.GetMutiLanguage("Text_20066");
            }
        }
        else
        {
            MyDebug.LogYellow("Upload Rank Data Fail: Long Time!");
            message = GameManager.Instance.GetMutiLanguage("Text_20067");
        }

		if(OnFinished != null)
		{
			OnFinished(message);
		}     
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
