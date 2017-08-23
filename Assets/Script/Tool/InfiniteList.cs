using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 挂在GridPanel上
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class InfiniteList : MonoBehaviour 
{
    private int itemAmount;                          //子物体的最少数量
    private int dataAmount;                          //实际信息的数量
    private int realIndex;                           //实际信息的序号
    private int extra = 2;                           //额外的子物体
    private bool init = false;                       //初始化列表
    private string itemName;                         //prefabItem名字
    private ArrayList dataList;                      //实际信息
    private Vector3 startPosition;
    private RectTransform gridRectTransform;
    private RectTransform parentRectTransform;
    private GridLayoutGroup gridLayoutGroup;
    private ScrollRect scrollRect;
    private List<RectTransform> children;



    private void Init()
    {
        if (init) return;
        gridRectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        //gridLayoutGroup.enabled = false;
        scrollRect = transform.parent.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });
        parentRectTransform = scrollRect.transform as RectTransform;
        children = new List<RectTransform>();
        init = true;
    }

    public void InitList(ArrayList dataList, string name)
    {
        Init();
        dataAmount = dataList.Count;
        realIndex = -1;
        itemName = name;
        this.dataList = dataList;
        children.Clear();

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
        int minAmount = Mathf.Min(itemAmount, dataAmount);
        if (transform.childCount < minAmount)
        {
            for (int i = transform.childCount; i < minAmount; i++)
            {
                GameObject item = GameManager.Instance.GetPrefabItem(itemName);
                item.transform.SetParent(transform);
                item.transform.localScale = Vector3.one;
            }
        }
        else
        {
            for (int i = minAmount; i < transform.childCount; i++)
            {
                GameObject go = transform.GetChild(i).gameObject;
                Destroy(go);
            }
        }

        for (int index = 0; index < transform.childCount; index++)
        {
            Transform trans = transform.GetChild(index);
            children.Add(transform.GetChild(index).GetComponent<RectTransform>());
            realIndex++;
            children[index].gameObject.name = itemName + realIndex.ToString();
            children[index].gameObject.SetActive(true);
            //children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
        }

        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            int row = Mathf.CeilToInt(minAmount / gridLayoutGroup.constraintCount);
            float height = row * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);
            float width = parentRectTransform.rect.width;
            gridRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, height);
            gridRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
            startPosition = GetMaxToWorldPos();
        }
        else
        {
            int column = Mathf.CeilToInt(minAmount / gridLayoutGroup.constraintCount);
            float height = parentRectTransform.rect.height;
            float width = column * (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x);
            gridRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, height);
            gridRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
            startPosition = GetMinToWorldPos();
        }
    }



    void ScrollCallback(Vector2 data)
    {
        UpdateChildren();
    }

    void UpdateChildren()
    {
        if (transform.childCount < itemAmount)
        {
            return;
        }


        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            Vector3 currentPos = GetMaxToWorldPos();

            float offsetY = currentPos.y - startPosition.y;

            if (offsetY > 0)
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
                                //children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                            }
                        }

                        //GridLayoutGroup 底部加长;
                        gridRectTransform.offsetMin -= new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

                        //更新child;
                        for (int index = 0; index < children.Count; index++)
                        {
                            children[index] = transform.GetChild(index).GetComponent<RectTransform>();
                        }
                    }
                }
            }
            else
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
                    //把底部的一行 移动到顶部
                    for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
                    {
                        realIndex--;
                        children[children.Count - 1 - index].SetAsFirstSibling();
                        children[children.Count - 1 - index].anchoredPosition = new Vector2(children[children.Count - 1 - index].anchoredPosition.x, children[0].anchoredPosition.y + gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);
                        children[children.Count - 1 - index].gameObject.SetActive(true);
                        children[children.Count - 1 - index].gameObject.name = itemName + realIndex.ToString();
                        //children[children.Count - 1 - index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                    }

                    //GridLayoutGroup 底部缩短;
                    gridRectTransform.offsetMin += new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

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

            if (offsetX < 0)
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
                                //children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                            }
                        }

                        //GridLayoutGroup 右侧加长;
                        gridRectTransform.offsetMax += new Vector2(gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x, 0);

                        //更新child;
                        for (int index = 0; index < children.Count; index++)
                        {
                            children[index] = transform.GetChild(index).GetComponent<RectTransform>();
                        }
                    }
                }
            }
            else
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
                    //把右边的一行 移动到左边;
                    for (int index = 0; index < gridLayoutGroup.constraintCount; index++)
                    {
                        realIndex--;
                        children[children.Count - 1 - index].SetAsFirstSibling();
                        children[children.Count - 1 - index].anchoredPosition = new Vector2(children[0].anchoredPosition.x - gridLayoutGroup.cellSize.x - gridLayoutGroup.spacing.x, children[children.Count - 1 - index].anchoredPosition.y);
                        children[children.Count - 1 - index].gameObject.SetActive(true);
                        children[children.Count - 1 - index].gameObject.name = itemName + realIndex.ToString();
                        //children[children.Count - 1 - index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                    }

                    //GridLayoutGroup 右侧缩短;
                    gridRectTransform.offsetMax -= new Vector2(gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x, 0);

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
