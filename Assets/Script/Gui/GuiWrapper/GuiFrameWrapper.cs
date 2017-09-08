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

    protected Dictionary<string, GameObject> GameObjectDict
    {
        get;
        private set;
    }
    protected Dictionary<string, Button> ButtonDict
    {
        get;
        private set;
    }
    protected Dictionary<string, Toggle> ToggleDict
    {
        get;
        private set;
    }
    protected Dictionary<string, Dropdown> DropdownDict
    {
        get;
        private set;
    }

    private delegate void ButtonDelegate(Button btn);
    private delegate void ToggleDelegate(Toggle tgl);
    private delegate void DropdownDelegate(Dropdown dpd);

    private ButtonDelegate btnDelegate;
    private ToggleDelegate tglDelegate;
    private DropdownDelegate dpdDelegate;

    protected virtual void Init()
    {
        btnDelegate = GetComponent<GuiFrameWrapper>().OnButtonClick;
        tglDelegate = GetComponent<GuiFrameWrapper>().OnToggleClick;
        dpdDelegate = GetComponent<GuiFrameWrapper>().OnDropdownClick;
        InitGameObjectDict();
        InitUnSelectableGui();
        InitSelectableGui();
    }

    private void InitGameObjectDict()
    {
        GameObjectDict = new Dictionary<string, GameObject>();
        Transform[] gameObjectArray = GetComponentsInChildren<Transform>(true);
        for(int i = 0; i < gameObjectArray.Length; i++)
        {
            GameObjectDict.Add(gameObjectArray[i].name, gameObjectArray[i].gameObject);
            //MyDebug.LogYellow(gameObjectArray[i].name);
        }
    }

    protected void InitUnSelectableGui()
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

    protected void InitSelectableGui()
    {
        if (btnDelegate != null) InitButton();
        if (tglDelegate != null) InitToggle();
        if (dpdDelegate != null) InitDropdown();
    }
    private void InitButton()
    {
        Button[] buttonArray = GetComponentsInChildren<Button>(true);
        ButtonDict = new Dictionary<string, Button>();
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
    }
    private void InitToggle()
    {
        Toggle[] toggleArray = GetComponentsInChildren<Toggle>(true);
        ToggleDict = new Dictionary<string, Toggle>();
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
    }
    private void InitDropdown()
    {
        Dropdown[] dropdownArray = GetComponentsInChildren<Dropdown>(true);
        DropdownDict = new Dictionary<string, Dropdown>();
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

