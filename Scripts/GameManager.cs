using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public enum GameLevelType
{
    Level1, Level2, Level3, Level4
}

public enum GameRoleType
{
    Chiken, Tiger, Deer, Dog,
    Level4,
}

public class GameManager
{
    private static readonly GameManager instance = new();
    public static GameManager Instance { get => instance; }

    public GameLevelType level = GameLevelType.Level1;
    public bool isPaused = false;

    public UnityAction valuesUpdation;

    private int score = 0;
    private int life = 3;

    public int Score { get => score; }
    public int Life { get => life; }
    public bool WinFlag { get => score >= 20; } 
    public bool IsDead { get => life <= 0; }

    public GameRoleType RoleType
    {
        get
        {
            return level switch {
                GameLevelType.Level1 => GameRoleType.Chiken,
                GameLevelType.Level2 => GameRoleType.Tiger,
                GameLevelType.Level3 => GameRoleType.Deer,
                GameLevelType.Level4 => GameRoleType.Dog,
                _ => GameRoleType.Chiken,
            };
        }
    }

    public void ResetLevelValues()
    {
        score = 0;
        life = 3;
        isPaused = false;
    }

    public void Reset()
    {
        level = GameLevelType.Level1;
        ResetLevelValues();
    }

    public GameLevelType NextLevel()
    {
        return level switch
        {
            GameLevelType.Level1 => GameLevelType.Level2,
            GameLevelType.Level2 => GameLevelType.Level3,
            GameLevelType.Level3 => GameLevelType.Level4,
            GameLevelType.Level4 => GameLevelType.Level1,
            _ => GameLevelType.Level1,
        };
    }

    public void JumpToGameScene()
    {
        string sceneName = level switch
        {
            GameLevelType.Level1 => "Scene14",
            GameLevelType.Level2 => "Scene2",
            GameLevelType.Level3 => "Scene3",
            GameLevelType.Level4 => "Scene14",
            _ => "Scene14",
        };
        ResetLevelValues();
        SceneManager.LoadScene(sceneName);
    }

    public void ScoreIncrease()
    {
        score++;
        valuesUpdation?.Invoke();
    }

    public void LifeDecrease()
    {
        life--;
        life = Mathf.Max(life, 0);
        valuesUpdation?.Invoke();

        Debug.Log("LifeDecrease");
        Debug.Log(life);
    }
}
