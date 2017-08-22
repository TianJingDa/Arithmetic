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
    private int minAmount;                          //子物体的最少数量
    private int amount;                             //实际信息的数量
    private int realIndex;                          //实际信息的序号
    private int extra = 2;                          //额外的子物体
    private bool init = false;                      //初始化列表
    private string itemName;                        //prefabItem名字
    private ArrayList dataList;                     //实际信息
    private Vector2 startPosition;
    private Vector2 gridLayoutOffsetMin;
    private Vector2 gridLayoutOffsetMax;
    private Vector2 gridLayoutPos;
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
        gridLayoutPos = gridRectTransform.anchoredPosition;
        gridLayoutOffsetMin = gridRectTransform.offsetMin;
        gridLayoutOffsetMax = gridRectTransform.offsetMax;
        scrollRect = transform.parent.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });
        parentRectTransform = scrollRect.transform as RectTransform;
        children = new List<RectTransform>();
        Vector3 scrollRectUp = scrollRect.transform.TransformPoint(parentRectTransform.rect.min);
        Vector3 childBottom = transform.GetChild(11).TransformPoint(transform.GetChild(11).GetComponent<RectTransform>().rect.max);
        Vector3 anchoredPosition = transform.TransformPoint(gridRectTransform.anchoredPosition);
        init = true;
    }

    public void InitList(ArrayList dataList, string name)
    {
        Init();
        amount = dataList.Count;
        realIndex = -1;
        itemName = name;
        this.dataList = dataList;
        children.Clear();

        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            int row = (int)(parentRectTransform.rect.height / (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y));
            minAmount = (row + extra) * gridLayoutGroup.constraintCount;
            //gridRectTransform.sizeDelta = new Vector2(gridRectTransform.sizeDelta.x, minAmount * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y));
        }
        else
        {
            int column = (int)(parentRectTransform.rect.width / (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x));
            minAmount = (column + extra) * gridLayoutGroup.constraintCount;
            //gridRectTransform.sizeDelta = new Vector2(minAmount * (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x), gridRectTransform.sizeDelta.y);
        }
        gridRectTransform.anchoredPosition = startPosition = gridLayoutPos;
        gridRectTransform.offsetMin = gridLayoutOffsetMin;
        gridRectTransform.offsetMax = gridLayoutOffsetMax;
        if (transform.childCount < minAmount)
        {
            for (int i = transform.childCount; i < minAmount; i++)
            {
                GameObject item = GameManager.Instance.GetPrefabItem(name);
                item.transform.SetParent(transform);
                item.transform.localScale = Vector3.one;
                //Instantiate(item, transform);
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
            //children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
        }

        //如果需要显示的个数小于设定的个数;
        for (int index = 0; index < minAmount; index++)
        {
            children[index].gameObject.SetActive(index < amount);
        }

        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            //如果小了一行，则需要把GridLayout的高度减去一行的高度;
            int row = (minAmount - amount) / gridLayoutGroup.constraintCount;
            if (row > 0)
            {
                gridRectTransform.sizeDelta -= new Vector2(0, (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y) * row);
            }
        }
        else
        {
            //如果小了一列，则需要把GridLayout的宽度减去一列的宽度;
            int column = (minAmount - amount) / gridLayoutGroup.constraintCount;
            if (column > 0)
            {
                gridRectTransform.sizeDelta -= new Vector2((gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x) * column, 0);
            }
        }
    }



    void ScrollCallback(Vector2 data)
    {
        UpdateChildren();
    }

    void UpdateChildren()
    {
        if (transform.childCount < minAmount)
        {
            return;
        }

        Vector2 currentPos = gridRectTransform.anchoredPosition;

        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            float offsetY = currentPos.y - startPosition.y;

            if (offsetY > 0)
            {
                //向上拉，向下扩展;
                {
                    if (realIndex >= amount - 1)
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
                            if (realIndex > amount - 1)
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
                        gridRectTransform.offsetMin += new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

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
            float offsetX = currentPos.x - startPosition.x;

            if (offsetX < 0)
            {
                //向左拉，向右扩展;
                {
                    if (realIndex >= amount - 1)
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

                            if (realIndex > amount - 1)
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
        }
        startPosition = currentPos;
    }

}
