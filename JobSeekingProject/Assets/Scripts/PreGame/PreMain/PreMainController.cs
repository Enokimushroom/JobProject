using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreMainController : MonoBehaviour
{
    public void CheckNextScene()
    {
        ScenesMgr.Instance.LoadScene("MainScene", null);
    }
}
