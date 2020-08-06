using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIButton : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void MousePointerEnter()
    {
        animator.SetBool("Selected", true);
    }

    public virtual void MousePointerExit()
    {
        animator.SetBool("Selected", false);
    }

    public virtual void MousePointerClick()
    {
        animator.SetTrigger("Pressed");
    }

    public void PlayOneShot(string clipName)
    {
        MusicMgr.Instance.PlaySound(clipName, false);
    }
}
