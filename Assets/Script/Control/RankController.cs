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
	private const string DownloadURL = "http://182.92.68.73:8091/getData";
	private const string UploadURL = "http://182.92.68.73:8091/setData";
	private const string DetailURL = "http://182.92.68.73:8091/getDetail";

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
	public IEnumerator DownloadRecord(CategoryInstance instance, Action<ArrayList> OnSucceed, Action<string> OnFail)
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
		form.AddField("userId", GameManager.Instance.UserID);
		form.AddField("jwttoken", GameManager.Instance.Token);
		form.AddField("model", (int)instance.patternID + 1);
		form.AddField("num", (int)instance.amountID + 1);
		form.AddField("calcu", (int)instance.symbolID + 1);
		form.AddField("digit", (int)instance.digitID + 2);
		form.AddField("operate", (int)instance.operandID + 2);

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
				if (response.code == 200)
                {
                    MyDebug.LogGreen("Download Rank Data Succeed!");
					lastRefreshTimeDict[instance] = DateTime.Now;
					if(response.data != null && response.data.Count > 0)
					{
						rankDataDict[instance] = response.data;
						if(OnSucceed != null)
						{
							ArrayList dataList = new ArrayList(response.data);
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
					MyDebug.LogYellow("Download Rank Data Fail:" + response.code);
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
	public IEnumerator UploadRecord(WWWForm form, Action<string> OnSuccees, Action<string> OnFail)
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
				if (response.code == 200)
                {
                    MyDebug.LogGreen("Upload Rank Data Succeed!");
                    message = GameManager.Instance.GetMutiLanguage("Text_20068");
					message = string.Format(message, response.data.rank);
                    if (OnSuccees != null)
                    {
                        OnSuccees(message);
                    }
                    yield break;
                }
                else
                {
					MyDebug.LogYellow("Upload Rank Data Fail:" + response.code);
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

		if(OnFail != null)
		{
            OnFail(message);
		}     
    }

	public IEnumerator GetRankDetail(WWWForm form, Action<string> OnSucceed, Action<string> OnFail)
	{
		WWW www = new WWW(DetailURL, form);

		float responseTime = 0;
		while (!www.isDone && responseTime < TimeOut)
		{
			responseTime += Time.deltaTime;
			yield return www;
		}

		string message = "";
		if (www.isDone)
		{
			GetDetailResponse response = JsonUtility.FromJson<GetDetailResponse>(www.text);
			if (response != null)
			{
				if (response.code == 200)
				{
					MyDebug.LogGreen("Get Rank Detail Succeed!");
					if(OnSucceed != null)
					{
						OnSucceed(response.data);
					}
                    yield break;
				}
				else
				{
					MyDebug.LogYellow("Get Rank Detail Fail:" + response.code);
					message = GameManager.Instance.GetMutiLanguage("Text_20066");
				}
			}
			else
			{
				MyDebug.LogYellow("Get Rank Detail Fail: Message Is Not Response!");
				message = GameManager.Instance.GetMutiLanguage("Text_20066");
			}
		}
		else
		{
			MyDebug.LogYellow("Get Rank Detail Fail: Long Time!");
			message = GameManager.Instance.GetMutiLanguage("Text_20067");
		}

		if(OnFail != null)
		{
			OnFail(message);
		}     
	}

    [Serializable]
    private class DownloadDataResponse
    {
		public int code;//200:成功
		public string errmsg;
		public List<RankInstance> data;
    }

    [Serializable]
    private class UploadDataResponse
    {
		public int code;//200:成功
		public string errmsg;
		public UploadData data;
    }

	[Serializable]
	private class UploadData
	{
		public int rank;
	}

	[Serializable]
	private class GetDetailResponse
	{
		public int code;//200:成功
		public string errmsg;
		public string data;
	}
}
