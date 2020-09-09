using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

[RequireComponent(typeof(PlayableDirector))]
public class PickUpSpellTrigger : TriggerBase
{
    private int itemID;
    private AudioSource idleLoopAudio;
    private PlayableDirector playableDirector;
    private readonly Dictionary<string, PlayableBinding> bindingDict = new Dictionary<string, PlayableBinding>();
    private bool repeatable = false;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        foreach(var bind in playableDirector.playableAsset.outputs)
        {
            if (!bindingDict.ContainsKey(bind.streamName))
                bindingDict.Add(bind.streamName, bind);
        }

        MusicMgr.Instance.PlaySound("PickUpSpellIdleAudio", true,(o)=> { idleLoopAudio = o; });
    }

    public override void Action()
    {
        if (enabled)
        {
            MusicMgr.Instance.StopSound(idleLoopAudio);
            PlayerStatus.Instance.EnableGravity = false;
            transform.GetChild(1).gameObject.SetActive(false);
            SetTrackDynamic("Animation Track", collision);
            SetTrackDynamic("Signal Track", collision);
            collision.transform.DOMove(transform.position, 0.5f);
            playableDirector.Play();
            if (itemID != 0) 
                StartCoroutine(GetItemInfo(itemID));
            if (!repeatable)
            {
                enabled = false;
            }
        }
    }

    private void SetTrackDynamic(string trackName,GameObject go)
    {
        if(bindingDict.TryGetValue(trackName,out PlayableBinding pb))
        {
            playableDirector.SetGenericBinding(pb.sourceObject, go);
        }
    }

    private IEnumerator GetItemInfo(int itemID)
    {
        yield return new WaitForSeconds((float)playableDirector.duration + 0.1f);
        ItemInfo item = new ItemInfo() { id = itemID, num = 1 };
        GameDataMgr.Instance.GetItem(item);
        PlayerStatus.Instance.EnableGravity = true;
        Destroy(gameObject);
    }

    public void SetItemID(int id)
    {
        itemID = id;
    }
}
