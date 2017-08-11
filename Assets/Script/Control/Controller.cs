using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有控制器的基类
/// </summary>
public abstract class Controller : MonoBehaviour
{
    [HideInInspector]
    public ControllerID id;
    public static int index = 0;
    protected abstract void InitController();//初始化控制器
}
