using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogReference : MonoBehaviour
{
    public static DialogReference instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject testGO;
    //scene GO can be load by using this script
}
