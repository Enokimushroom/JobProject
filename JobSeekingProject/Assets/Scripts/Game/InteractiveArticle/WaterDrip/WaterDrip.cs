using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrip : MonoBehaviour
{
    private Animator anim;
    private Vector3 originPos;
    private Rigidbody2D rb;
    private int minReadyDrip = 5;
    private int maxReadyDrip = 15;
    private float lastReadyTime;
    private bool drop;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originPos = transform.position;
        lastReadyTime = Time.time + Random.Range(minReadyDrip, maxReadyDrip);
    }

    private void Update()
    {
        if (Time.time >= lastReadyTime && !drop)
        {
            drop = true;
            anim.SetTrigger("Drop");
        }
    }

    public void Reset()
    {
        lastReadyTime = Time.time + Random.Range(minReadyDrip, maxReadyDrip);
        drop = false;
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = originPos;
    }

    public void Drop()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            int index = Random.Range(1, 5);
            string audioName = "WaterDrip0" + index.ToString();
            MusicMgr.Instance.PlaySound(audioName, false);
            anim.SetTrigger("OnGround");
        }
    }

}
