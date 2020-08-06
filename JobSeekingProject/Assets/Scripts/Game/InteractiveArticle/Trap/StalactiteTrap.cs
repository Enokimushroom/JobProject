using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteTrap : MonoBehaviour
{
    private bool isTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isTrigger&& collision.CompareTag("Player"))
        {
            StartCoroutine(DelayFall());
        }
    }

    IEnumerator DelayFall()
    {
        MusicMgr.Instance.PlaySound("StalactiteRelease", false);
        isTrigger = true;
        PEManager.Instance.GetParticleEffectOneOff("StalactiteDustPE", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        PEManager.Instance.GetParticleEffectOneOff("StalactiteDustFallPE", transform, Vector3.zero, Vector3.one, Quaternion.identity);
        PEManager.Instance.GetParticleEffect("StalactiteDustTrailPE", transform.GetChild(0), Vector3.zero, Vector3.one, Quaternion.identity);
    }
}
