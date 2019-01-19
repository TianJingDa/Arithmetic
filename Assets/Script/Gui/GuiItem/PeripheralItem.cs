using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class PeripheralItem : Item, IPointerClickHandler
{
	private const float connectTime = 5f;

	private BluetoothInstance content;
	private GameObject detailWin;
	private GameObject bluetoothConnectWaiting;
	private Text periphralName;
	private Text periphralAddress;
	private Text bluetoothConnectTime;

	protected override void InitPrefabItem(object data)
	{
		Init();
		content = data as BluetoothInstance;
		if (content == null)
		{
			MyDebug.LogYellow("BluetoothInstance is null!!");
			return;
		}
		periphralName.text = content.name;
		periphralAddress.text = content.address;
	}

	protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
	{
		periphralName = gameObjectDict["PeriphralName"].GetComponent<Text>();
		periphralAddress = gameObjectDict["PeriphralAddress"].GetComponent<Text>();
	}

	protected override void InitDetailWin(GameObject detailWin)
	{
		this.detailWin = detailWin;
	}
		
	public void OnPointerClick(PointerEventData eventData)
	{
        if (detailWin)
		{
			detailWin.SetActive(true);
			Dictionary<string, GameObject> detailWinDict = CommonTool.InitGameObjectDict(detailWin);
			bluetoothConnectWaiting = detailWinDict["BluetoothConnectWaiting"];
			GameObject connectConfirmBtn = detailWinDict["ConnectConfirmBtn"];
			bluetoothConnectTime = detailWinDict["BluetoothConnectTime"].GetComponent<Text>();
			Text bluetoothPeripheralDetailTitle_Text = detailWinDict["BluetoothPeripheralDetailTitle_Text"].GetComponent<Text>();

			bluetoothConnectWaiting.SetActive(false);
			string tip = GameManager.Instance.GetMutiLanguage(bluetoothPeripheralDetailTitle_Text.index);
            bluetoothPeripheralDetailTitle_Text.text = string.Format(tip, content.name);
			CommonTool.AddEventTriggerListener(connectConfirmBtn, EventTriggerType.PointerClick, ConnectToPeripheral);
		}
	}

	private void ConnectToPeripheral(BaseEventData evenData)
	{
		GameManager.Instance.CurBluetoothInstance = content;
		bluetoothConnectWaiting.SetActive(true);
		StartCoroutine(ConnectCountDown());
        BluetoothLEHardwareInterface.ConnectToPeripheral (GameManager.Instance.CurBluetoothInstance.address, 
            (address) => 
				{
                    StopAllCoroutines();
                    int seed = UnityEngine.Random.Range(1, int.MaxValue);
                    BluetoothMessage message = new BluetoothMessage(-1, seed, GameManager.Instance.UserName);
                    GameManager.Instance.SetSendMessageFunc(true);
                    GameManager.Instance.BLESendMessage(message);
				},
			null,
			(address, serviceUUID, characteristicUUID) => 
				{
					if (CommonTool.IsEqualUUID(serviceUUID, GameManager.Instance.ServiceUUID))
					{				
						if (CommonTool.IsEqualUUID(characteristicUUID, GameManager.Instance.ReadUUID))
						{
                            BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (GameManager.Instance.CurBluetoothInstance.address,
                                                                                                   GameManager.Instance.ServiceUUID,
                                                                                                   GameManager.Instance.ReadUUID, null, 
								                                                                   GameManager.Instance.CentralReceiveMessage);
						}
					}
				}, 
			(address) =>
				{
                    // this will get called when the device disconnects
                    // be aware that this will also get called when the disconnect
                    // is called above. both methods get call for the same action
                    // this is for backwards compatibility
                    MyDebug.LogWhite("Peripheral Disconnect!");
                    string tip = GameManager.Instance.GetMutiLanguage("Text_80019");
                    GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Single, tip, () => GameManager.Instance.SwitchWrapper(GuiFrameID.StartFrame), null);
                    GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);

                });
	}

	private IEnumerator ConnectCountDown()
	{
		float time = connectTime;
		while(time > 0)
		{
			time -= Time.deltaTime;
			bluetoothConnectTime.text = Mathf.CeilToInt(time).ToString();
			yield return null;
		}
        BluetoothLEHardwareInterface.DisconnectPeripheral(content.address, null);
        detailWin.SetActive(false);
    }
}

[Serializable]
public class BluetoothInstance
{
	public string address;
	public string name;

    public BluetoothInstance() { }

    public BluetoothInstance(string address, string name)
	{
		this.address = address;
		this.name = name;
	}
}