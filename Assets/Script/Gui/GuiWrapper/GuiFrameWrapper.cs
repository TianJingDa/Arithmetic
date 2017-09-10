using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary>
/// 所有GUI显示层的基类，先用数据初始化再找物体
/// </summary>
public abstract class GuiFrameWrapper : MonoBehaviour
{
    [HideInInspector]
    public GuiFrameID id;

    private delegate void ButtonDelegate(Button btn);
    private delegate void ToggleDelegate(Toggle tgl);
    private delegate void DropdownDelegate(Dropdown dpd);
    
    protected void Init()
    {
        InitImage();
        InitText();
        ButtonDelegate   btnDelegate = GetComponent<GuiFrameWrapper>().OnButtonClick;
        ToggleDelegate   tglDelegate = GetComponent<GuiFrameWrapper>().OnToggleClick;
        DropdownDelegate dpdDelegate = GetComponent<GuiFrameWrapper>().OnDropdownClick;
        Dictionary<string, GameObject> GameObjectDict = InitGameObjectDict();
        Dictionary<string, Button> ButtonDict = InitButton(btnDelegate, GameObjectDict);
        Dictionary<string, Toggle> ToggleDict = InitToggle(tglDelegate, GameObjectDict);
        Dictionary<string, Dropdown> DropdownDict = InitDropdown(dpdDelegate, GameObjectDict);
        GetComponent<GuiFrameWrapper>().OnStart(GameObjectDict, ButtonDict, ToggleDict, DropdownDict);
    }

    private Dictionary<string, GameObject> InitGameObjectDict()
    {
        Dictionary<string, GameObject> GameObjectDict = new Dictionary<string, GameObject>();
        Transform[] gameObjectArray = GetComponentsInChildren<Transform>(true);
        for(int i = 0; i < gameObjectArray.Length; i++)
        {
            //MyDebug.LogYellow(gameObjectArray[i].name);
            GameObjectDict.Add(gameObjectArray[i].name, gameObjectArray[i].gameObject);
        }
        return GameObjectDict;
    }

    protected void RefreshGui()
    {
        InitText();
        InitImage();
    }
    private void InitImage()
    {
        return;
        Image[] imageArray = gameObject.GetComponentsInChildren<Image>(true);
        if (imageArray.Length == 0)
        {
            return;
        }
        for (int i = 0; i < imageArray.Length; i++)
        {
            if (imageArray[i].index == "")
            {
                continue;
            }
            Sprite sprite = GameManager.Instance.GetSprite(imageArray[i].index);
            if (sprite != null)
            {
                imageArray[i].sprite = sprite;
            }
            else
            {
                MyDebug.LogYellow("Can not load Sprite:" + imageArray[i].index);
            }
        }
    }
    private void InitText()
    {
        Text[] textArray = gameObject.GetComponentsInChildren<Text>(true);
        if (textArray.Length == 0)
        {
            return;
        }
        //Font curFont = GameManager.Instance.GetFont();
        for (int i = 0; i < textArray.Length; i++)
        {
            if (textArray[i].index == "")
            {
                continue;
            }
            //textArray[i].font = curFont;
            //textArray[i].color = GameManager.Instance.GetColor(textArray[i].index);
            textArray[i].text = GameManager.Instance.GetMutiLanguage(textArray[i].index);
        }
    }


    private Dictionary<string, Button> InitButton(ButtonDelegate btnDelegate, Dictionary<string, GameObject> GameObjectDict)
    {
        Button[] buttonArray = GetComponentsInChildren<Button>(true);
        if (buttonArray.Length == 0) return null;
        Dictionary<string, Button> ButtonDict = new Dictionary<string, Button>();
        for(int i = 0; i < buttonArray.Length; i++)
        {
            Button curButton = buttonArray[i];
            curButton.onClick.AddListener(() => btnDelegate(curButton));
            ButtonDict.Add(curButton.name, curButton);
            if (!GameObjectDict.Remove(curButton.name))
            {
                MyDebug.LogYellow(curButton.name + " can NOT be removed from GameObjectDict!!!");
            }
        }
        return ButtonDict;
    }
    private Dictionary<string, Toggle> InitToggle(ToggleDelegate tglDelegate, Dictionary<string, GameObject> GameObjectDict)
    {
        Toggle[] toggleArray = GetComponentsInChildren<Toggle>(true);
        if (toggleArray.Length == 0) return null;
        Dictionary<string, Toggle> ToggleDict = new Dictionary<string, Toggle>();
        for(int i = 0; i < toggleArray.Length; i++)
        {
            Toggle curToggle = toggleArray[i];
            curToggle.onValueChanged.AddListener(value => tglDelegate(curToggle));
            ToggleDict.Add(curToggle.name, curToggle);
            if (!GameObjectDict.Remove(curToggle.name))
            {
                MyDebug.LogYellow(curToggle.name + " can NOT be removed from GameObjectDict!!!");
            }
        }
        return ToggleDict;
    }
    private Dictionary<string, Dropdown> InitDropdown(DropdownDelegate dpdDelegate, Dictionary<string, GameObject> GameObjectDict)
    {
        Dropdown[] dropdownArray = GetComponentsInChildren<Dropdown>(true);
        if (dropdownArray.Length == 0) return null;
        Dictionary<string, Dropdown>  DropdownDict = new Dictionary<string, Dropdown>();
        for(int i = 0; i < dropdownArray.Length; i++)
        {
            Dropdown curDropdown = dropdownArray[i];
            curDropdown.onValueChanged.AddListener(index => dpdDelegate(curDropdown));
            DropdownDict.Add(curDropdown.name, curDropdown);
            if (!GameObjectDict.Remove(curDropdown.name))
            {
                MyDebug.LogYellow(curDropdown.name + " can NOT be removed from GameObjectDict!!!");
            }
        }
        return DropdownDict;
    }

    protected virtual void OnStart(Dictionary<string, GameObject> GameObjectDict, 
                                   Dictionary<string, Button>     ButtonDict, 
                                   Dictionary<string, Toggle>     ToggleDict, 
                                   Dictionary<string, Dropdown>   DropdownDict)
    {

    }
    protected virtual void OnButtonClick(Button btn)
    {
        if (!btn)
        {
            MyDebug.LogYellow("Button is NULL!");
            return;
        }
    }
    protected virtual void OnToggleClick(Toggle tgl)
    {
        if (!tgl)
        {
            MyDebug.LogYellow("Toggle is NULL!");
            return;
        }
    }
    protected virtual void OnDropdownClick(Dropdown dpd)
    {
        if (!dpd)
        {
            MyDebug.LogYellow("Dropdown is NULL!");
            return;
        }
    }

    protected void GuiAppear() { }
    protected void GuiDisappear() { }
}

