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

	private void InitRankData()
	{
		
	}

}
