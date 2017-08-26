﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 挂在GridPanel上
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class InfiniteList : MonoBehaviour 
{
    private int itemAmount;                          //子物体的数量
    private int dataAmount;                          //信息的数量
    private int minAmount;                           //Mathf.Min(itemAmount, dataAmount)
    private int realIndex;                           //信息的序号
    private int extra = 2;                           //额外行数
    private bool init = false;                       //初始化
    private string itemName;                         //prefabItem名字
    private ArrayList dataList;                      //实际信息
    private Vector3 startPosition;
    private RectTransform gridRectTransform;
    private RectTransform parentRectTransform;
    private GridLayoutGroup gridLayoutGroup;
    private ScrollRect scrollRect;
    private List<RectTransform> children;
    private List<Vector2> childrenAnchoredPostion;


    private void Init(string name)
    {
        if (init) return;
        itemName = name;
        gridRectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        scrollRect = transform.parent.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });
        parentRectTransform = scrollRect.transform as RectTransform;
        children = new List<RectTransform>();
        childrenAnchoredPostion = new List<Vector2>();
        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            int row = (int)(parentRectTransform.rect.height / (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y));
            itemAmount = (row + extra) * gridLayoutGroup.constraintCount;
        }
        else
        {
            int column = (int)(parentRectTransform.rect.width / (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x));
            itemAmount = (column + extra) * gridLayoutGroup.constraintCount;
        }
        for (int i = 0; i < itemAmount; i++)
        {
            GameObject item = GameManager.Instance.GetPrefabItem(itemName);
            item.transform.SetParent(transform);
            item.transform.localScale = Vector3.one;
            childrenAnchoredPostion.Add(item.GetComponent<RectTransform>().anchoredPosition);
        }
        init = true;
    }

    public void InitList(ArrayList dataList, string name)
    {
        Init(name);
        gridLayoutGroup.enabled = true;
        gridRectTransform.offsetMin = Vector2.zero;
        gridRectTransform.offsetMax = Vector2.zero;
        this.dataList = dataList;
        dataAmount = dataList.Count;
        realIndex = -1;
        children.Clear();
        minAmount = Mathf.Min(itemAmount, dataAmount);
        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            float height = Mathf.CeilToInt(minAmount / gridLayoutGroup.constraintCount) * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);
            gridRectTransform.offsetMin -= new Vector2(0f, (height - gridRectTransform.rect.height));
            startPosition = GetMaxToWorldPos();
        }
        else
        {
            float width = Mathf.CeilToInt(minAmount / gridLayoutGroup.constraintCount) * (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x);
            gridRectTransform.offsetMax += new Vector2((width - gridRectTransform.rect.width), 0f);
            startPosition = GetMinToWorldPos();
        }

        for (int index = 0; index < itemAmount; index++)
        {
            realIndex++;
            children.Add(transform.GetChild(index).GetComponent<RectTransform>());
            children[index].anchoredPosition = childrenAnchoredPostion[index];
            children[index].gameObject.name = itemName + realIndex.ToString();
            children[index].gameObject.SetActive(index < dataAmount);
            children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
        }
    }

    void ScrollCallback(Vector2 data)
    {
        UpdateChildren();
    }

    void UpdateChildren()
    {
        gridLayoutGroup.enabled = false;

        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            Vector3 currentPos = GetMaxToWorldPos();

            float offsetY = currentPos.y - startPosition.y;

            if (offsetY > 0.001 && gridRectTransform.offsetMax.y > 0)
            {
                //向上拉，向下扩展;
                {
                    if (realIndex >= dataAmount - 1)
                    {
                        startPosition = currentPos;
                        return;
                    }

                    float scrollRectUp = scrollRect.transform.TransformPoint(parentRectTransform.rect.max).y;
                    float childBottom = children[0].transform.TransformPoint(children[0].rect.min).y;

                    if (childBottom >= scrollRectUp)
                    {
                        //GridLayoutGroup 底部加长;
                        gridRectTransform.offsetMin -= new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

                        //移动到底部;
                        for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
                        {
                            children[index].SetAsLastSibling();
                            children[index].anchoredPosition = new Vector2(children[index].anchoredPosition.x, children[children.Count - 1].anchoredPosition.y - gridLayoutGroup.cellSize.y - gridLayoutGroup.spacing.y);
                            realIndex++;
                            if (realIndex > dataAmount - 1)
                            {
                                children[index].gameObject.SetActive(false);
                            }
                            else
                            {
                                children[index].gameObject.name = itemName + realIndex.ToString();
                                children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                            }
                        }

                        //更新child;
                        for (int index = 0; index < children.Count; index++)
                        {
                            children[index] = transform.GetChild(index).GetComponent<RectTransform>();
                        }
                    }
                }
            }
            else //if (offsetY <= 0 )//|| gridRectTransform.offsetMax.y <= 0)
            {
                //向下拉，下面收缩;
                if (realIndex  <= children.Count - 1)
                {
                    startPosition = currentPos;
                    return;
                }
                float scrollRectBottom = scrollRect.transform.TransformPoint(parentRectTransform.rect.min).y;
                float childUp = children[children.Count - 1].transform.TransformPoint(children[children.Count - 1].rect.max).y;

                if (childUp < scrollRectBottom)
                {
                    //GridLayoutGroup 底部缩短;
                    gridRectTransform.offsetMin += new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

                    //把底部的一行 移动到顶部
                    for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
                    {
                        children[children.Count - 1 - index].SetAsFirstSibling();
                        children[children.Count - 1 - index].anchoredPosition = new Vector2(children[children.Count - 1 - index].anchoredPosition.x, children[0].anchoredPosition.y + gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);
                        children[children.Count - 1 - index].gameObject.SetActive(true);
                        children[children.Count - 1 - index].gameObject.name = itemName + (realIndex - minAmount).ToString();
                        children[children.Count - 1 - index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex - minAmount]);
                        realIndex--;
                    }


                    //更新child;
                    for (int index = 0; index < children.Count; index++)
                    {
                        children[index] = transform.GetChild(index).GetComponent<RectTransform>();
                    }
                }
            }
            startPosition = currentPos;
        }
        else
        {
            Vector3 currentPos = GetMinToWorldPos();

            float offsetX = currentPos.x - startPosition.x;

            if (offsetX < -0.001 && gridRectTransform.offsetMin.x < 0)
            {
                //向左拉，向右扩展;
                {
                    if (realIndex >= dataAmount - 1)
                    {
                        startPosition = currentPos;
                        return;
                    }

                    float scrollRectLeft = scrollRect.transform.TransformPoint(parentRectTransform.rect.min).x;
                    float childRight = children[0].transform.TransformPoint(children[0].rect.max).x;

                    if (childRight <= scrollRectLeft)
                    {
                        //GridLayoutGroup 右侧加长;
                        gridRectTransform.offsetMax += new Vector2(gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x, 0);

                        //移动到右边;
                        for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
                        {
                            children[index].SetAsLastSibling();
                            children[index].anchoredPosition = new Vector2(children[children.Count - 1].anchoredPosition.x + gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x, children[index].anchoredPosition.y);
                            realIndex++;

                            if (realIndex > dataAmount - 1)
                            {
                                children[index].gameObject.SetActive(false);
                            }
                            else
                            {
                                children[index].gameObject.name = itemName + realIndex.ToString();
                                children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                            }
                        }


                        //更新child;
                        for (int index = 0; index < children.Count; index++)
                        {
                            children[index] = transform.GetChild(index).GetComponent<RectTransform>();
                        }
                    }
                }
            }
            else //if (offsetX > 0.01)
            {
                //向右拉，右边收缩;
                if (realIndex  <= children.Count - 1)
                {
                    startPosition = currentPos;
                    return;
                }
                float scrollRectRight = scrollRect.transform.TransformPoint(parentRectTransform.rect.max).x;
                float childLeft = children[children.Count - 1].transform.TransformPoint(children[children.Count - 1].rect.min).x;

                if (childLeft >= scrollRectRight)
                {
                    //GridLayoutGroup 右侧缩短;
                    gridRectTransform.offsetMax -= new Vector2(gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x, 0);

                    //把右边的一行 移动到左边;
                    for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
                    {
                        children[children.Count - 1 - index].SetAsFirstSibling();
                        children[children.Count - 1 - index].anchoredPosition = new Vector2(children[0].anchoredPosition.x - gridLayoutGroup.cellSize.x - gridLayoutGroup.spacing.x, children[children.Count - 1 - index].anchoredPosition.y);
                        children[children.Count - 1 - index].gameObject.SetActive(true);
                        children[children.Count - 1 - index].gameObject.name = itemName + (realIndex - minAmount).ToString();
                        children[children.Count - 1 - index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex - minAmount]);
                        realIndex--;
                    }


                    //更新child;
                    for (int index = 0; index < children.Count; index++)
                    {
                        children[index] = transform.GetChild(index).GetComponent<RectTransform>();
                    }
                }
            }
            startPosition = currentPos;
        }
    }
    /// <summary>
    /// 获取Grid右上角的世界坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMaxToWorldPos()
    {
        Vector3 max = transform.TransformPoint(gridRectTransform.rect.max);
        return max;
    }
    /// <summary>
    /// 获取Grid左下角的世界坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMinToWorldPos()
    {
        Vector3 min = transform.TransformPoint(gridRectTransform.rect.min);
        return min;
    }

}
