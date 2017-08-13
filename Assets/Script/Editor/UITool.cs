using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UITool : Editor 
{
    [MenuItem("Custom Editor/将锚点设为本身大小")]
    public static void AnthorFitter()
    {
        RectTransform target = Selection.gameObjects[0].transform as RectTransform;
        RectTransform targetParent= Selection.gameObjects[0].transform.parent as RectTransform;
        if (target != null && targetParent != null)
        {
            float minX = 0.5f * (1 - target.rect.width / targetParent.rect.width);
            float minY = 0.5f * (1 - target.rect.height / targetParent.rect.height);
            float maxX = 0.5f * (1 + target.rect.width / targetParent.rect.width);
            float maxY= 0.5f * (1 + target.rect.height / targetParent.rect.height);
            target.anchorMin = new Vector2(minX, minY);
            target.anchorMax = new Vector2(maxX, maxY);
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
        }
    }
}
