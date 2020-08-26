using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    //唯一的背景音乐组件
    private AudioSource bgMusic = null;
    //背景音乐声音大小
    public float bgValue { get; private set; } = 1;

    //音效依附对象
    private GameObject soundObj = null;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效声音大小
    public float soundValue { get; private set; } = 1;

    //主音量大小
    public float mainValue { get; private set; } = 1;

    public MusicMgr()
    {
        MonoMgr.Instance.AddUpdateListener(Update);
    }

    private void Update()
    {
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    public void ChangeMainValue(float v)
    {
        mainValue = v;
        if (bgMusic == null && soundList.Count == 0) return;

        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = soundValue * mainValue;
        }

        bgMusic.volume = bgValue * mainValue;
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBGMusic(string name)
    {
        if(bgMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BgMusic";
            bgMusic = obj.AddComponent<AudioSource>();
        }

        //异步加载背景音乐 加载完成后播放
        ResMgr.Instance.LoadAsync<AudioClip>(name, (clip) =>
        {
            if (bgMusic == null)
            {
                GameObject obj = new GameObject();
                obj.name = "BgMusic";
                bgMusic = obj.AddComponent<AudioSource>();
            }
            bgMusic.clip = clip;
            bgMusic.volume = bgValue * mainValue;
            bgMusic.loop = true;
            bgMusic.Play();
        });
    }

    public void PauseBGMusic()
    {
        if (bgMusic == null)
            return;

        bgMusic.Pause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGMusic()
    {
        if (bgMusic == null)
            return;

        bgMusic.Stop();
    }

    public void ChangeBGValue(float v)
    {
        bgValue = v;
        if (bgMusic == null)
            return;

        bgMusic.volume = bgValue;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }

        //当音效资源异步加载结束后 再添加一个音效
        ResMgr.Instance.LoadAsync<AudioClip>(name, (clip) =>
        {
            if (soundObj == null) return;
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);
            if (callback != null)
                callback(source);
        });
    }

    /// <summary>
    /// 改变音效声音大小
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for(int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = value * mainValue;
        }
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }

    public void MusicClear()
    {
        soundList.Clear();
        soundObj = null;
        bgMusic = null;
    }
}
