using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class BluetoothItem : Item, IPointerClickHandler
{
	private const float connectTime = 5f;

	private BluetoothInstance content;
	private GameObject detailWin;
	private GameObject bluetoothConnectWaiting;
	private Text periphralName;
	private Text periphralAddress;
	private Text bluetoothConnectTimeText;

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
		if(detailWin)
		{
			detailWin.SetActive(true);
			Dictionary<string, GameObject> detailWinDict = CommonTool.InitGameObjectDict(detailWin);
			bluetoothConnectWaiting = detailWinDict["BluetoothConnectWaiting"];
			GameObject connectConfirmBtn = detailWinDict["ConnectConfirmBtn"];
			bluetoothConnectTimeText = detailWinDict["BluetoothConnectTimeText"].GetComponent<Text>();
			Text peripheralConnectTip = detailWinDict["PeripheralConnectTip"].GetComponent<Text>();

			bluetoothConnectWaiting.SetActive(false);
			string tip = GameManager.Instance.GetMutiLanguage(peripheralConnectTip.index);
			peripheralConnectTip.text = string.Format(tip, content.name);
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
					GameManager.Instance.LastGUI = GuiFrameID.BluetoothFrame;
					GameManager.Instance.SwitchWrapper(GuiFrameID.FightFrame);
				},
			(address, serviceUUID) => {},
			(address, serviceUUID, characteristicUUID) => 
				{
					if (CommonTool.IsEqualUUID(serviceUUID, GameManager.Instance.ServiceUUID))
					{				
						if (CommonTool.IsEqualUUID(characteristicUUID, GameManager.Instance.ReadUUID))
						{
							BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (GameManager.Instance.CurBluetoothInstance.address, 
									   															   GameManager.Instance.ServiceUUID, 
																								   GameManager.Instance.ReadUUID,
								(deviceAddress, notification) => {}, 
								(deviceAddress, characteristic, data) => {});
						}
					}
				}, 
			(address) =>
				{
					// this will get called when the device disconnects
					// be aware that this will also get called when the disconnect
					// is called above. both methods get call for the same action
					// this is for backwards compatibility
					MyDebug.LogWhite("Peripheral Disconnect Actively!");
				});
	}

	private IEnumerator ConnectCountDown()
	{
		float time = connectTime;
		while(time > 0)
		{
			time -= Time.deltaTime;
			bluetoothConnectTimeText.text = Mathf.CeilToInt(time).ToString();
			yield return null;
		}
		//TODO:待验证
		BluetoothLEHardwareInterface.DisconnectPeripheral (content.address, (message)=>
			{
				MyDebug.LogYellow("Disconnect Peripheral Because Time Out!");
				detailWin.SetActive(false);
			});


	}
}

[Serializable]
public class BluetoothInstance
{
	public string address;
	public string name;

	public BluetoothInstance(string address, string name)
	{
		this.address = address;
		this.name = name;
	}
}