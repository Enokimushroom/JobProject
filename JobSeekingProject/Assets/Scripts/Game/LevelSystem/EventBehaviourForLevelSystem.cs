using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "EBForLevelSystem", menuName = "LevelMgr/LevelEB")]
public class EventBehaviourForLevelSystem : ScriptableObject
{
    private Transform rewardPos;
    private GameObject levelTrigger;

    public void GetMoneyReward()
    {
        MonoMgr.Instance.StartCoroutine(GetMoney());
    }

    public void GetItemReward()
    {
        MonoMgr.Instance.StartCoroutine(GetItem());
    }

    public void GetSkillReward(int id)
    {
        MonoMgr.Instance.StartCoroutine(GetSkill(id));
    }

    private IEnumerator GetMoney()
    {
        PlayerStatus.Instance.InputEnable = false;
        CheerAndShake();
        int randomCount = Random.Range(80, 120);
        WaitForSeconds time = new WaitForSeconds(0.1f);
        for (int i = 0; i < randomCount; ++i)
        {
            PoolMgr.Instance.GetObj("Geo", (geo) => {
                geo.GetComponent<Geo>().SetMoneyPerUnit(5);
                int index = Random.Range(0, 3);
                geo.transform.position = rewardPos.GetChild(index).position;
                Vector2 force = AddForceCalculate.CalculateFroce(geo.transform.position, new Vector3(Random.Range(-7.0f, 6.0f), -3.5f, 0), Random.Range(1.0f, 2.0f));
                geo.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                geo.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            });
            yield return time;
        }
        yield return new WaitForSeconds(2.0f);
        PlayerStatus.Instance.InputEnable = true;
        LevelTriggerEnd();
    }

    private IEnumerator GetItem()
    {
        PlayerStatus.Instance.InputEnable = false;
        CheerAndShake();
        WaitForSeconds delay = new WaitForSeconds(1.0f);
        yield return delay;
        for (int i = 0; i < 3; ++i)
        {
            int temp = i;
            ResMgr.Instance.LoadAsync<GameObject>("ChestItem", (o) =>
            {
                o.transform.position = rewardPos.position;
                Vector2 force = AddForceCalculate.CalculateFroce(o.transform.position, new Vector3(-3 + temp * 3, -3.5f, 0), 2.0f);
                o.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
                o.GetComponent<ChestItem>().SetItemID(16 + temp, 1);
            });
            yield return delay;
        }
        yield return delay;
        PlayerStatus.Instance.InputEnable = true;
        LevelTriggerEnd();
    }

    private IEnumerator GetSkill(int id)
    {
        PlayerStatus.Instance.InputEnable = false;
        CheerAndShake();
        GameObject spell = ResMgr.Instance.Load<GameObject>("PickUpSpell");
        spell.transform.position = new Vector3(rewardPos.position.x, rewardPos.position.y + 3, 0);
        spell.GetComponent<PickUpSpellTrigger>().SetItemID(id);
        yield return new WaitForSeconds(2.0f);
        spell.transform.DOMoveY(-1, 3.0f).onComplete = () => {
            PlayerStatus.Instance.InputEnable = true; 
            LevelTriggerEnd();
        };
    }

    private void CheerAndShake()
    {
        MusicMgr.Instance.PlaySound("CrowdCheerAudio", false);
        CinemachineShake.Instance.ShakeCamera(1.5f, 2.0f);
        rewardPos = GameObject.Find("RewardPos").transform;
    }

    private void LevelTriggerEnd()
    {
        levelTrigger = GameObject.Find("LevelTrigger");
        levelTrigger.GetComponent<LevelTrigger>().End();
    }
}
