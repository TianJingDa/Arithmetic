using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterItem : Item
{
    protected AchievementInstance content;//详情
    protected GameObject detailWin;


    protected override void InitDetailWin(GameObject detailWin)
    {
        this.detailWin = detailWin;
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
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        
    }

    private void OnClick()
    {
        detailWin.SetActive(true);
        CommonTool.GuiScale(detailWin, GameManager.Instance.CurCanvasGroup, true);
    }
}
