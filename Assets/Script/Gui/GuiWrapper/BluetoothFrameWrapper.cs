using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothFrameWrapper : GuiFrameWrapper
{
	private bool   isCentral;
	private Dictionary<string,string> peripheralDict;

	private PatternID   curPatternID;
	private AmountID    curAmountID;
	private SymbolID    curSymbolID;
	private DigitID     curDigitID;
	private OperandID   curOperandID;

	private GameObject  bluetoothPeripheralBtn;
    private GameObject  bluetoothAndroidTip;
    private GameObject  bluetoothRoleContent;
    private GameObject  bluetoothCategoryContent;
	private GameObject  bluetoothScanResultContent;
	private GameObject	bluetoothPeripheralScanTip;
	private GameObject 	bluetoothPeripheralScrollRect;

    void Start ()
    {
        id = GuiFrameID.BluetoothFrame;
        Init();
		curPatternID = PatternID.Number;
		curOperandID = OperandID.TwoNumbers;
		peripheralDict = new Dictionary<string, string> ();

#if UNITY_ANDROID
		bluetoothPeripheralBtn.SetActive(false);
        bluetoothAndroidTip.SetActive(true);
#elif UNITY_IOS
        bluetoothPeripheralBtn.SetActive(true);
        bluetoothAndroidTip.SetActive(false);
#endif
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        bluetoothPeripheralBtn        	= gameObjectDict["BluetoothAndroidTip"];
        bluetoothAndroidTip         	= gameObjectDict["BluetoothAndroidTip"];
        bluetoothRoleContent        	= gameObjectDict["BluetoothRoleContent"];
		bluetoothCategoryContent    	= gameObjectDict["BluetoothCategoryContent"];
		bluetoothScanResultContent    	= gameObjectDict["BluetoothScanResultContent"];
		bluetoothPeripheralScanTip      = gameObjectDict["BluetoothPeripheralScanTip"];
		bluetoothPeripheralScrollRect	= gameObjectDict["BluetoothPeripheralScrollRect"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);

        switch (btn.name)
        {
            case "Bluetooth2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, false);
                break;
			case "BluetoothCentralBtn":
				InitializeBluetooth (true);
				break;
			case "BluetoothPeripheralBtn":
				InitializeBluetooth (false);	
                break;
			case "BackFromContentBtn":
				CommonTool.GuiHorizontalMove(bluetoothCategoryContent, Screen.width, MoveID.RightOrUp, canvasGroup, false);
				break;
			case "BackFromScanResultBtn":
				StopScan ();
				break;
			case "BluetoothScanBtn":
				StartScan ();
				break;
			case "Bluetooth2FightFrameBtn":
				CategoryInstance curCategoryInstance = new CategoryInstance(curPatternID, curAmountID, curSymbolID, curDigitID, curOperandID);
				GameManager.Instance.LastGUI = GuiFrameID.BluetoothFrame;
				GameManager.Instance.CurCategoryInstance = curCategoryInstance;
				GameManager.Instance.SwitchWrapper(GuiFrameID.FightFrame);
				break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

	protected override void OnDropdownClick (Dropdown dpd)
	{
		base.OnDropdownClick (dpd);
		switch (dpd.name)
		{
			case "PatternDropdown":
			case "OperandDropdown":
				break;
			case "AmountDropdown":
				curAmountID = (AmountID)dpd.value;
				break;
			case "SymbolDropdown":
				curSymbolID = (SymbolID)dpd.value;
				break;
			case "DigitDropdown":
				curDigitID = (DigitID)dpd.value;
				break;
			default:
				MyDebug.LogYellow("Can not find Dropdown: " + dpd.name);
				break;
		}
	}

	/// <summary>
	/// 刷新Dropdown的状态
	/// </summary>
	private void RefreshCategoryContent()
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

	private void RefreshScanResultContent()
	{
		bluetoothPeripheralScanTip.SetActive (!isCentral);
		RemovePeripherals ();
	}

	private void AddPeripheral (string address, string name)
	{
		if (!peripheralDict.ContainsKey (address))
		{
			GameObject peripheral = GameManager.Instance.GetPrefabItem("BluetoothItem");
			peripheral.SendMessage ("InitPrefabItem", new BluetoothInstance (address, name));
			peripheral.transform.SetParent (bluetoothPeripheralScrollRect.transform);
			peripheral.transform.localScale = Vector3.one;

			peripheralDict [address] = name;
		}
	}

	private void RemovePeripherals ()
	{
		for (int i = 0; i < bluetoothPeripheralScrollRect.transform.childCount; i++)
		{
			GameObject gameObject = bluetoothPeripheralScrollRect.transform.GetChild (i).gameObject;
			Destroy (gameObject);
		}

		if (peripheralDict != null)	peripheralDict.Clear ();
	}

	private void StartScan()
	{
		GameManager.Instance.ServiceUUID = (int)curAmountID + "" + (int)curSymbolID + "" + (int)curDigitID + "0";
		GameManager.Instance.ReadUUID    = (int)curAmountID + "" + (int)curSymbolID + "" + (int)curDigitID + "1";
		GameManager.Instance.WriteUUID   = (int)curAmountID + "" + (int)curSymbolID + "" + (int)curDigitID + "2";

		if (isCentral) 
		{
			MyDebug.LogGreen("Start Scaning!");
			bluetoothScanResultContent.SetActive (true);
			RefreshScanResultContent ();
			CommonTool.GuiHorizontalMove (bluetoothScanResultContent, Screen.width, MoveID.RightOrUp, canvasGroup, true);
			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (new string[]{GameManager.Instance.ServiceUUID}, 
				(address, name) => 
				{
					AddPeripheral (address, name);
				});
		} 
		else 
		{
			BluetoothLEHardwareInterface.PeripheralName(GameManager.Instance.UserName);

			BluetoothLEHardwareInterface.CreateCharacteristic(GameManager.Instance.ReadUUID, 
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyRead |
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyNotify,
				BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsReadable, null, 0, null);

			BluetoothLEHardwareInterface.CreateCharacteristic(GameManager.Instance.WriteUUID, 
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyWrite,
				BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsWriteable, null, 0, 
				ReceiveCentralMessage);

			BluetoothLEHardwareInterface.CreateService(GameManager.Instance.ServiceUUID, true, (message)=>
				{
					MyDebug.LogGreen("Create Service Success:"+message);
					BluetoothLEHardwareInterface.StartAdvertising (() => 
						{
							MyDebug.LogGreen("Start Advertising!");
							bluetoothScanResultContent.SetActive (true);
							RefreshScanResultContent ();
							CommonTool.GuiHorizontalMove (bluetoothScanResultContent, Screen.width, MoveID.RightOrUp, canvasGroup, true);
						});
				});
		}
	}

	private void StopScan()
	{
		if (isCentral)
		{
			BluetoothLEHardwareInterface.StopScan ();
			CommonTool.GuiHorizontalMove(bluetoothScanResultContent, Screen.width, MoveID.RightOrUp, canvasGroup, false);
		} 
		else
		{
			BluetoothLEHardwareInterface.StopAdvertising (() => 
				{
					CommonTool.GuiHorizontalMove(bluetoothScanResultContent, Screen.width, MoveID.RightOrUp, canvasGroup, false);
				});
		}
	}


	private void ReceiveCentralMessage(string UUID, byte[] bytes)
	{
		MyDebug.LogGreen("Receive Message!");
	}
		
	private void InitializeBluetooth(bool isCentral)
    {
		this.isCentral = isCentral;
		BluetoothLEHardwareInterface.Initialize(isCentral, !isCentral, () =>
        {
            MyDebug.LogGreen("Central Initialize Success");
			bluetoothCategoryContent.SetActive(true);
			RefreshCategoryContent();
			CommonTool.GuiHorizontalMove(bluetoothCategoryContent,Screen.width,MoveID.RightOrUp,canvasGroup,true);
        },
        (error) =>
        {
            if (error.Contains("Not Supported"))
            {
                MyDebug.LogYellow("Not Supported");
            }
            else if (error.Contains("Not Authorized"))
            {
                MyDebug.LogYellow("Not Authorized");
            }
            else if (error.Contains("Powered Off"))
            {
                MyDebug.LogYellow("Powered Off");
            }
            else if (error.Contains("Not Enabled"))
            {
                MyDebug.LogYellow("Not Enabled");
            }
            else
            {
                MyDebug.LogYellow("Central Initialize Fail: " + error);
            }
        });
    }
}
