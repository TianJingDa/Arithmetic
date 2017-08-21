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
    private int extra = 2;                          //额外的子物体
    private int minAmount;                          //子物体的最少数量
    private int amount;                             //实际信息数量
    private int realIndex;                          //实际信息的序号
    private string itemName;                        //prefabItem名字
    private ArrayList dataList;                     //实际信息
    private Vector2 startPosition;
    private Vector2 gridLayoutSize;
    private Vector2 gridLayoutPos;
    private RectTransform gridRectTransform;
    private RectTransform parentRectTransform;
    private GridLayoutGroup gridLayoutGroup;
    private ScrollRect scrollRect;
    private List<RectTransform> children;



    void Start()
    {
        minAmount = 0;
        amount = 0;
        realIndex = -1;
        gridRectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.enabled = false;
        gridLayoutPos = gridRectTransform.anchoredPosition;
        gridLayoutSize = gridRectTransform.sizeDelta;
        scrollRect = transform.parent.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });
        parentRectTransform = scrollRect.transform as RectTransform;
        children = new List<RectTransform>();
    }

    public void InitList(ArrayList dataList, string name)
    {
        gridRectTransform.anchoredPosition = startPosition = gridLayoutPos;
        gridRectTransform.sizeDelta = gridLayoutSize;
        amount = dataList.Count;
        itemName = name;
        this.dataList = dataList;
        children.Clear();
        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            int row = (int)(parentRectTransform.rect.height / (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y));
            minAmount = (row + extra) * gridLayoutGroup.constraintCount;
        }
        else
        {
            int column = (int)(parentRectTransform.rect.width / (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x));
            minAmount = (column + extra) * gridLayoutGroup.constraintCount;
        }
        if (transform.childCount < minAmount)
        {
            for (int i = transform.childCount; i < minAmount; i++)
            {
                GameObject item = GameManager.Instance.GetPrefabItem(name);
                Instantiate(item, transform);
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
            children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
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

                    float scrollRectUp = scrollRect.transform.TransformPoint(Vector3.zero).y;

                    Vector3 childBottomLeft = new Vector3(children[0].anchoredPosition.x, children[0].anchoredPosition.y - gridLayoutGroup.cellSize.y, 0f);
                    float childBottom = transform.TransformPoint(childBottomLeft).y;

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
                                children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                            }
                        }

                        //GridLayoutGroup 底部加长;
                        gridRectTransform.sizeDelta += new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

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
                RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
                Vector3 scrollRectAnchorBottom = new Vector3(0, -scrollRectTransform.rect.height - gridLayoutGroup.spacing.y, 0f);
                float scrollRectBottom = scrollRect.transform.TransformPoint(scrollRectAnchorBottom).y;
                Vector3 childUpLeft = new Vector3(children[children.Count - 1].anchoredPosition.x, children[children.Count - 1].anchoredPosition.y, 0f);
                float childUp = transform.TransformPoint(childUpLeft).y;

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
                        children[children.Count - 1 - index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                    }

                    //GridLayoutGroup 底部缩短;
                    gridRectTransform.sizeDelta -= new Vector2(0, gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);

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

                    float scrollRectLeft = scrollRect.transform.TransformPoint(Vector3.zero).x;
                    Vector3 childBottomRight = new Vector3(children[0].anchoredPosition.x + gridLayoutGroup.cellSize.x, children[0].anchoredPosition.y, 0f);
                    float childRight = transform.TransformPoint(childBottomRight).x;

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
                                children[index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                            }
                        }

                        //GridLayoutGroup 右侧加长;
                        gridRectTransform.sizeDelta += new Vector2(gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x, 0);

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
                RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
                Vector3 scrollRectAnchorRight = new Vector3(scrollRectTransform.rect.width + gridLayoutGroup.spacing.x, 0, 0f);
                float scrollRectRight = scrollRect.transform.TransformPoint(scrollRectAnchorRight).x;
                Vector3 childUpLeft = new Vector3(children[children.Count - 1].anchoredPosition.x, children[children.Count - 1].anchoredPosition.y, 0f);
                float childLeft = transform.TransformPoint(childUpLeft).x;

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
                        children[children.Count - 1 - index].gameObject.SendMessage("InitPrefabItem", dataList[realIndex]);
                    }

                    //GridLayoutGroup 右侧缩短;
                    gridRectTransform.sizeDelta -= new Vector2(gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x, 0);

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
