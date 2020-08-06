using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreMainController : MonoBehaviour
{
    [SerializeField] private Animator saveIconAnim;

    public void CheckNextScene()
    {
        ScenesMgr.Instance.LoadSceneAsyn("MainScene", () => 
        {
            StartCoroutine(SetAnim());
        });
    }
    IEnumerator SetAnim()
    {
        saveIconAnim.Play("SavingExit");
        yield return new WaitForSeconds(2.0f);
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1.0f);
        ScenesMgr.Instance.ao.allowSceneActivation = true;
    }
}
