using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class FirstGameCGController : MonoBehaviour
{
    [SerializeField] private VideoPlayer prologue;
    [SerializeField] private VideoPlayer intro;
    [SerializeField] private CanvasGroup skipHintTxt;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float bufferTime;
    private VideoPlayer currentVP;
    private bool skipHint;
    private bool txtOver;
    private Animator anim;
    private bool buffer;
    private float pressTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
        skipHintTxt.alpha = 0;
        intro.loopPointReached += EndOfIntro;
        prologue.loopPointReached += EndOfPrologue;
        buffer = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && buffer)
        {
            buffer = false;
            pressTime = Time.time;
            if (skipHint)
            {
                skipHint = false;
                skipHintTxt.alpha = 0;
                if (!txtOver)
                {
                    anim.Play("TxtAnim", 0, 751);
                    audioSource.Stop();
                }
                else
                    currentVP.frame = (int)currentVP.frameCount;
            }
            else
            {
                skipHint = true;
                StartCoroutine(DisplayHint());
            }
        }
        if (pressTime + bufferTime <= Time.time && !buffer)
        {
            buffer = true;
        }
    }

    IEnumerator DisplayHint()
    {
        skipHint = true;
        while (skipHintTxt.alpha != 1)
        {
            skipHintTxt.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PlayVedio()
    {
        txtOver = true;
        currentVP = prologue;
        currentVP.Play();
    }

    private void EndOfPrologue(VideoPlayer source)
    {
        currentVP = intro;
        currentVP.Play();
    }

    private void EndOfIntro(VideoPlayer source)
    {
        anim.SetTrigger("FadeOut");
        //遮挡以切换场景
    }

    private void LoadScene()
    {
        Debug.Log("核心数据加载成功");
        //为角色状态管理器作为观察者订阅核心数据的更新
        PlayerStatus.Instance.Init();
        //自定义键盘初始化
        KeyCodeMgr.Instance.Init();
        //已完成和未完成任务的初始化
        TaskMgr.Instance.Init();
        //关卡加载器初始化
        LevelManager.Instance.Init();
        //初始化地图管理器，读取角色档案所在地图（场景）
        MapMgr.Instance.Init();
        //已完成和未完成任务的初始化（交给地图加载器调用）
        //生成真正的人物角色（交给地图加载器调用）
        //初始化任务者管理器，记录并转移中转任务（交给地图加载器调用）
        //通知各位订阅者更新
        GameDataMgr.Instance.playerInfo.Notify();
        InputMgr.Instance.StartOrEndCheck(true);
        Debug.Log("GDM流程结束");
    }
}
 