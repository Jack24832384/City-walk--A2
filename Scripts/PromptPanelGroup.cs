using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PromptPanelType
{
    Win, GameOver, Pause, Setting, Menu,
}

public class PromptPanelGroup : MonoBehaviour
{
    private Dictionary<PromptPanelType, Image> panelDict = new();

    public UnityAction WinNextAction;
    public UnityAction WinReplayAction;
    public UnityAction GameOverReplayAction;
    public UnityAction GameOverMenuAction;
    public UnityAction PauseBackAction;
    public UnityAction PauseMenuAction;
    public UnityAction SettingDoneAction;

    private bool nextButtonTitleIsNext = false;

    public void SetNextButtonTitle(bool isNext)
    {
        nextButtonTitleIsNext = isNext;
        if (panelDict.ContainsKey(PromptPanelType.Win))
        {
            Image targetPanel = panelDict[PromptPanelType.Win];
            Button button = ToolUtils.FetchButton(targetPanel.transform, "btnNext");
            if (button != null)
            {
                button.GetComponentInChildren<Text>().text = isNext ? "NEXT" : "MENU";
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanel(PromptPanelType type)
    {
        if (panelDict.ContainsKey(type))
        {
            gameObject.SetActive(true);
            Image targetPanel = panelDict[type];
            if (!targetPanel.gameObject.activeSelf)
            {
                foreach (var item in panelDict.Values)
                {
                    item.gameObject.SetActive(item == targetPanel);
                }
            }
        }
    }
    void Start()
    {
        FetchElements();
        AddButtonListeners();
        Hide();
    }

    void AddButtonListeners()
    {
        if (panelDict.ContainsKey(PromptPanelType.Win))
        {
            Image winPanel = panelDict[PromptPanelType.Win];
            ToolUtils.AddButtonAction(winPanel.transform, "btnNext", () => { WinNextAction?.Invoke(); });
            ToolUtils.AddButtonAction(winPanel.transform, "btnAgain", () => { WinReplayAction?.Invoke(); });
            Button button = ToolUtils.FetchButton(winPanel.transform, "btnNext");
            if (button != null)
            {
                button.GetComponentInChildren<Text>().text = nextButtonTitleIsNext ? "NEXT" : "MENU";
            }
        }
        if (panelDict.ContainsKey(PromptPanelType.GameOver))
        {
            Image gameOverPanel = panelDict[PromptPanelType.GameOver];
            ToolUtils.AddButtonAction(gameOverPanel.transform, "btnAgain", () => { GameOverReplayAction?.Invoke(); });
            ToolUtils.AddButtonAction(gameOverPanel.transform, "btnMenu", () => { GameOverMenuAction?.Invoke(); });
        }
        if (panelDict.ContainsKey(PromptPanelType.Pause))
        {
            Image pausePanel = panelDict[PromptPanelType.Pause];
            ToolUtils.AddButtonAction(pausePanel.transform, "btnBack", () => { PauseBackAction?.Invoke(); });
            ToolUtils.AddButtonAction(pausePanel.transform, "btnMenu", () => { PauseMenuAction?.Invoke(); });
        }
        if (panelDict.ContainsKey(PromptPanelType.Menu))
        {
            Image menuPanel = panelDict[PromptPanelType.Menu];
        }
        if (panelDict.ContainsKey(PromptPanelType.Setting))
        {
            Image settingPanel = panelDict[PromptPanelType.Setting];
            ToolUtils.AddButtonAction(settingPanel.transform, "btnBack", () => { SettingDoneAction?.Invoke(); });
            Slider slider = ToolUtils.FetchItem<Slider>(settingPanel.transform, "Slider");
            slider.value = AudioListener.volume;
            slider.onValueChanged.AddListener( (value) =>
            {
                AudioListener.volume = value;
            });
        }
    }

    void FetchElements()
    {
        panelDict = new();
        Dictionary<string, PromptPanelType> panelTypeDict = new()
        {
            { "settingPanel", PromptPanelType.Setting },
            { "winPanel", PromptPanelType.Win },
            { "menuPanel", PromptPanelType.Menu },
            { "gameOver", PromptPanelType.GameOver },
            { "pausePanel", PromptPanelType.Pause },
        };
        int childrenCount = transform.childCount;
        if (childrenCount > 0)
        {
            for (int i = 0; i < childrenCount; i++)
            {
                Image item = transform.GetChild(i).GetComponent<Image>();
                if (item != null)
                {
                    string name = item.name;
                    if (panelTypeDict.ContainsKey(name))
                    {
                        item.gameObject.SetActive(false);
                        PromptPanelType type = panelTypeDict[name];
                        panelDict[type] = item;
                    }
                }
            }
        }
    }
}
