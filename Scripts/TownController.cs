using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TownController : MonoBehaviour 
{
    private Text scoreText;
    private Text lifeText;
    private PromptPanelGroup panelGroup;

    void Start()
    {
        scoreText = ToolUtils.FetchText(transform, "valuePanel/scorePanel/value");
        lifeText = ToolUtils.FetchText(transform, "valuePanel/lifePanel/value");

        GameManager manager = GameManager.Instance;
        manager.valuesUpdation = OnGameValuesUpdation;
        scoreText.text = manager.Score.ToString();
        lifeText.text = manager.Life.ToString();

        SetupPanelGroup();
        ToolUtils.PlayBgMusic();
    }

    void SetupPanelGroup()
    {
        ToolUtils.AddButtonAction(transform, "btnPause", OnPauseButtonClick);
        ToolUtils.AddButtonAction(transform, "btnHelp", OnHelpButtonClick);
        panelGroup = ToolUtils.FetchItem<PromptPanelGroup>(transform, "promptPanelGroup");
        panelGroup.WinNextAction = OnNextButtonClick;
        panelGroup.WinReplayAction = OnAgainButtonClick;
        panelGroup.GameOverReplayAction = OnAgainButtonClick;
        panelGroup.GameOverMenuAction = OnMenuButtonClick;
        panelGroup.PauseBackAction = OnPanelBackButtonClick;
        panelGroup.PauseMenuAction = OnMenuButtonClick;
        panelGroup.SettingDoneAction = OnPanelBackButtonClick;
        panelGroup.SetNextButtonTitle(GameManager.Instance.NextLevel() != GameLevelType.Level1);
    }

    void OnGameValuesUpdation()
    {
        GameManager manager = GameManager.Instance;
        lifeText.text = manager.Life.ToString();
        scoreText.text = manager.Score.ToString();
        if (manager.IsDead)
        {
            panelGroup.ShowPanel(PromptPanelType.GameOver);
        }
        if (manager.WinFlag)
        {
            panelGroup.ShowPanel(PromptPanelType.Win);
        }
    }

    void OnPauseButtonClick()
    {
        if (GameManager.Instance.isPaused) return;

        ToolUtils.PlayButtonClickMusic();
        GameManager.Instance.isPaused = true;
        panelGroup.ShowPanel(PromptPanelType.Pause);
    }

    void OnHelpButtonClick()
    {
        if (GameManager.Instance.isPaused) return;

        ToolUtils.PlayButtonClickMusic();
        GameManager.Instance.isPaused = true;
        panelGroup.ShowPanel(PromptPanelType.Setting);
    }

    void OnPanelBackButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        panelGroup.Hide();
        GameManager.Instance.isPaused = false;
    }

    void OnAgainButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        GameManager.Instance.ResetLevelValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnNextButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        GameManager manager = GameManager.Instance;
        manager.level = manager.NextLevel();
        if (manager.level == GameLevelType.Level1)
        {
            ToolUtils.JumpToMenuPage();
        }
        else
        {
            manager.JumpToGameScene();
        }
    }

    void OnMenuButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        ToolUtils.JumpToMenuPage();
    }
}
