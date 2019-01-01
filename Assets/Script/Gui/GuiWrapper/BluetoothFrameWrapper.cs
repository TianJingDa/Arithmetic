using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothFrameWrapper : GuiFrameWrapper
{
    private GameObject bluetoothReceiverBtn;
    private GameObject bluetoothAndroidTip;
    private GameObject bluetoothRoleContent;
    private GameObject bluetoothCategoryContent;

    void Start ()
    {
        id = GuiFrameID.BluetoothFrame;
        Init();

        bluetoothRoleContent.SetActive(true);
        bluetoothCategoryContent.SetActive(false);
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
        bluetoothReceiverBtn        = gameObjectDict["BluetoothAndroidTip"];
        bluetoothAndroidTip         = gameObjectDict["BluetoothAndroidTip"];
        bluetoothRoleContent        = gameObjectDict["BluetoothRoleContent"];
        bluetoothCategoryContent    = gameObjectDict["BluetoothCategoryContent"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);

        switch (btn.name)
        {
            case "Bluetooth2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame, false);
                break;
            case "BluetoothOrganizerBtn":
                AsOrganizer();
                break;
            case "BluetoothReceiverBtn":
                break;                
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private void AsOrganizer()
    {
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            MyDebug.LogGreen("Central Initialize Success");
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
        if (string.IsNullOrEmpty(GameManager.Instance.UserName))
        {
            //此处需要弹框
            MyDebug.LogYellow("Please Set Your Name!");
            return;
        }
        BluetoothLEHardwareInterface.Initialize(false, true, () => 
        {
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
            MyDebug.LogYellow("Peripheral Initialize Fail: " + error);
        });

    }
}
