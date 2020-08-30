using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : TriggerBase
{

    public override void Action()
    {
        //关门。
        transform.GetComponent<Animator>().SetTrigger("Close");
        //播Timeline
    }

}
