﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// 排行榜界面
/// </summary>
public class RankFrameWrapper : GuiFrameWrapper
{
	private int delta;
	private Dictionary<int, string[]> amountDropdownTextDict;
	private List<Dropdown.OptionData> digitDropdownOptionsList;

	private PatternID   curPatternID;
	private AmountID    curAmountID;
	private SymbolID    curSymbolID;
	private DigitID     curDigitID;
	private OperandID   curOperandID;

	private GameObject  rankDataContent;
    private RectTransform rankDataGrid;
    private Dropdown    amountDropdown;
	private Dropdown    digitDropdown;

	void Start () 
	{
		id = GuiFrameID.RankFrame;
		Init();
		delta = 0;
		amountDropdownTextDict = new Dictionary<int, string[]>();
		amountDropdownTextDict.Add(0, new string[] { "Text_30013", "Text_30014", "Text_30015" });
		amountDropdownTextDict.Add(1, new string[] { "Text_30016", "Text_30017", "Text_30018" });
		digitDropdownOptionsList = new List<Dropdown.OptionData>(digitDropdown.options);
		RefreshAllDropdown();
	}

	protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
	{
		rankDataContent = gameObjectDict["RankDataContent"];

        rankDataGrid    = gameObjectDict["RankDataGrid"].GetComponent<RectTransform>();
        digitDropdown   = gameObjectDict["DigitDropdown"].GetComponent<Dropdown>();
		amountDropdown  = gameObjectDict["AmountDropdown"].GetComponent<Dropdown>();
	}

	protected override void OnButtonClick(Button btn)
	{
		base.OnButtonClick(btn);
		switch (btn.name)
		{
			case "Rank2StartFrameBtn":
			case "RankData2StartFrameBtn":
				GameManager.Instance.SwitchWrapperWithScale(GuiFrameID.StartFrame, false);
				break;
			case "RankDataBtn":
				CategoryInstance instance = new CategoryInstance(curPatternID, curAmountID, curSymbolID, curDigitID, curOperandID);
				GameManager.Instance.DownloadData(instance, OnDownloadSucceed, OnDownloadFail);
				break;
			case "RankData2RankFrameBtn":
				CommonTool.GuiHorizontalMove(rankDataContent, Screen.width, MoveID.RightOrUp, canvasGroup, false);
				break;
			default:
				MyDebug.LogYellow("Can not find Button: " + btn.name);
				break;
		}
	}

	protected override void OnDropdownClick(Dropdown dpd)
	{
		base.OnDropdownClick(dpd);
		switch (dpd.name)
		{
			case "PatternDropdown":
				curPatternID = (PatternID)dpd.value;
				RefreshAmountDropdown(dpd.value);
				break;
			case "AmountDropdown":
				curAmountID = (AmountID)dpd.value;
				break;
			case "SymbolDropdown":
				curSymbolID = (SymbolID)dpd.value;
				RefreshDigitDropdown(dpd.value);
				break;
			case "DigitDropdown":
				curDigitID = (DigitID)(dpd.value + delta);
				break;
			case "OperandDropdown":
				curOperandID = (OperandID)dpd.value;
				break;
			default:
				MyDebug.LogYellow("Can not find Dropdown: " + dpd.name);
				break;
		}
	}

	/// <summary>
	/// 刷新Dropdown的状态
	/// </summary>
	/// <param name="dpd"></param>
	/// <param name="index"></param>
	private void RefreshAllDropdown()
	{
		Dropdown[] dropdownArray = GetComponentsInChildren<Dropdown>(true);
		for(int i = 0; i < dropdownArray.Length; i++)
		{
			for (int j = 0; j < dropdownArray[i].options.Count; j++)
			{
				dropdownArray[i].options[j].text = GameManager.Instance.GetMutiLanguage(dropdownArray[i].options[j].text);
			}
		}
	}
	private void RefreshAmountDropdown(int index)
	{
		for (int i = 0; i < amountDropdown.options.Count; i++)
		{
			amountDropdown.options[i].text = GameManager.Instance.GetMutiLanguage(amountDropdownTextDict[index][i]);
		}
		amountDropdown.value = 0;
		amountDropdown.RefreshShownValue();
	}
	private void RefreshDigitDropdown(int index)
	{
		switch (index)
		{
			case 0:
			case 1:
				digitDropdown.options = digitDropdownOptionsList;
				delta = 0;
				break;
			case 2:
				digitDropdown.options = digitDropdownOptionsList.GetRange(0, 2);
				delta = 0;
				break;
			case 3:
				digitDropdown.options = digitDropdownOptionsList.GetRange(1, 2);
				delta = 1;
				break;
		}
		digitDropdown.value = 0;
		digitDropdown.RefreshShownValue();
		OnDropdownClick(digitDropdown);
	}

    private void OnDownloadSucceed(ArrayList dataList)
    {
        rankDataContent.SetActive(true);
        CommonTool.RefreshScrollContent(rankDataGrid, dataList, GuiItemID.RankItem);
        CommonTool.GuiHorizontalMove(rankDataContent, Screen.width, MoveID.RightOrUp, canvasGroup, true);
    }

	private void OnDownloadFail(string message)
	{
		GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
		GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
	}
}