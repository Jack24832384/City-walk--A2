using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPage : MonoBehaviour
{
    public Image startPanel;
    public Button playButton;
    public Button settingButton;
    public Button helpButton;

    public Image levelPanel;
    public Button levelButton0;
    public Button levelButton1;
    public Button levelButton2;
    public Button levelButton3;
    public Button menuButton;

    public Image settingPanel;
    public Slider volumeSlider;
    public Button doneButton;

    public Image helpPanel;
    public Button okButton;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        settingButton.onClick.AddListener(OnSettingButtonClick);
        helpButton.onClick.AddListener(OnHelpButtonClick);
        levelButton0.onClick.AddListener(() => { OnLevelButtonClick(levelButton0); });
        levelButton1.onClick.AddListener(() => { OnLevelButtonClick(levelButton1); });
        levelButton2.onClick.AddListener(() => { OnLevelButtonClick(levelButton2); });
        levelButton3.onClick.AddListener(() => { OnLevelButtonClick(levelButton3); });
        menuButton.onClick.AddListener(OnMenuButtonClick);
        doneButton.onClick.AddListener(OnDoneButtonClick);
        okButton.onClick.AddListener(OnOkButtonClick);
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
        GameManager.Instance.valuesUpdation = null;
        ToolUtils.PlayBgMusic();
    }

    void OnVolumeSliderValueChanged(float value)
    {
        AudioListener.volume = value;
    }

    void OnPlayButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        GameManager.Instance.Reset();
        //GameManager.Instance.JumpToGameScene();
        ShowLevelPanel();
    }

    void OnSettingButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        ShowSettingPanel();
    }

    void OnHelpButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        ShowHelpPanel();
    }

    void OnDoneButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        ShowMenuPanel();
    }

    void OnOkButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        ShowMenuPanel();
    }

    void OnLevelButtonClick(Button sender)
    {
        ToolUtils.PlayButtonClickMusic();
        GameLevelType level;
        if (sender == levelButton0)
        {
            level = GameLevelType.Level1;
        }
        else if (sender == levelButton1)
        {
            level = GameLevelType.Level2;
        }
        else if (sender == levelButton2)
        {
            level = GameLevelType.Level3;
        }
        else
        {
            level = GameLevelType.Level4;
        }
        GameManager.Instance.level = level;
        GameManager.Instance.JumpToGameScene();
    }

    void OnMenuButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        ShowMenuPanel();
    }

    void ShowLevelPanel()
    {
        levelPanel.gameObject.SetActive(true);
        startPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        helpPanel.gameObject.SetActive(false);
    }

    void ShowMenuPanel()
    {
        levelPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        settingPanel.gameObject.SetActive(false);
        helpPanel.gameObject.SetActive(false);
    }

    void ShowSettingPanel()
    {
        levelPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(true);
        helpPanel.gameObject.SetActive(false);
    }

    void ShowHelpPanel()
    {
        levelPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        helpPanel.gameObject.SetActive(true);
    }
}
