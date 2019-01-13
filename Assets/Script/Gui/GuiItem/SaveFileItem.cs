using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Text;


public class SaveFileItem : Item, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    protected float durationThreshold = 1.0f;
    protected bool isLongPress;
    protected bool onlyWrong;
    //protected bool hasAchievement;

    protected SaveFileInstance content;//详情
    //protected GameObject saveFileAchievement_No;
    protected Text saveFileName;
    protected Text saveFileType_Time;
    protected Text saveFileType_Number;
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
        isLongPress = true;
        Vector3 curPosition = ((RectTransform)transform).position;
        float distance = Mathf.Abs(position.y - curPosition.y);
        if (distance <= 2 && isLongPress) OnLongPress();
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        saveFileName = gameObjectDict["SaveFileName"].GetComponent<Text>();
        saveFileType_Time = gameObjectDict["SaveFileType_Time"].GetComponent<Text>();
        saveFileType_Number = gameObjectDict["SaveFileType_Number"].GetComponent<Text>();
        //saveFileAchievement_No = gameObjectDict["SaveFileAchievement_No"];
    }
    protected override void InitPrefabItem(object data)
    {
        Init();
        content = data as SaveFileInstance;
        if (content == null)
        {
            MyDebug.LogYellow("SaveFileInstance is null!!");
            return;
        }
        //hasAchievement = !string.IsNullOrEmpty(content.achievementName);
        saveFileName.text = content.fileName;
        //saveFileAchievement_No.SetActive(!hasAchievement);
        int digit = (int)content.cInstance.digitID + 2;
        int operand = (int)content.cInstance.operandID + 2;
        if (content.cInstance.patternID == PatternID.Time)
        {
            saveFileType_Time.gameObject.SetActive(true);
            saveFileType_Number.gameObject.SetActive(false);
            int amount = GameManager.Instance.AmountArray_Time[(int)content.cInstance.amountID];
            saveFileType_Time.text = string.Format(saveFileType_Time.text, amount, digit, operand);
        }
        else
        {
            saveFileType_Time.gameObject.SetActive(false);
            saveFileType_Number.gameObject.SetActive(true);
            int amount = GameManager.Instance.AmountArray_Number[(int)content.cInstance.amountID];
            saveFileType_Number.text = string.Format(saveFileType_Number.text, amount, digit, operand);
        }
    }
    protected void OnShortPress()
    {
        if (content == null) return;
        GameManager.Instance.CurSaveFileInstance = content;
        GameManager.Instance.SwitchWrapper(GuiFrameID.SaveFileFrame, true);
    }

    //protected void OnAchievementBtn(BaseEventData data)
    //{
    //    //GameObject achievementDetailBgInSaveFile = detailWinDict["AchievementDetailBgInSaveFile"];
    //    //achievementDetailBgInSaveFile.SetActive(true);
    //    //CommonTool.GuiScale(achievementDetailBgInSaveFile, GameManager.Instance.CurCanvasGroup, true);
    //    //Image achievementDetailImageInSaveFile = detailWinDict["AchievementDetailImageInSaveFile"].GetComponent<Image>();
    //    //Text achievementDetailMainTitleInSaveFile = detailWinDict["AchievementDetailMainTitleInSaveFile"].GetComponent<Text>();
    //    //Text achievementDetailSubTitleInSaveFile = detailWinDict["AchievementDetailSubTitleInSaveFile"].GetComponent<Text>();
    //    //Text achievementDetailFinishTimeInSaveFile = detailWinDict["AchievementDetailFinishTimeInSaveFile"].GetComponent<Text>();
    //    //AchievementInstance instance = GameManager.Instance.GetAchievement(content.achievementName);
    //    //achievementDetailImageInSaveFile.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
    //    //achievementDetailMainTitleInSaveFile.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
    //    //achievementDetailSubTitleInSaveFile.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
    //    //achievementDetailFinishTimeInSaveFile.text = GetFinishTime(instance.fileName);

    //}
    //protected string GetFinishTime(string time)
    //{
    //    StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
    //    newTime.Insert(4, ".");
    //    newTime.Insert(7, ".");
    //    return newTime.ToString();
    //}

    protected void OnLongPress()
    {
        if (content == null) return;
        string tip = GameManager.Instance.GetMutiLanguage("Text_20015");
        GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Double, tip,
                      () => GameManager.Instance.DeleteAchievement(content.achievementName), null);
        GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
    }

}
[Serializable]
public class SaveFileInstance
{
    public float timeCost;
    public float opponentAccuracy;
    public string fileName;
    public string accuracy;
    public string opponentName;
    public List<QuentionInstance> qInstancList;
    public string achievementName;//所获成就
    public CategoryInstance cInstance;
}
