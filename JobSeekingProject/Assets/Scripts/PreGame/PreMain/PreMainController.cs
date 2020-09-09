using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreMainController : MonoBehaviour
{
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
