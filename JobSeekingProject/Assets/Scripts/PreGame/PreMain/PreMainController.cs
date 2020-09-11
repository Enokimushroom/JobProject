using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class PreMainController : MonoBehaviour
{
    private string configMapTxt;

    public void CheckNextScene()
    {
        ScenesMgr.Instance.LoadScene("MainScene", ()=> {
            InputMgr.Instance.StartOrEndCheck(true);
            KeyCodeMgr.Instance.Init();
        });
    }

    private void Awake()
    {
        ResMgr.Instance.Init();
    }

}
