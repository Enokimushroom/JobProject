using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class FirstGameCGController : MonoBehaviour
{
    [SerializeField] private Animator crossFade;
    [SerializeField] private VideoPlayer prologue;
    [SerializeField] private VideoPlayer intro;
    [SerializeField] private CanvasGroup skipHintTxt;
    [SerializeField] private AudioSource audioSource;
    private VideoPlayer currentVP;
    private bool skipHint;
    private bool txtOver;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        skipHintTxt.alpha = 0;
        intro.loopPointReached += EndOfIntro;
        prologue.loopPointReached += EndOfPrologue;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
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
        crossFade.Play("CrossFadeIn");
        //遮挡以切换场景
    }
}
 