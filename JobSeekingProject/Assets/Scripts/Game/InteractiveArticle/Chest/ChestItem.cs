using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    private ItemInfo item = new ItemInfo();
    private int count = 2;
    private AudioSource loopAudio;

    public void SetItemID(int id,int num)
    {
        item.id = id;
        item.num = num;
    }

    private void Start()
    {
        MusicMgr.Instance.PlaySound("ChestItemLoopAudio", true, (o) => { loopAudio = o; });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            count--;
            if (count == 0)
            {
                transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                transform.GetComponent<CircleCollider2D>().isTrigger = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && Input.GetKeyDown(KeyCodeMgr.Instance.Interact.CurrentKey))
        {
            Debug.Log("获取物品");
            MusicMgr.Instance.StopSound(loopAudio);
            MusicMgr.Instance.PlaySound("ChestItemPickUpAudio", false);
            GameDataMgr.Instance.GetItem(item);
            transform.GetChild(0).gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
