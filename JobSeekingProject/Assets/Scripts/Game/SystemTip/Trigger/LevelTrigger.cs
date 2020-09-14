using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelTrigger : TriggerBase
{
    private BoxCollider2D box;
    private PolygonCollider2D cameraCol;
    private Collider2D originCol;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        cameraCol = GetComponentInChildren<PolygonCollider2D>();
    }

    public override void Action()
    {
        StartCoroutine(triggerAction());
    }

    private IEnumerator triggerAction()
    {
        //关门并且修改碰撞器位置
        transform.GetComponent<Animator>().SetTrigger("Start");
        MusicMgr.Instance.PlaySound("LevelGateCloseAudio", false);
        box.offset = new Vector2(-0.5f, 0);
        box.size = new Vector2(1.3f, 4.68f);
        box.isTrigger = false;
        originCol = GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().m_BoundingShape2D;
        GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().m_BoundingShape2D = cameraCol;
        UIMgr.Instance.MapHintTxt("愚人竞技场");
        yield return new WaitForSeconds(0.5f);
        MusicMgr.Instance.PlaySound("CrowdCheerAudio", false);
        yield return new WaitForSeconds(2.0f);
        LevelManager.Instance.EnqueueDungeon();
    }

    public void End()
    {
        transform.GetComponent<Animator>().SetTrigger("End");
        MusicMgr.Instance.PlaySound("LevelGateOpenAudio", false);
        box.enabled = false;
        GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().m_BoundingShape2D = originCol;

    }
}
