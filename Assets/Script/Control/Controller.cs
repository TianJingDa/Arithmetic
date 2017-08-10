using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有控制器的基类
/// </summary>
public abstract class Controller : MonoBehaviour
{
    protected ControllerID id;
    protected abstract void InitController();//初始化控制器
}
