using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UITool : Editor 
{
    /// <summary>
    /// 使用时需现将物体放在其父物体中心，锚点在0.5，0.5处
    /// </summary>
    [MenuItem("Custom Editor/将锚点设为本身大小")]
    public static void AnchorFitter()
    {
        RectTransform target = Selection.gameObjects[0].transform as RectTransform;
        RectTransform targetParent= Selection.gameObjects[0].transform.parent as RectTransform;
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
            float maxX = 0.5f * (1 + target.rect.width / targetParent.rect.width)+ deltaX / targetParent.rect.width;
            float maxY = 0.5f * (1 + target.rect.height / targetParent.rect.height)+ deltaY / targetParent.rect.height;
            target.anchorMin = new Vector2(minX, minY);
            target.anchorMax = new Vector2(maxX, maxY);
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
        }
    }
    [MenuItem("Custom Editor/将锚点设为本身大小（水平）")]
    public static void AnchorFitter_H()
    {
        RectTransform target = Selection.gameObjects[0].transform as RectTransform;
        RectTransform targetParent = Selection.gameObjects[0].transform.parent as RectTransform;
        if (target != null && targetParent != null)
        {
            //target.offsetMin -= new Vector2(0, 200);
            //return;
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
    [MenuItem("Custom Editor/将锚点设为本身大小（垂直）")]
    public static void AnchorFitter_V()
    {
        RectTransform target = Selection.gameObjects[0].transform as RectTransform;
        RectTransform targetParent = Selection.gameObjects[0].transform.parent as RectTransform;
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
