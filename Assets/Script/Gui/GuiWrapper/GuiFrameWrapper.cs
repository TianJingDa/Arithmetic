using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary>
/// 所有GUI显示层的基类，先用数据初始化再找物体
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
            Debug.LogError("Button is NULL!");
            return;
        }
    }
    protected void InitGui()
    {
        InitGuiText();
        InitGuiImage();
    }
    private void InitGuiImage()
    {
        return;
        Image[] imageArray = gameObject.GetComponentsInChildren<Image>(true);
        if (imageArray.Length == 0)
        {
            return;
        }
        for (int i = 0; i < imageArray.Length; i++)
        {
            if (imageArray[i].index == "")
            {
                Debug.LogError("This image's index is NULL:" + GetObjectPath(imageArray[i].gameObject));
                continue;
            }
            Sprite sprite = GameManager.Instance.GetSprite(imageArray[i].index);
            if (sprite != null)
            {
                imageArray[i].sprite = sprite;
            }
            else
            {
                Debug.LogError("Can not load Sprite:" + imageArray[i].index);
            }
        }
    }
    private void InitGuiText()
    {
        Text[] textArray = gameObject.GetComponentsInChildren<Text>(true);
        if (textArray.Length == 0)
        {
            return;
        }
        //Font curFont = GameManager.Instance.GetFont();
        for (int i = 0; i < textArray.Length; i++)
        {
            if (textArray[i].index == "")
            {
                continue;
            }
            //textArray[i].font = curFont;
            //textArray[i].color = GameManager.Instance.GetColor(textArray[i].index);
            textArray[i].text = GameManager.Instance.GetMutiLanguage(textArray[i].index);
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

