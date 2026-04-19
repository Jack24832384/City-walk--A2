using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Progress;

enum CharactorType
{
    Chiken, Tiger, Deer, Dog,
}

public class ScoreController : MonoBehaviour
{
    private Text scoreText;
    private Image coinIcon;
    private Image cornIcon;
    private Image meatIcon;
    private Text lifeText;
    private PromptPanelGroup panelGroup;

    public GameObject coinGroup;
    public GameObject cornGroup;
    public GameObject meatGroup;

    public GameObject Chicken;
    public GameObject Tiger;
    public GameObject Deer;
    public GameObject Dog;

    List<Vector3> animalLocations;
    List<Vector3> animalRotatios;

    void Start()
    {
        scoreText = ToolUtils.FetchText(transform, "valuePanel/scorePanel/value");
        coinIcon = ToolUtils.FetchImage(transform, "valuePanel/scorePanel/coinIcon");
        cornIcon = ToolUtils.FetchImage(transform, "valuePanel/scorePanel/cornIcon");
        meatIcon = ToolUtils.FetchImage(transform, "valuePanel/scorePanel/meatIcon");
        lifeText = ToolUtils.FetchText(transform, "valuePanel/lifePanel/value");
        ToolUtils.AddButtonAction(transform, "btnPause", OnPauseButtonClick);
        ToolUtils.AddButtonAction(transform, "btnHelp", OnHelpButtonClick);

        List<GameObject> animalList = new()
        {
            Chicken, Tiger, Deer, Dog
        };
        animalLocations = new();
        animalRotatios = new();
        foreach (var item in animalList)
        {
            animalLocations.Add(item.transform.localPosition);
            animalRotatios.Add(item.transform.localEulerAngles);
        }

        GameManager manager = GameManager.Instance;
        manager.valuesUpdation = OnGameValuesUpdation;
        scoreText.text = manager.Score.ToString();
        lifeText.text = manager.Life.ToString();

        SetupPanelGroup();
        StartGame();
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
        manager.ResetLevelValues();
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

    void OnPanelBackButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        panelGroup.Hide();
        GameManager.Instance.isPaused = false;
    }

    void OnMenuButtonClick()
    {
        ToolUtils.PlayButtonClickMusic();
        ToolUtils.JumpToMenuPage();
    }

    void StartGame()
    {
        CharactorType animal = GameManager.Instance.RoleType switch 
        { 
            GameRoleType.Chiken => CharactorType.Chiken,
            GameRoleType.Tiger => CharactorType.Tiger,
            GameRoleType.Deer => CharactorType.Deer,
            GameRoleType.Dog => CharactorType.Dog,
            _ => CharactorType.Chiken,
        };
        ResetCurrentAnimal(animal);
    }

    private void ResetCurrentAnimal(CharactorType type)
    {
        List<CharactorType> typeList = new()
        {
            CharactorType.Chiken,
            CharactorType.Tiger,
            CharactorType.Deer,
            CharactorType.Dog,
        };
        List<GameObject> animalList = new()
        {
            Chicken, Tiger, Deer, Dog,
        };
        for (int i = 0; i < animalList.Count; i++)
        {
            GameObject animal = animalList[i];
            animal.transform.localPosition = animalLocations[i];
            animal.transform.localEulerAngles = animalRotatios[i];
            animal.transform.parent.gameObject.SetActive(typeList[i] == type);
        }
        GameObject visibleGroup = type switch
        {
            CharactorType.Chiken => coinGroup,
            CharactorType.Tiger => meatGroup,
            CharactorType.Deer => cornGroup,
            CharactorType.Dog => coinGroup,
            _ => coinGroup,
        };
        coinGroup.SetActive(visibleGroup == coinGroup);
        cornGroup.SetActive(visibleGroup == cornGroup);
        meatGroup.SetActive(visibleGroup == meatGroup);
        int childrenCount = visibleGroup.transform.childCount;
        if (childrenCount > 0)
        {
            for (int i = 0; i < childrenCount; i++)
            {
                visibleGroup.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        
        Image visibleIcon = type switch
        {
            CharactorType.Chiken => coinIcon,
            CharactorType.Tiger => meatIcon,
            CharactorType.Deer => cornIcon,
            CharactorType.Dog => coinIcon,
            _ => coinIcon,
        };
        coinIcon.gameObject.SetActive(visibleIcon == coinIcon);
        cornIcon.gameObject.SetActive(visibleIcon == cornIcon);
        meatIcon.gameObject.SetActive(visibleIcon == meatIcon);

        GameManager manager = GameManager.Instance;
        manager.ResetLevelValues();
        scoreText.text = manager.Score.ToString();
        lifeText.text = manager.Life.ToString();
    }
}
