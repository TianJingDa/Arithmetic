using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using System;
using DG.Tweening;


public class AchievementItem : Item, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    protected float durationThreshold = 1.0f;
    protected bool isLongPress;
    protected bool onlyWrong;

    protected AchievementInstance content;//详情
    protected GameObject deleteWin;
    protected GameObject achievementItem_WithoutAchievement;
    protected Text achievementName;
    protected Image achievementImage;
    protected Vector3 position;

    public void OnPointerDown(PointerEventData eventData)
    {
        isLongPress = false;
        position = ((RectTransform)transform).position;
        StartCoroutine("TimeCounter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("TimeCounter");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isLongPress)
        {
            StopCoroutine("TimeCounter");
            OnShortPress();
        }
    }
    protected IEnumerator TimeCounter()
    {
        float duration = 0;
        while (duration < durationThreshold)
        {
            duration += Time.deltaTime;
            yield return null;
        }
        isLongPress = deleteWin != null;
        Vector3 curPosition = ((RectTransform)transform).position;
        float distance = Mathf.Abs(position.y - curPosition.y);
        if (distance <= 2 && isLongPress) OnLongPress();
    }
    protected void OnShortPress()
    {
        if (content == null || string.IsNullOrEmpty(content.finishTime)) return;
        GameManager.Instance.CurAchievementInstance = content;
        GameManager.Instance.SwitchWrapper(GuiFrameID.AchievementDetailFrame, true);
    }
    protected void ShowShareBtn()
    {
        Dictionary<string, GameObject> detailWinDict = new Dictionary<string, GameObject>();
        Text achievementDetailTitleInStatistics = detailWinDict["AchievementDetailTitleInStatistics"].GetComponent<Text>();
        achievementDetailTitleInStatistics.gameObject.SetActive(true);
        achievementDetailTitleInStatistics.text = string.Format(achievementDetailTitleInStatistics.text, GameManager.Instance.UserName);
        detailWinDict["AchievementDetailShareBtnInStatistics"].SetActive(false);
        RectTransform achievementShareBtnsBgInStatistics = detailWinDict["AchievementShareBtnsBgInStatistics"].transform as RectTransform;
        achievementShareBtnsBgInStatistics.gameObject.SetActive(true);
        achievementShareBtnsBgInStatistics.DOMoveY(achievementShareBtnsBgInStatistics.rect.y, 0.3f, true).From();
    }

    protected void OnLongPress()
    {
        if (content == null || string.IsNullOrEmpty(content.finishTime)) return;
        deleteWin.SetActive(true);
        GameObject deleteConfirmBtnInAchievement = CommonTool.GetGameObjectByName(deleteWin, "DeleteConfirmBtnInAchievement");
        CommonTool.AddEventTriggerListener(deleteConfirmBtnInAchievement, EventTriggerType.PointerClick, OnLongPress);
    }
    protected void OnLongPress(BaseEventData data)
    {
        deleteWin.SetActive(false);
        GameManager.Instance.DeleteAchievement(content.achievementName);
    }

    protected override void InitDeleteWin(GameObject deleteWin)
    {
        this.deleteWin = deleteWin;
    }
    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        achievementName                     = gameObjectDict["AchievementName"].GetComponent<Text>();
        achievementImage                    = gameObjectDict["AchievementImage"].GetComponent<Image>();
        achievementItem_WithoutAchievement  = gameObjectDict["AchievementItem_WithoutAchievement"];
    }
    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as AchievementInstance;
        if (content == null)
        {
            MyDebug.LogYellow("AchievementInstance is null!!");
            return;
        }
        bool notHasAchievement = string.IsNullOrEmpty(content.finishTime);
        achievementName.text = GameManager.Instance.GetMutiLanguage(content.mainTitleIndex);
        achievementItem_WithoutAchievement.SetActive(notHasAchievement);
        if (!notHasAchievement) achievementImage.sprite = GameManager.Instance.GetSprite(content.imageIndex);
        List<GameObject> stars = CommonTool.GetGameObjectsContainName(gameObject, "Star");
        for(int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive((i + 1) <= content.star);
        }
    }
}
[Serializable]
public class AchievementInstance
{
    public string achievementName;
    public float accuracy;
    public float meanTime;
    public string mainTitleIndex;
    public string subTitleIndex;
    public string imageIndex;
    public string chapterImageIndex;
    public string finishTime;//完成时间
    //public string classType;
    public CategoryInstance cInstance;
    public int star;
    public int difficulty;
}
