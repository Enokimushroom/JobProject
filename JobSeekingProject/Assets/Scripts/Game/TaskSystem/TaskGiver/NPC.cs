using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string ID;
    public string _ID
    {
        get
        {
            return ID;
        }
    }

    [SerializeField]
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
    }
}
