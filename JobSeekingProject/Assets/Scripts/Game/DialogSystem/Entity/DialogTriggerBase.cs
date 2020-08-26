using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerBase : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 size;
    private int index;
    private bool hintOn;
    private BoxCollider2D col;
    private CharacterMovement cm;
    protected TaskGiver tg;
    [SerializeField] protected DialogBase[] db;
    [SerializeField] protected GameObject hintObj;
    [SerializeField] protected bool nextDialogOnInteract;

    public virtual void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.offset = offset;
        col.size = size;
        index = nextDialogOnInteract ? -1 : 0;
        hintObj = transform.GetChild(0).gameObject;
        hintObj.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(offset.x * transform.localScale.x, offset.y * transform.localScale.y, 0), size);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnterAction(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExitAction();
        }
    }

    public virtual void EnterAction(Collider2D collision)
    {
        if (!hintOn)
        {
            hintOn = true;
            hintObj.SetActive(true);
            cm = collision.GetComponent<CharacterMovement>();
        }
        if (Input.GetKeyDown(KeyCodeMgr.Instance.Interact.CurrentKey) && !PlayerStatus.Instance.IsForzen && PlayerStatus.Instance.OnGround)
        {
            //调整面向
            if ((transform.position.x - cm.transform.position.x > 0 && !PlayerStatus.Instance.IsFacingRight) || (transform.position.x - cm.transform.position.x < 0 && PlayerStatus.Instance.IsFacingRight)) cm.Flip();
            CheckDialog();
        }
    }

    public virtual void CheckDialog()
    {
        if (nextDialogOnInteract && !DialogMgr.Instance.inDialog)
        {
            if (index < db.Length - 1)
            {
                index++;
            }
        }
        DialogMgr.Instance.EnqueueDialog(db[index], tg);
    }

    public virtual void ExitAction()
    {
        hintObj.SetActive(false);
        hintOn = false;
    }

}
