using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lift : MonoBehaviour
{
    public bool moving { get; set; }
    public bool up { get; set; }
    public bool down { get; set; }
    [SerializeField] private float startPosY;
    [SerializeField] private float endPosY;
    [SerializeField] private float transitionTime;
    [SerializeField] private float waitingTime;

    private Collider2D liftTrigger;

    //111,13
    //100,-16
    //-13.5
    //15

    private void Start()
    {
        up = false;
        down = true;
        liftTrigger = transform.parent.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (up && !moving)
            {
                moving = true;
                StartCoroutine(Down());
            }
            else if (down && !moving)
            {
                moving = true;
                StartCoroutine(Up());
            }
        }
    }

    private IEnumerator Down()
    {
        yield return new WaitForSeconds(waitingTime);
        GameManager.Instance.playerGO.transform.SetParent(transform);
        Tween t = transform.DOLocalMoveY(startPosY, transitionTime);
        t.onUpdate = () => { GameManager.Instance.playerGO.GetComponent<Rigidbody2D>().velocity = Vector2.zero; };
        t.onComplete = () =>
        {
            up = false;
            down = true;
            moving = false;
            liftTrigger.offset = new Vector2(111, 13);
            GameManager.Instance.playerGO.transform.SetParent(null);
        };

    }

    private IEnumerator Up()
    {
        yield return new WaitForSeconds(waitingTime);
        transform.DOLocalMoveY(endPosY, transitionTime).onComplete = () =>
        {
            up = true;
            down = false;
            moving = false;
            liftTrigger.offset = new Vector2(100, -16);
        };
    }

    public void BackUp()
    {
        moving = true;
        transform.DOLocalMoveY(endPosY, transitionTime).onComplete = () =>
        {
            up = true;
            down = false;
            moving = false;
        };
    }

    public void BackDown()
    {
        moving = true;
        transform.DOLocalMoveY(startPosY, transitionTime).onComplete = () =>
        {
            up = false;
            down = true;
            moving = false;
        };
    }
}
