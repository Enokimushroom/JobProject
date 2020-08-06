using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Event", menuName = "Task/Event")]
public class EventBehaviourForLevelSystem : ScriptableObject
{
    public void GeneratorAnim()
    {
        ResMgr.Instance.LoadAsync<GameObject>("fly", (obj) => {
            float temp = 0;
            DOTween.To(() => temp, x => temp = x, 1, 2.0f).OnStepComplete(() =>
            {
                EventCenter.Instance.EventTrigger("CreateGenerator");
            });
        });

    }
}
