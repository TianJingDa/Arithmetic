using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary>
/// 所有GUI显示层的基类
/// </summary>
public abstract class GuiFrameWrapper : MonoBehaviour
{
    [HideInInspector]
    public GuiFrameID id;
    public virtual void UpdateWrapper() { }
    public virtual void OnClick(Button btn)
    {
        if (btn == null)
        {
            Debug.Log("Button is NULL!");
            return;
        }
    }
    public GuiFrameID CurExamID
    {
        get
        {
            return GameManager.Instance.M_CurExamID;
        }
        set
        {
            if (id == GuiFrameID.SetUpFrame)
            {
                GameManager.Instance.M_CurExamID = value;
            }
        }
    }
    public void InitGui()
    {
        InitGuiText();
    }
    private void InitGuiText()
    {
        Text[] textArray = gameObject.GetComponentsInChildren<Text>(true);
        if (textArray.Length == 0)
        {
            Debug.Log("No child has Text component!");
            return;
        }
        for (int i = 0; i < textArray.Length; i++)
        {
            if (textArray[i].text == "")
            {
                Debug.Log("This text's content is NULL:" + GetTextPath(textArray[i].gameObject));
                continue;
            }
            textArray[i].text = GameManager.Instance.GetMutiLanguage(textArray[i].text);
        }
    }
    private string GetTextPath(GameObject obj)
    {
        StringBuilder path = new StringBuilder(obj.name);
        Transform Parent = obj.transform.parent;
        while (Parent != null)
        {
            path.Insert(0, Parent.name + "/");
            Parent = Parent.parent;
        }
        return path.ToString();
    }
}
