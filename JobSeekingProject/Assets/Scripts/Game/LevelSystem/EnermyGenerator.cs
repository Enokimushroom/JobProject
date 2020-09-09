using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class EnermyGenerator : MonoBehaviour
{
    public event Action generatorHandler;
    [HideInInspector]public Vector2 generatePos;
    Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        MusicMgr.Instance.PlaySound("ColCageAppearAudio", false);
    }

    public void BoolAnim(string para)
    {
        anim.SetBool(para, true); 
    }

    void EnermyGenerate()
    {
        MusicMgr.Instance.PlaySound("ColCageOpenAudio", false);
        generatorHandler?.Invoke();
    }

    void Disappear()
    {
        float temp = 0;
        DOTween.To(() => temp, x => temp = x, 1, 2.0f).OnStepComplete(() =>
        {
            MusicMgr.Instance.PlaySound("ColCageDisappearAudio", false);
            transform.DOMove(generatePos, 1.0f).onComplete = () =>
            {
                Destroy(this.gameObject, 1.0f);
            };
        });
    }
}
