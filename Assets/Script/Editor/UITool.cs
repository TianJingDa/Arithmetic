using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class UITool : Editor 
{
    /// <summary>
    /// 使用时需现将物体放在其父物体中心，锚点在0.5，0.5处
    /// </summary>
    [MenuItem("Custom Editor/将锚点设为本身大小")]
    public static void AnchorFitter()
    {
        for(int i=0;i< Selection.gameObjects.Length; i++)
        {
            RectTransform target = Selection.gameObjects[i].transform as RectTransform;
            RectTransform targetParent = Selection.gameObjects[i].transform.parent as RectTransform;
            if (target != null && targetParent != null)
            {
                //Debug.Log("target.rect.min:" + target.rect.min.ToString());
                //Debug.Log("target.rect.max:" + target.rect.max.ToString());
                //Debug.Log("target.rect.x:" + target.rect.x.ToString());
                //Debug.Log("target.rect.y:" + target.rect.y.ToString());
                //Debug.Log("target.rect.xMin:" + target.rect.xMin.ToString());
                //Debug.Log("target.rect.xMax:" + target.rect.xMax.ToString());
                //Debug.Log("target.rect.yMin:" + target.rect.yMin.ToString());
                //Debug.Log("target.rect.yMax:" + target.rect.yMax.ToString());
                //Debug.Log("target.rect.position:" + target.rect.position.ToString());
                //Debug.Log("target.position:" + target.position.ToString());
                //Debug.Log("target.localPosition:" + target.localPosition.ToString());
                //Debug.Log("target.anchoredPosition:" + target.anchoredPosition.ToString());
                //return;
                float deltaX = target.localPosition.x;
                float deltaY = target.localPosition.y;
                float minX = 0.5f * (1 - target.rect.width / targetParent.rect.width) + deltaX / targetParent.rect.width;
                float minY = 0.5f * (1 - target.rect.height / targetParent.rect.height) + deltaY / targetParent.rect.height;
                float maxX = 0.5f * (1 + target.rect.width / targetParent.rect.width) + deltaX / targetParent.rect.width;
                float maxY = 0.5f * (1 + target.rect.height / targetParent.rect.height) + deltaY / targetParent.rect.height;
                target.anchorMin = new Vector2(minX, minY);
                target.anchorMax = new Vector2(maxX, maxY);
                target.offsetMin = Vector2.zero;
                target.offsetMax = Vector2.zero;
            }

        }
    }
    [MenuItem("Custom Editor/将锚点设为本身大小（水平）")]
    public static void AnchorFitter_H()
    {
        for(int i=0;i< Selection.gameObjects.Length; i++)
        {
            RectTransform target = Selection.gameObjects[i].transform as RectTransform;
            RectTransform targetParent = Selection.gameObjects[i].transform.parent as RectTransform;
            if (target != null && targetParent != null)
            {
                float width = target.rect.width;
                float height = target.rect.height;
                float deltaX = target.localPosition.x;
                float deltaY = target.localPosition.y;
                float minX = 0.5f * (1 - target.rect.width / targetParent.rect.width) + deltaX / targetParent.rect.width;
                float minY = 0.5f * (1 - target.rect.height / targetParent.rect.height) + deltaY / targetParent.rect.height;
                float maxX = 0.5f * (1 + target.rect.width / targetParent.rect.width) + deltaX / targetParent.rect.width;
                float maxY = 0.5f * (1 + target.rect.height / targetParent.rect.height) + deltaY / targetParent.rect.height;
                target.anchorMin = new Vector2(minX, (maxY + minY) / 2);
                target.anchorMax = new Vector2(maxX, (maxY + minY) / 2);
                target.offsetMin = new Vector2(0, -(height / 2));
                target.offsetMax = new Vector2(0, (height / 2));
            }
        }
    }
    [MenuItem("Custom Editor/将锚点设为本身大小（垂直）")]
    public static void AnchorFitter_V()
    {
        for(int i=0;i< Selection.gameObjects.Length; i++)
        {
            RectTransform target = Selection.gameObjects[i].transform as RectTransform;
            RectTransform targetParent = Selection.gameObjects[i].transform.parent as RectTransform;
            if (target != null && targetParent != null)
            {
                float width = target.rect.width;
                float height = target.rect.height;
                float deltaX = target.localPosition.x;
                float deltaY = target.localPosition.y;
                float minX = 0.5f * (1 - target.rect.width / targetParent.rect.width) + deltaX / targetParent.rect.width;
                float minY = 0.5f * (1 - target.rect.height / targetParent.rect.height) + deltaY / targetParent.rect.height;
                float maxX = 0.5f * (1 + target.rect.width / targetParent.rect.width) + deltaX / targetParent.rect.width;
                float maxY = 0.5f * (1 + target.rect.height / targetParent.rect.height) + deltaY / targetParent.rect.height;
                target.anchorMin = new Vector2((minX + maxX) / 2, minY);
                target.anchorMax = new Vector2((minX + maxX) / 2, maxY);
                target.offsetMin = new Vector2(0, -(width / 2));
                target.offsetMax = new Vector2(0, (width / 2));
            }
        }
    }
    [MenuItem("Custom Editor/将锚点设为本身中心")]
    public static void AnchorCenter()
    {
        for(int i=0;i< Selection.gameObjects.Length; i++)
        {
            RectTransform target = Selection.gameObjects[0].transform as RectTransform;
            RectTransform targetParent = Selection.gameObjects[0].transform.parent as RectTransform;
            if (target != null && targetParent != null)
            {
                float width = target.rect.width;
                float height = target.rect.height;
                float X = 0.5f + target.localPosition.x / targetParent.rect.width;
                float Y = 0.5f + target.localPosition.y / targetParent.rect.height;
                target.anchorMin = new Vector2(X, Y);
                target.anchorMax = new Vector2(X, Y);
                target.anchoredPosition = Vector2.zero;
            }
        }
    }
    [MenuItem("Custom Editor/恢复TextIndex")]
    public static void RecoverTextIndex()
    {
        for(int i = 0; i < Selection.gameObjects.Length; i++)
        {
            Text[] textArray = Selection.gameObjects[i].GetComponentsInChildren<Text>(true);
            for(int j = 0; j < textArray.Length; j++)
            {
                if(!string.IsNullOrEmpty(textArray[j].text) && textArray[j].text.Contains("Text_"))
                {
                    textArray[j].index = textArray[j].text;
                }
            }
        }
    }
    [MenuItem("Custom Editor/恢复ImageIndex")]
    public static void RecoverImageIndex()
    {
        int num = 0;
        Dictionary<string, string> imageDict = InitImageData();
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            Image[] imageArray = Selection.gameObjects[i].GetComponentsInChildren<Image>(true);
            for (int j = 0; j < imageArray.Length; j++)
            {
                if (imageDict.ContainsKey(imageArray[j].name))
                {
                    imageArray[j].index = imageDict[imageArray[j].name];
                    num++;
                }
            }
        }
        MyDebug.LogGreen("num:" + num);
    }
    private static Dictionary<string,string> InitImageData()
    {
        TextAsset imageAsset = Resources.Load("Language/Image", typeof(TextAsset)) as TextAsset;
        if (imageAsset == null)
        {
            MyDebug.LogYellow("Load File Error!");
            return null;
        }
        char[] charSeparators = new char[] { "\r"[0], "\n"[0] };
        string[] lineArray = imageAsset.text.Split(charSeparators, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> lineList;
        Dictionary<string, string> imageDict = new Dictionary<string, string>();
        for (int i = 0; i < lineArray.Length; i++)
        {
            lineList = new List<string>(lineArray[i].Split(','));
            imageDict.Add(lineList[1], lineList[0]);
        }

        return imageDict;
    }
    [MenuItem("Custom Editor/抓取布局")]
    public static void GetLayout()
    {
        Dictionary<string, MyRectTransform> rectTransformDict = new Dictionary<string, MyRectTransform>();
        RectTransform[] rectTransformArray = Selection.gameObjects[0].GetComponentsInChildren<RectTransform>(true);
        for (int i = 0; i < rectTransformArray.Length; i++)
        {
            if (rectTransformArray[i].name == "FightFrame") continue;
            MyRectTransform rect = new MyRectTransform();
            rect.pivot = new MyVector2(rectTransformArray[i].pivot.x, rectTransformArray[i].pivot.y);
            rect.anchorMax = new MyVector2(rectTransformArray[i].anchorMax.x, rectTransformArray[i].anchorMax.y);
            rect.anchorMin = new MyVector2(rectTransformArray[i].anchorMin.x, rectTransformArray[i].anchorMin.y);
            rect.offsetMax = new MyVector2(rectTransformArray[i].offsetMax.x, rectTransformArray[i].offsetMax.y);
            rect.offsetMin = new MyVector2(rectTransformArray[i].offsetMin.x, rectTransformArray[i].offsetMin.y);
            rect.localEulerAngles = new MyVector3(rectTransformArray[i].localEulerAngles.x, rectTransformArray[i].localEulerAngles.y, rectTransformArray[i].localEulerAngles.z);

            rectTransformDict.Add(rectTransformArray[i].name, rect);
        }
        string path = Application.dataPath + "/Resources/Layout/Vertical/Default.txt";
        IOHelper.SetData(path, rectTransformDict);
    }


}
