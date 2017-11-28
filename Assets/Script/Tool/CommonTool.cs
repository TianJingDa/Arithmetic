using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;


public static class CommonTool 
{
    private static float tweenDuration = 0.5f;

    public static T GetComponentByName<T>(GameObject root, string name) where T : Component
    {
        T[] array = root.GetComponentsInChildren<T>(true);
        T result = null;
        if (array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].name == name)
                {
                    result = array[i];
                    break;
                }
            }
        }
        if (result == null)
        {
            MyDebug.LogYellow("Can not find :" + name);
        }
        return result;
    }
    public static T GetComponentContainsName<T>(GameObject root, string name) where T : Component
    {
        T[] array = root.GetComponentsInChildren<T>(true);
        T result = null;
        if (array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].name.Contains(name))
                {
                    result = array[i];
                    break;
                }
            }
        }
        if (result == null)
        {
            MyDebug.LogYellow("Can not find :" + name);
        }
        return result;
    }
    public static List<T> GetComponentsContainName<T>(GameObject root, string name) where T : Component
    {
        List<T> result = new List<T>();
        T[] array = root.GetComponentsInChildren<T>(true);
        if (array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].name.Contains(name))
                {
                    result.Add(array[i]);
                }
            }
        }
        if (result.Count == 0)
        {
            MyDebug.LogYellow("Can not find :" + name);
        }
        return result;
    }
    public static GameObject GetGameObjectByName(GameObject root, string name)
    {
        GameObject result = null;
        if (root != null)
        {
            Transform[] objArray = root.GetComponentsInChildren<Transform>(true);
            if (objArray != null)
            {
                for (int i = 0; i < objArray.Length; i++)
                {
                    if (objArray[i].name == name)
                    {
                        result = objArray[i].gameObject;
                        break;
                    }
                }
            }
        }
        if (result == null)
        {
            MyDebug.LogYellow("Can not find :" + name);
        }
        return result;
    }
    public static GameObject GetGameObjectContainsName(GameObject root, string name)
    {
        GameObject result = null;
        if (root != null)
        {
            Transform[] objArray = root.GetComponentsInChildren<Transform>(true);
            if (objArray != null)
            {
                for (int i = 0; i < objArray.Length; i++)
                {
                    if (objArray[i].name.Contains(name))
                    {
                        result = objArray[i].gameObject;
                        break;
                    }
                }
            }
        }
        if (result == null)
        {
            MyDebug.LogYellow("Can not find :" + name);
        }
        return result;
    }
    public static List<GameObject> GetGameObjectsContainName(GameObject root, string name)
    {
        List<GameObject> result = new List<GameObject>();
        Transform[] objArray = root.GetComponentsInChildren<Transform>(true);
        if (objArray != null)
        {
            for (int i = 0; i < objArray.Length; i++)
            {
                if (objArray[i].name.Contains(name))  
                {
                    result.Add(objArray[i].gameObject);
                }
            }
        }
        if (result.Count == 0)
        {
            MyDebug.LogYellow("Can not find :" + name);
        }
        return result;
    }
    public static GameObject GetParentByName(GameObject child,string name)
    {
        GameObject result = null;

        if(child.transform.parent.gameObject.name == name)
        {
            result = child.transform.parent.gameObject;
        }
        else if(child.transform.parent != null)
        {
            result = GetParentByName(child.transform.parent.gameObject, name);
        }

        if (result == null)
        {
            MyDebug.LogYellow("Can not find :" + name);
        }

        return result;


    }
    public static void InitText(GameObject root)
    {
        Text[] textArray = root.GetComponentsInChildren<Text>(true);
        if (textArray.Length == 0)
        {
            return;
        }
        //Font curFont = GameManager.Instance.GetFont();
        for (int i = 0; i < textArray.Length; i++)
        {
            //textArray[i].font = curFont;
            //textArray[i].color = GameManager.Instance.GetColor(textArray[i].index);
            if (string.IsNullOrEmpty(textArray[i].index)) continue;
            string text = GameManager.Instance.GetMutiLanguage(textArray[i].index);
            if (text.Contains("\\n")) text = text.Replace("\\n", "\n");
            textArray[i].text = text;
        }
    }
    public static void InitImage(GameObject root)
    {
        return;
        Image[] imageArray = root.GetComponentsInChildren<Image>(true);
        if (imageArray.Length == 0)
        {
            return;
        }
        for (int i = 0; i < imageArray.Length; i++)
        {
            if (string.IsNullOrEmpty(imageArray[i].index))
            {
                continue;
            }
            Sprite sprite = GameManager.Instance.GetSprite(imageArray[i].index);
            if (sprite != null)
            {
                imageArray[i].sprite = sprite;
            }
            else
            {
                MyDebug.LogYellow("Can not load Sprite:" + imageArray[i].index);
            }
        }
    }
    public static Dictionary<string, GameObject> InitGameObjectDict(GameObject root)
    {
        Dictionary<string, GameObject> gameObjectDict = new Dictionary<string, GameObject>();
        Transform[] gameObjectArray = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < gameObjectArray.Length; i++)
        {
            try
            {
                gameObjectDict.Add(gameObjectArray[i].name, gameObjectArray[i].gameObject);
            }
            catch
            {
                if(!gameObjectArray[i].name.Contains("Question"))
                    MyDebug.LogYellow(gameObjectArray[i].name);
            }
        }
        return gameObjectDict;
    }
    public static Dictionary<string, RectTransform> InitRectTransformDict(GameObject root)
    {
        Dictionary<string, RectTransform> rectTransformDict = new Dictionary<string, RectTransform>();
        RectTransform[] rectTransformArray = root.GetComponentsInChildren<RectTransform>(true);
        for (int i = 0; i < rectTransformArray.Length; i++)
        {
            rectTransformDict.Add(rectTransformArray[i].name, rectTransformArray[i]);
        }
        return rectTransformDict;
    }
    public static void AddEventTriggerListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }
        trigger.triggers = new List<EventTrigger.Entry>();
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }
    public static Rect GetShotTargetRect(RectTransform targetRect)
    {
        float maxWidth = Screen.width;
        float maxHeight = Screen.height;
        float width = targetRect.rect.width;
        float height = targetRect.rect.height;
        float startX = (maxWidth - width) * 0.5f + targetRect.localPosition.x;
        float startY = (maxHeight - height) * 0.5f + targetRect.localPosition.y;
        return new Rect(startX, startY, width, height);
    }
    /// <summary>
    /// Gui水平动画
    /// </summary>
    /// <param name="gui"></param>
    /// <param name="endValue"></param>
    /// <param name="mID"></param>
    /// <param name="isIn"></param>
    public static void GuiHorizontalMove(GameObject gui, float endValue, MoveID mID, CanvasGroup canvasGroup, bool isIn)
    {
        if (isIn)
        {
            gui.transform.DOLocalMoveX(endValue * (int)mID, tweenDuration, true).
                          From().
                          SetEase(Ease.OutQuint).
                          OnStart(() => canvasGroup.interactable = false).
                          OnComplete(() => canvasGroup.interactable = true);
        }
        else
        {
            gui.transform.DOLocalMoveX(endValue * (int)mID, tweenDuration, true).
                          SetEase(Ease.OutQuint).
                          OnStart(() => canvasGroup.interactable = false).
                          OnComplete(() =>
                          {
                              canvasGroup.interactable = true;
                              gui.SetActive(false);
                              gui.transform.localPosition = Vector3.zero;
                          });
        }
    }
    /// <summary>
    /// Gui垂直动画
    /// </summary>
    /// <param name="gui"></param>
    /// <param name="endValue"></param>
    /// <param name="mID"></param>
    /// <param name="isIn"></param>
    public static void GuiVerticalMove(GameObject gui, float endValue, MoveID mID, CanvasGroup canvasGroup, bool isIn)
    {
        if (isIn)
        {
            gui.transform.DOLocalMoveY(endValue * (int)mID, tweenDuration, true).
                          From().
                          SetEase(Ease.OutQuint).
                          OnStart(() => canvasGroup.interactable = false).
                          OnComplete(() => canvasGroup.interactable = true);
        }
        else
        {
            gui.transform.DOLocalMoveY(endValue * (int)mID, tweenDuration, true).
                          SetEase(Ease.OutQuint).
                          OnStart(() => canvasGroup.interactable = false).
                          OnComplete(() =>
                          {
                              canvasGroup.interactable = true;
                              gui.SetActive(false);
                              gui.transform.localPosition = Vector3.zero;
                          });
        }
    }
    /// <summary>
    /// Gui缩放动画
    /// </summary>
    /// <param name="gui"></param>
    /// <param name="isIn"></param>
    public static void GuiScale(GameObject gui, CanvasGroup canvasGroup, bool isIn)
    {
        if (isIn)
        {
            gui.transform.DOScale(Vector3.zero, tweenDuration).
                          From().
                          SetEase(Ease.OutQuint).
                          OnStart(() => canvasGroup.interactable = false).
                          OnComplete(() => canvasGroup.interactable = true);
        }
        else
        {
            gui.transform.DOScale(Vector3.zero, tweenDuration).
                          SetEase(Ease.OutQuint).
                          OnStart(() => canvasGroup.interactable = false).
                          OnComplete(() =>
                          {
                              canvasGroup.interactable = true;
                              gui.SetActive(false);
                              gui.transform.localScale = Vector3.one;
                          });
        }
    }

}
