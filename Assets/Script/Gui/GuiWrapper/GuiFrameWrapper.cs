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
            return GameManager.Instance.m_CurExamID;
        }
        set
        {
            if (id == GuiFrameID.SetUpFrame)
            {
                GameManager.Instance.m_CurExamID = value;
            }
        }
    }
    public void InitGui()
    {
        InitGuiText();
        InitGuiImage();
    }
    private void InitGuiImage()
    {
        ImageExtension[] imageArray = gameObject.GetComponentsInChildren<ImageExtension>(true);
        if (imageArray.Length == 0)
        {
            Debug.Log("No child has Image component which need modify!");
            return;
        }
        for (int i = 0; i < imageArray.Length; i++)
        {
            if (imageArray[i].index == "")
            {
                Debug.Log("This image's index is NULL:" + GetObjectPath(imageArray[i].gameObject));
                continue;
            }
            Sprite sprite = GameManager.Instance.GetSprite(imageArray[i].index);
            if (sprite != null)
            {
                imageArray[i].gameObject.GetComponent<Image>().sprite = sprite;
            }
            else
            {
                Debug.Log("Can not load Sprite:" + imageArray[i].index);
            }
        }
    }
    private void InitGuiText()
    {
        Text[] textArray = gameObject.GetComponentsInChildren<Text>(true);
        if (textArray.Length == 0)
        {
            Debug.Log("No child has Text component!");
            return;
        }
        Font curFont = GameManager.Instance.GetFont();
        for (int i = 0; i < textArray.Length; i++)
        {
            if (textArray[i].text == "")
            {
                Debug.Log("This text's content is NULL:" + GetObjectPath(textArray[i].gameObject));
                continue;
            }
            textArray[i].font = curFont;
            textArray[i].color = GameManager.Instance.GetColor(textArray[i].text);
            textArray[i].text = GameManager.Instance.GetMutiLanguage(textArray[i].text);
        }
    }
    private string GetObjectPath(GameObject obj)
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
