using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothFrameWrapper : GuiFrameWrapper
{
	private enum ContentType
	{
		Role,
		Category,
		ScanResult
	}

    private GameObject  bluetoothReceiverBtn;
    private GameObject  bluetoothAndroidTip;
    private GameObject  bluetoothRoleContent;
    private GameObject  bluetoothCategoryContent;
	private GameObject  bluetoothScanResultContent;

	private PatternID   curPatternID;
	private AmountID    curAmountID;
	private SymbolID    curSymbolID;
	private DigitID     curDigitID;
	private OperandID   curOperandID;

    void Start ()
    {
        id = GuiFrameID.BluetoothFrame;
        Init();
		curPatternID = PatternID.Number;
		curOperandID = OperandID.TwoNumbers;

        bluetoothRoleContent.SetActive(true);
		bluetoothCategoryContent.SetActive(false);
		bluetoothScanResultContent.SetActive(false);
#if UNITY_ANDROID
        bluetoothReceiverBtn.SetActive(false);
        bluetoothAndroidTip.SetActive(true);
#elif UNITY_IOS
        bluetoothReceiverBtn.SetActive(true);
        bluetoothAndroidTip.SetActive(false);
#endif
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        bluetoothReceiverBtn        	= gameObjectDict["BluetoothAndroidTip"];
        bluetoothAndroidTip         	= gameObjectDict["BluetoothAndroidTip"];
        bluetoothRoleContent        	= gameObjectDict["BluetoothRoleContent"];
		bluetoothCategoryContent    	= gameObjectDict["BluetoothCategoryContent"];
		bluetoothScanResultContent    	= gameObjectDict["BluetoothScanResultContent"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);

        switch (btn.name)
        {
            case "Bluetooth2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, false);
                break;
			case "Bluetooth2FightFrameBtn":
				GameManager.Instance.SwitchWrapper(GuiFrameID.FightFrame);
				break;
			case "BackFromContentBtn":
				SwitchContent (ContentType.Role);
				break;
			case "BackFromScanResultBtn":
				SwitchContent (ContentType.Category);
				break;
			case "BluetoothOrganizerBtn":
				InitializeBluetooth (true);
				break;
            case "BluetoothReceiverBtn":
				InitializeBluetooth (false);	
                break;
			case "BluetoothScanBtn":
				
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

	private void SwitchContent(ContentType type)
	{
		ShowRoleConetent (type == ContentType.Role);
		ShowCategoryContent (type == ContentType.Category);
		ShowScanResultContent (type == ContentType.ScanResult);
	}

	private void ShowRoleConetent(bool isShow)
	{
		bluetoothRoleContent.SetActive (isShow);
	}

	private void ShowCategoryContent(bool isShow)
	{
		bluetoothCategoryContent.SetActive (isShow);
	}

	private void ShowScanResultContent(bool isShow)
	{
		bluetoothScanResultContent.SetActive (isShow);
		//清理prefab，重置状态
	}

	private void InitializeBluetooth(bool isCentral)
	{
		if (isCentral)
			AsOrganizer ();
		else
			AsReceiver ();
	}
		
    private void AsOrganizer()
    {
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            MyDebug.LogGreen("Central Initialize Success");
			SwitchContent (ContentType.Category);
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

    private void AsReceiver()
    {
        BluetoothLEHardwareInterface.Initialize(false, true, () => 
        {
			SwitchContent (ContentType.Category);

            BluetoothLEHardwareInterface.PeripheralName(GameManager.Instance.UserName);

            BluetoothLEHardwareInterface.CreateCharacteristic(GameManager.Instance.ReadCharacteristicUUID, 
                                                               BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyRead |
                                                               BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyNotify,
                                                               BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsReadable, null, 0,
            (characteristicUUID, bytes) =>
            {

            });

            BluetoothLEHardwareInterface.CreateCharacteristic(GameManager.Instance.WriteCharacteristicUUID, 
                                                               BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyWrite,
                                                               BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsWriteable, null, 0,
            (characteristicUUID, bytes) => 
            {

            });

            BluetoothLEHardwareInterface.CreateService(GameManager.Instance.ServiceUUID, true, (serviceUUID) => 
            {

            });

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
				MyDebug.LogYellow("Peripheral Initialize Fail: " + error);
			}
        });

    }
}
