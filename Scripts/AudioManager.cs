using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音乐管理类
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get => instance; set => instance = value; }

    private AudioSource bgMusic;
    private AudioSource effectMusic;
    //[HideInInspector]
    private List<AudioSource> audioSourceList = new List<AudioSource>();

    private bool isOpenBg = true;//是否开启背景音乐
    private bool isOpenEffect = true;//是否开启特效音乐

    //保存当前游戏所有的音乐片段
    private Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();
    public List<AudioClip> audioClipList = new List<AudioClip>();//所有音乐片段

    int tempIdnex = 0;
    //public List<AudioClip> ListBgClips = new List<AudioClip>();//多个背景音乐
    void OnDisable()
    {
        
    }

    void OnDestroy()
    {
        audioClipList.Clear();
        audioClipDic.Clear();
    }

    void Awake()
    {
        instance = this;
        bgMusic = gameObject.AddComponent<AudioSource>();
        effectMusic = gameObject.AddComponent<AudioSource>();

        bgMusic.loop = true;
        bgMusic.playOnAwake = false;

        effectMusic.loop = false;//音效不需要循环
        effectMusic.playOnAwake = false;
    }

    void Start()
    {
        for (int i = 0; i < audioClipList.Count; i++)
        {
            if (audioClipList[i] == null) return;
            audioClipDic.Add(audioClipList[i].name, audioClipList[i]);
        }
        //        for (int i = 0; i < ListBgClips.Count; i++)
        //        {
        //            if (ListBgClips[i] == null) return;
        //            audioClipDic.Add(ListBgClips[i].name, ListBgClips[i]);
        //        }
        //audioSourceList.Add(effectMusic);
    }

    void LateUpdate()
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (audioSourceList[i].isPlaying == false)
            {
                audioSourceList[i].clip = null;
            }
        }
    }

    //播放背景音乐，游戏的背景音乐默认只能播放一个
    public void PlayBgMusic(string _name, float volum = 1)
    {
        tempIdnex++;
        StartCoroutine(InvokePlayBG(_name, tempIdnex,volum));
    }

    IEnumerator InvokePlayBG(string _name,int tempIndex, float volum = 1)
    {
        if(tempIndex==1)
            yield return new WaitForSeconds(1.5f);
        else
            yield return new WaitForSeconds(0.01f);

        AudioClip clip = null;
        if (audioClipDic.TryGetValue(_name, out clip))
        {
            //Debug.LogError("背景音乐----------------------------" + clip.name);
            bgMusic.clip = clip;
            bgMusic.volume = volum;
            bgMusic.Play();
        }
        else
            Debug.LogError("没有找到背景音乐----------------------------");
    }
    //播放音效，音效可以播放多个
    public void PlayEffectMusic(string _name, float volumn = 1)
    {
        AudioClip clip = null;
        if (audioClipDic.TryGetValue(_name, out clip))
        {
            //Debug.LogError("播放音效----------------------------" + clip.name);
            for (int i = 0; i < audioSourceList.Count; i++)
            {
                if (audioSourceList[i].clip == null)
                {
                    audioSourceList[i].clip = clip;
                    if (isOpenEffect)
                    {
                        audioSourceList[i].volume = volumn;
                    }
                    else
                    {
                        audioSourceList[i].volume = 0;
                    }
                    audioSourceList[i].Play();
                    return;
                }
            }
            GameObject effectAudio = new GameObject();
            AudioSource audio = effectAudio.AddComponent<AudioSource>();
            audio.loop = false;
            audio.playOnAwake = false;
            audioSourceList.Add(audio);
            audio.clip = clip;
            if (isOpenEffect)
                audio.volume = volumn;
            else
                audio.volume = 0;

            audio.Play();
        }
        else
            Debug.LogError("没有找到音效----------------------------");
    }

    //不播放重复的特效，覆盖音效
    public void PlayEffectMusicNoRepeat(string _name)
    {
        AudioClip clip = null;
        if (audioClipDic.TryGetValue(_name, out clip))
        {
            effectMusic.clip = clip;
            effectMusic.Play();
        }
    }

    //控制背景音乐音量
    public float BgMusicVolume
    {
        get
        {
            return bgMusic.volume;
        }
        set
        {
            bgMusic.volume = value;
        }
    }

    //停止背景音乐
    public void StopBgMusic()
    {
        bgMusic.Stop();
        bgMusic.clip = null;
    }

    //停止音效
    public void StopEffectMusic()
    {
        effectMusic.Stop();
        effectMusic.clip = null;
    }

    //控制音效音量
    public float EffectMusicVolume
    {
        get
        {
            return effectMusic.volume;
        }
        set
        {
            effectMusic.volume = value;

            for (int i = 0; i < audioSourceList.Count; i++)
            {
                audioSourceList[i].volume = value;
            }
        }
    }

    //开启所有音乐的音量
    public void OpenAllAudio()
    {
        isOpenBg = true;
        isOpenEffect = true;
        bgMusic.volume = 1;
        effectMusic.volume = 1;
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            audioSourceList[i].volume = 1;
        }
    }

    //关闭所有音乐的音量
    public void CloseAllAudio()
    {
        isOpenBg = false;
        isOpenEffect = false;
        bgMusic.volume = 0;
        effectMusic.volume = 0;
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            audioSourceList[i].volume = 0;
        }
    }

    //只开启背景音量
    public void OpenBgAudio()
    {
        isOpenBg = true;
        BgMusicVolume = 1;
    }

    //只关闭背景音量
    public void CloseBgAudio()
    {
        isOpenBg = false;
        BgMusicVolume = 0;
    }

    //只开启特效音量
    public void OpenEffectAudio()
    {
        isOpenEffect = true;
        effectMusic.volume = 1;
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            audioSourceList[i].volume = 1;
        }
    }

    //只关闭特效音量
    public void CloseEffectAudio()
    {
        isOpenEffect = false;
        effectMusic.volume = 0;
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            audioSourceList[i].volume = 0;
        }
    }

    //关闭指定特效音量
    public void CloseEffectAudio(string audioName)
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (audioSourceList[i].clip != null && audioSourceList[i].clip.name == audioName)
            {
                audioSourceList[i].clip = null;
                return;
            }
        }
    }
    //关闭所有特效音量
    public void CloseAllEffectAudio()
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (audioSourceList[i].clip != null)
            {
                audioSourceList[i].clip = null;
                return;
            }
        }
    }

    /// <summary>
    /// 背景音乐多个，循环播放
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="isFree">是否是免费</param>
    //    public IEnumerator BgLoop(bool isFree, string clipName = null)
    //    {
    //        if (isFree)
    //        {
    //            PlayBgMusic(clipName);
    //        }
    //        else
    //        {
    //            int currIndex = 0;
    //            while (!isFree)
    //            {
    //                if (currIndex < ListBgClips.Count)
    //                {
    //                    if (!string.IsNullOrEmpty(ListBgClips[currIndex].name))
    //                    {
    //                        PlayBgMusic(ListBgClips[currIndex].name);
    //                    }
    //                }                                       
    //                yield return new WaitForSeconds(ListBgClips[currIndex].length);
    //                if (++currIndex >= ListBgClips.Count) currIndex = 0;
    //            }
    //        }
    //    }

    public void OnClearAudio()
    {
        audioClipList.Clear();
        audioClipDic.Clear();
    }
}