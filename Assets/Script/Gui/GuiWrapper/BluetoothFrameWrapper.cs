using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothFrameWrapper : GuiFrameWrapper
{
	private const float 				advertisingTime = 10;
	private bool   						isCentral;
    private bool                        scaning;
    private Dictionary<string, string>  peripheralDict;

	private PatternID   curPatternID;
	private AmountID    curAmountID;
	private SymbolID    curSymbolID;
	private DigitID     curDigitID;
	private OperandID   curOperandID;

    private GameObject  bluetoothCategoryContent;
	private GameObject  bluetoothScanResultContent;
	private GameObject  bluetoothPeripheralDetailBg;
	private GameObject  bluetoothAdvertisingStopBtn;
	private GameObject  bluetoothConnectWaiting;
    private GameObject  bluetoothReScanBtn;
    private Transform   bluetoothScrollContent;
    private Text  		bluetoothConnectTime;


    void Start ()
    {
        id = GuiFrameID.BluetoothFrame;
        Init();
        scaning = false;
        curPatternID = PatternID.Number;
		curOperandID = OperandID.TwoNumbers;
		peripheralDict = new Dictionary<string, string> ();
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
		bluetoothCategoryContent    	= gameObjectDict["BluetoothCategoryContent"];
		bluetoothScanResultContent    	= gameObjectDict["BluetoothScanResultContent"];
		bluetoothPeripheralDetailBg     = gameObjectDict["BluetoothPeripheralDetailBg"];
		bluetoothConnectWaiting 		= gameObjectDict["BluetoothConnectWaiting"];
        bluetoothAdvertisingStopBtn     = gameObjectDict["BluetoothAdvertisingStopBtn"];
        bluetoothReScanBtn              = gameObjectDict["BluetoothReScanBtn"];
        bluetoothConnectTime            = gameObjectDict["BluetoothConnectTime"].GetComponent<Text>();
        bluetoothScrollContent          = gameObjectDict["BluetoothScrollContent"].GetComponent<Transform>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);

        switch (btn.name)
        {
            case "Bluetooth2StartFrameBtn":
                GameManager.Instance.SwitchWrapperWithScale(GuiFrameID.StartFrame, false);
                break;
			case "BluetoothCentralBtn":
				InitializeBluetooth (true);
				break;
			case "BluetoothPeripheralBtn":
				InitializeBluetooth (false);	
                break;
			case "BackFromContentBtn":
                BluetoothLEHardwareInterface.DeInitialize(() =>
                {
                    MyDebug.LogGreen("DeInitialize Success!");
                    CommonTool.GuiHorizontalMove(bluetoothCategoryContent, Screen.width, MoveID.RightOrUp, canvasGroup, false);
                });
				break;
			case "BackFromScanResultBtn":
			case "BluetoothAdvertisingStopBtn":
				StopScan();
				break;
			case "BluetoothScanBtn":
				StartScan();
				break;
			case "ConnectCancelBtn":
				bluetoothPeripheralDetailBg.SetActive(false);
				break;
			case "BluetoothReScanBtn":
				ReScan();
				break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

	protected override void OnDropdownClick(Dropdown dpd)
	{
		base.OnDropdownClick(dpd);
		switch(dpd.name)
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

	private void ReScan()
	{
		if(isCentral)
		{
			BluetoothLEHardwareInterface.StopScan();
			RemovePeripherals();
			MyDebug.LogGreen("Start ReScaning!");
			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (new string[]{GameManager.Instance.ServiceUUID}, 
				(address, name) => 
				{
					AddPeripheral (address, name);
				});
		}
	}

	private void RefreshScanResultContent()
	{
		RemovePeripherals ();
        bluetoothReScanBtn.SetActive(isCentral);
        bluetoothPeripheralDetailBg.SetActive(!isCentral);
		bluetoothConnectWaiting.SetActive(!isCentral);
		bluetoothAdvertisingStopBtn.SetActive(!isCentral);
	}

	private void AddPeripheral(string address, string name)
	{
		if(!peripheralDict.ContainsKey(address))
		{
			GameObject peripheral = GameManager.Instance.GetPrefabItem(GuiItemID.PeripheralItem);
			peripheral.name = "BluetoothItem" + peripheralDict.Count;
			peripheral.SendMessage("InitPrefabItem", new BluetoothInstance(address, name));
            peripheral.SendMessage("InitDeleteWin", bluetoothPeripheralDetailBg);
			peripheral.transform.SetParent(bluetoothScrollContent);
			peripheral.transform.localScale = Vector3.one;

			peripheralDict[address] = name;
		}
	}

	private void RemovePeripherals()
	{
		for(int i = 0; i < bluetoothScrollContent.childCount; i++)
		{
			GameObject gameObject = bluetoothScrollContent.GetChild(i).gameObject;
			Destroy(gameObject);
		}

        if (peripheralDict != null) peripheralDict.Clear();
	}

	private void StartScan()
	{
        if (scaning)
        {
            MyDebug.LogYellow("Scaning!!!");
            return;
        }
        scaning = true;

        BluetoothLEHardwareInterface.RemoveCharacteristics();
		BluetoothLEHardwareInterface.RemoveServices();

		CategoryInstance curCategoryInstance = new CategoryInstance(curPatternID, curAmountID, curSymbolID, curDigitID, curOperandID);
		GameManager.Instance.CurCategoryInstance = curCategoryInstance;

		GameManager.Instance.ServiceUUID = (int)curAmountID + "" + (int)curSymbolID + "" + (int)curDigitID + "0";
		GameManager.Instance.ReadUUID    = (int)curAmountID + "" + (int)curSymbolID + "" + (int)curDigitID + "1";
		GameManager.Instance.WriteUUID   = (int)curAmountID + "" + (int)curSymbolID + "" + (int)curDigitID + "2";

		if (isCentral) 
		{
			MyDebug.LogGreen("Central Start Scaning!");
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
            MyDebug.LogGreen("Peripheral Start Scaning!");
            BluetoothLEHardwareInterface.PeripheralName(GameManager.Instance.UserName);

			BluetoothLEHardwareInterface.CreateCharacteristic(GameManager.Instance.ReadUUID, 
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyRead |
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyNotify,
				BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsReadable, null, 0, null);

			BluetoothLEHardwareInterface.CreateCharacteristic(GameManager.Instance.WriteUUID, 
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyWrite,
				BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsWriteable, null, 0, 
				GameManager.Instance.PeripheralReceiveMessage);

			BluetoothLEHardwareInterface.CreateService(GameManager.Instance.ServiceUUID, true, (message)=>
				{
                    MyDebug.LogGreen("Create Service Success:" + message);
					BluetoothLEHardwareInterface.StartAdvertising (() => 
						{
							MyDebug.LogGreen("Start Advertising!");
							bluetoothScanResultContent.SetActive (true);
							RefreshScanResultContent ();
							StartCoroutine(AdvertisingCountDown());
							CommonTool.GuiHorizontalMove (bluetoothScanResultContent, Screen.width, MoveID.RightOrUp, canvasGroup, true);
						});
				});
		}
	}

	private IEnumerator AdvertisingCountDown()
	{
		float time = advertisingTime;
		while(time > 0)
		{
			time -= Time.deltaTime;
			bluetoothConnectTime.text = Mathf.CeilToInt(time).ToString();
			yield return null;
		}
		StopScan();
	}

	private void StopScan()
	{
        scaning = false;
		if (isCentral)
		{
			BluetoothLEHardwareInterface.StopScan ();
			CommonTool.GuiHorizontalMove(bluetoothScanResultContent, Screen.width, MoveID.RightOrUp, canvasGroup, false);
		} 
		else
		{
			StopAllCoroutines();
			BluetoothLEHardwareInterface.StopAdvertising (() => 
				{
					CommonTool.GuiHorizontalMove(bluetoothScanResultContent, Screen.width, MoveID.RightOrUp, canvasGroup, false);
				});
		}
	}
		
	private void InitializeBluetooth(bool isCentral)
    {
        this.isCentral = isCentral;
        BluetoothLEHardwareInterface.Initialize(isCentral, !isCentral,
            () =>
            {
                MyDebug.LogGreen("Initialize Success: " + isCentral);
                bluetoothCategoryContent.SetActive(true);
                RefreshCategoryContent();
                CommonTool.GuiHorizontalMove(bluetoothCategoryContent, Screen.width, MoveID.RightOrUp, canvasGroup, true);
            },
            (error) =>
            {
                string message = "";
                if (error.Contains("Not Supported"))
                {
                    MyDebug.LogYellow("Not Supported");
                    message = GameManager.Instance.GetMutiLanguage("Text_80009");
                }
                else if (error.Contains("Not Authorized"))
                {
                    MyDebug.LogYellow("Not Authorized");
                    message = GameManager.Instance.GetMutiLanguage("Text_80010");
                }
                else if (error.Contains("Powered Off"))
                {
                    MyDebug.LogYellow("Powered Off");
                    message = GameManager.Instance.GetMutiLanguage("Text_80011");
                }
                else if (error.Contains("Not Enabled"))
                {
                    MyDebug.LogYellow("Not Enabled");
                    message = GameManager.Instance.GetMutiLanguage("Text_80012");
                }
                else
                {
                    MyDebug.LogYellow("Central Initialize Fail: " + error);
                    message = "Unknow Error:" + error;
                }
                GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Single, message);
                GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
            });

    }
}
