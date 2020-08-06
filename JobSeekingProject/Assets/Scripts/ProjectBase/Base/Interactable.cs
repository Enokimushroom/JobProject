using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float interactRange;
    public Vector3 interactOffset;
    private void Update()
    {
        if (Vector2.Distance(gameObject.transform.position + interactOffset, GameManager.Instance.playerGO.transform.position) < interactRange)
        {
            Debug.Log("1");
            Interact();
        }
        else
        {
            Debug.Log("2");
            TooFar();
        }
    }

    public abstract void Interact();

    public abstract void TooFar();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + interactOffset, interactRange);
    }
}
