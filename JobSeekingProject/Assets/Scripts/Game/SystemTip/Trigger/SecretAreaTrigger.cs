using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretAreaTrigger : TriggerBase
{
    private PolygonCollider2D anotherCol;
    private Collider2D mainCol;
    private Vector3 playerPos;
    private Vector3 originCameraPos;
    private Vector3 offset;

    private void Start()
    {
        offset = GetComponent<Collider2D>().offset;
        anotherCol = transform.GetChild(0).GetComponent<PolygonCollider2D>();
    }

    public override void Action()
    {
        if (playerPos.x > (transform.position + offset).x)
        {
            ToSecretCamera();
        }
        else
        {
            BackMainCamera();
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPos = collision.transform.position;
            Action();
        }
    }

    private void ToSecretCamera()
    {
        GameManager.Instance.CameraFollowPlayer(false);
        GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().enabled = false;
        mainCol = GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().m_BoundingShape2D;
        MusicMgr.Instance.PlaySound("SecretDiscovered", false);
        originCameraPos = GameObject.FindWithTag("MainCamera").transform.position;
        GameManager.Instance.cvc.transform.DOMove(transform.position, 1.0f).onComplete = () =>
        {
            GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().m_BoundingShape2D = anotherCol;
            GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().enabled = true;
            GameManager.Instance.CameraFollowPlayer(true);
        };
    }

    private void BackMainCamera()
    {
        GameManager.Instance.CameraFollowPlayer(false);
        GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().enabled = false;
        GameManager.Instance.cvc.transform.DOMove(originCameraPos, 1.0f).onComplete = () =>
        {
            GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().m_BoundingShape2D = mainCol;
            GameManager.Instance.cvc.GetComponent<CinemachineConfiner>().enabled = true;
            GameManager.Instance.CameraFollowPlayer(true);
        };
    }
}
