using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geo : MonoBehaviour
{
    private Transform target;
    private bool FlyToPlayer;
    private bool isGrounded;
    private Vector2 speed = Vector2.zero;
    [Tooltip("金钱量")]
    private MoneyDetails md;
    [Tooltip("飞速修正系数")]
    public float minModification = 7;
    public float maxModification = 11;

    private void Start()
    {
        target = GameManager.Instance.playerGO.transform;
        FlyToPlayer = false;
        md.moneyAmount = 2;
        md.moneySource = MoneyDetails.Source.PickUp;
    }

    // Update is called once per frame
    private void Update()
    {
        if (FlyToPlayer)
        {
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref speed, Time.deltaTime * Random.Range(minModification, maxModification));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collector"))
        {
            FlyToPlayer = true;
            transform.GetComponent<CircleCollider2D>().isTrigger = true;
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerStatus.Instance.ChangeMoney(md);
            int index = Random.Range(1, 3);
            string audioName = "GeoCollect0" + index.ToString();
            MusicMgr.Instance.PlaySound(audioName, false);
            PEManager.Instance.GetParticleObjectDuringTime("GeoCollectLight", collision.transform, Vector3.zero, Vector3.one, Quaternion.identity, 0.5f);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            int index = Random.Range(1, 3);
            string audioName = "GeoHitGround0" + index.ToString();
            MusicMgr.Instance.PlaySound(audioName, false);
        }
    }
}
