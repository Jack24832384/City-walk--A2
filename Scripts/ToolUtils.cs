using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToolUtils
{
    public static Text FetchText(Transform transform, string path)
    {
        return FetchItem<Text>(transform, path);
    }

    public static Graphic FetchPanel(Transform transform, string path)
    {
        return FetchItem<Graphic>(transform, path);
    }

    public static Graphic FetchGraphic(Transform transform, string path)
    {
        return FetchItem<Graphic>(transform, path);
    }

    public static Image FetchImage(Transform transform, string path)
    {
        return FetchItem<Image>(transform, path);
    }

    public static Button FetchButton(Transform transform, string path)
    {
        return FetchItem<Button>(transform, path);
    }

    public static InputField FetchInputField(Transform transform, string path)
    {
        return FetchItem<InputField>(transform, path);
    }

    public static void AddButtonAction(Transform transform, string path, UnityAction call)
    {
        transform.Find(path).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(call);
    }

    public static T FetchItem<T>(Transform transform, string path)
    {
        return transform.Find(path).GetComponent<T>();
    }

    public static List<Button> FetchButtonList(Transform transform, string path, string prefix, int count)
    {
        return FetchItemList<Button>(transform, path, prefix, count);
    }

    public static List<Image> FetchImageList(Transform transform, string path, string prefix, int count)
    {
        return FetchItemList<Image>(transform, path, prefix, count);
    }

    public static List<Graphic> FetchPanelList(Transform transform, string path, string prefix, int count)
    {
        return FetchItemList<Graphic>(transform, path, prefix, count);
    }

    public static List<Text> FetchTextList(Transform transform, string path, string prefix, int count)
    {
        return FetchItemList<Text>(transform, path, prefix, count);
    }

    public static List<T> FetchItemList<T>(Transform transform, string path, string prefix, int count)
    {
        List<T> list = new List<T>();
        if (count <= 0)
        {
            return list;
        }

        for (int i = 0; i < count; i++)
        {
            string localPath = path;
            if (path != null && path.Length > 0)
            {
                if ((path + "/").Contains("//") == false)
                {
                    localPath = path + "/";
                }
            }
            string fullPath = localPath + prefix + i.ToString();
            T item = transform.Find(fullPath).GetComponent<T>();
            if (item != null)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static void JumpToMenuPage()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public static void PlayBgMusic()
    {
        string bgMusic = "";
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Contains("Menu"))
        {
            bgMusic = "bg1";
        }
        else if (sceneName.Contains("2"))
        {
            bgMusic = "bg2";
        }
        else if (sceneName.Contains("3")) 
        {
            bgMusic = "bg3";
        }
        else
        {
            if (GameManager.Instance.level == GameLevelType.Level1)
            {
                bgMusic = "bg1";
            }
            else
            {
                bgMusic = "bg4";
            }
        }
        AudioManager.Instance.PlayBgMusic(bgMusic);
    }

    public static void PlayButtonClickMusic()
    {
        AudioManager.Instance.PlayEffectMusic("btn_click");
    }
}