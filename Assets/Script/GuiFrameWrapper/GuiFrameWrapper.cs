using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有GUI显示层的基类
/// </summary>
public abstract class GuiFrameWrapper : MonoBehaviour
{
    [HideInInspector]
    public GuiFrameID id;
    public virtual void InitUI() { }
    public virtual void UpdateWrapper() { }
}
