using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 速算类别选择界面
/// </summary>
public class CategoryFrameWrapper : GuiFrameWrapper
{
    private int delta;
    private Dictionary<int, string[]> amountDropdownTextDict;
    private List<Dropdown.OptionData> digitDropdownOptionsList;

    private PatternID   curPatternID;
    private AmountID    curAmountID;
    private SymbolID    curSymbolID;
    private DigitID     curDigitID;
    private OperandID   curOperandID;

    private GameObject  categoryTipBg;
    private Dropdown    amountDropdown;
    private Dropdown    digitDropdown;

    void Start () 
	{
        id = GuiFrameID.CategoryFrame;
        Init();
        delta = 0;
        amountDropdownTextDict = new Dictionary<int, string[]>();
        amountDropdownTextDict.Add(0, new string[] { "Text_30013", "Text_30014", "Text_30015" });
        amountDropdownTextDict.Add(1, new string[] { "Text_30016", "Text_30017", "Text_30018" });
        digitDropdownOptionsList = new List<Dropdown.OptionData>(digitDropdown.options);
        RefreshAllDropdown();
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict, 
                                    Dictionary<string, Button>     ButtonDict, 
                                    Dictionary<string, Toggle>     ToggleDict, 
                                    Dictionary<string, Dropdown>   DropdownDict)
    {
        categoryTipBg = ButtonDict["CategoryTipBg"].gameObject;
        amountDropdown = DropdownDict["AmountDropdown"];
        digitDropdown = DropdownDict["DigitDropdown"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Category2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.StartFrame);
                break;
            case "Category2FightFrameBtn":
                CategoryInstance curCategoryInstance = new CategoryInstance(curPatternID, curAmountID, curSymbolID, curDigitID, curOperandID);
                GameManager.Instance.CurCategoryInstance = curCategoryInstance;
                GameManager.Instance.SwitchWrapper(GuiFrameID.CategoryFrame, GuiFrameID.FightFrame);
                break;
            case "CategoryTipBtn":
            case "CategoryTipBg":
            case "CategoryTipPage":
                categoryTipBg.SetActive(!categoryTipBg.activeSelf);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    protected override void OnDropdownClick(Dropdown dpd)
    {
        base.OnDropdownClick(dpd);
        switch (dpd.name)
        {
            case "PatternDropdown":
                curPatternID = (PatternID)dpd.value;
                RefreshAmountDropdown(dpd.value);
                break;
            case "AmountDropdown":
                curAmountID = (AmountID)dpd.value;
                break;
            case "SymbolDropdown":
                curSymbolID = (SymbolID)dpd.value;
                RefreshDigitDropdown(dpd.value);
                break;
            case "DigitDropdown":
                curDigitID = (DigitID)(dpd.value + delta);
                break;
            case "OperandDropdown":
                curOperandID = (OperandID)dpd.value;
                break;
            default:
                MyDebug.LogYellow("Can not find Dropdown: " + dpd.name);
                break;
        }
    }

    /// <summary>
    /// 刷新Dropdown的状态
    /// </summary>
    /// <param name="dpd"></param>
    /// <param name="index"></param>
    private void RefreshAllDropdown()
    {
        Dropdown[] dropdownArray = GetComponentsInChildren<Dropdown>(true);
        for(int i = 0; i < dropdownArray.Length; i++)
        {
            for (int j = 0; j < dropdownArray[i].options.Count; j++)
            {
                dropdownArray[i].options[j].text = GameManager.Instance.GetMutiLanguage(dropdownArray[i].options[j].text);
            }
        }
    }
    private void RefreshAmountDropdown(int index)
    {
        for (int i = 0; i < amountDropdown.options.Count; i++)
        {
            amountDropdown.options[i].text = GameManager.Instance.GetMutiLanguage(amountDropdownTextDict[index][i]);
        }
        amountDropdown.value = 0;
        amountDropdown.RefreshShownValue();
    }
    private void RefreshDigitDropdown(int index)
    {
        switch (index)
        {
            case 0:
            case 1:
                digitDropdown.options = digitDropdownOptionsList;
                delta = 0;
                break;
            case 2:
                digitDropdown.options = digitDropdownOptionsList.GetRange(0, 2);
                delta = 0;
                break;
            case 3:
                digitDropdown.options = digitDropdownOptionsList.GetRange(1, 3);
                delta = 1;
                break;
        }
        digitDropdown.value = 0;
        digitDropdown.RefreshShownValue();
        OnDropdownClick(digitDropdown);
    }
}
public class CategoryInstance
{
    public CategoryInstance(PatternID patternID, AmountID amountID, SymbolID symbolID, DigitID digitID, OperandID operandID)
    {
        this.patternID  = patternID;
        this.amountID   = amountID;
        this.symbolID   = symbolID;
        this.digitID    = digitID;
        this.operandID  = operandID;
    }
    public PatternID patternID;
    public AmountID  amountID;
    public SymbolID  symbolID;
    public DigitID   digitID;
    public OperandID operandID;
}
