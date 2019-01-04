using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BluetoothItem : Item
{
	private BluetoothInstance content;

	protected override void InitPrefabItem(object data)
	{
		Init();
		content = data as BluetoothInstance;
		if (content == null)
		{
			MyDebug.LogYellow("BluetoothInstance is null!!");
			return;
		}
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