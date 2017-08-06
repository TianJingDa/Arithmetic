using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有GUI数据层的基类
/// </summary>
public class GuiFrame
{
    protected GuiFrameID id;

    public GuiFrame (GuiFrameID id)
    {
        this.id = id;
    }
    public virtual void Active()
    {
        
    }
    public virtual void DeActive()
    {

    }

}
