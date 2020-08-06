using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SkillInputController : MonoBehaviour
{
    private CharacterSkillSystem skillSystem;
    private bool comboUp;
    private bool comboDown;
    private SkillData currentSkill;
    private CustomButton currentCB;
    private bool currentCBPressing;

    private void Awake()
    {
        skillSystem = GetComponent<CharacterSkillSystem>();
    }

    private void Start()
    {
        EventCenter.Instance.AddEventListener<KeyCode>("xPress", CheckKeyDown);
        EventCenter.Instance.AddEventListener<KeyCode>("xPressing", CheckKeyPress);
        EventCenter.Instance.AddEventListener<KeyCode>("xUp", CheckKeyUp);
    }

    private void CheckKeyDown(KeyCode key)
    {
        //如果玩家处于冻结状态,return 
        if (PlayerStatus.Instance.IsForzen) return;
        if (!PlayerStatus.Instance.InputEnable) return;

        CheckKeyCode(key, KeyCodeMgr.Instance.Attack);
        CheckKeyCode(key, KeyCodeMgr.Instance.Jump);
        CheckKeyCode(key, KeyCodeMgr.Instance.Sprint);
        CheckKeyCode(key, KeyCodeMgr.Instance.Recover);
        CheckKeyCode(key, KeyCodeMgr.Instance.SuperSprint);
    }
    
    private void CheckKeyCode(KeyCode key, CustomButton cb)
    {
        if (key == cb.CurrentKey)
        {
            if (cb.SkillID != null)
            {
                currentCB = cb;
                currentSkill = ResMgr.Instance.Load<SkillData>(cb.SkillID);
                StartCoroutine(CheckDifferentByPressingTime(cb));
            }
            else
            {
                Debug.Log("按键无绑定此技能");
            }
        }
    }

    #region 监听组合按键触发和按键时长
    private void CheckKeyPress(KeyCode key)
    {
        if (key == KeyCodeMgr.Instance.Up.CurrentKey)
            comboUp = true;
        if (key == KeyCodeMgr.Instance.Down.CurrentKey)
            comboDown = true;
        if (currentCB == null) return;
        if (key == currentCB.CurrentKey)
            currentCBPressing = true;

    }
    private void CheckKeyUp(KeyCode key)
    {
        if (key == KeyCodeMgr.Instance.Up.CurrentKey)
            comboUp = false;
        if (key == KeyCodeMgr.Instance.Down.CurrentKey)
            comboDown = false;
        if (currentCB == null) return;
        if (key == currentCB.CurrentKey)
            currentCBPressing = false;
    }
    #endregion

    /// <summary>
    /// 按键时长检测
    /// </summary>
    IEnumerator  CheckDifferentByPressingTime(CustomButton cb)
    {
        if (currentSkill.differentByPressingTime)
        {
            float pressTime = 0;
            currentCBPressing = true;
            while (currentCBPressing)
            {
                pressTime += 0.05f;
                yield return new WaitForSeconds(0.05f);
                if (pressTime > 0.2f) break;
            }
            if (pressTime <= 0.2f)
            {
                currentSkill = ResMgr.Instance.Load<SkillData>(currentSkill.shortPressingSkillID);
            }
        }
        StartCoroutine(CheckComboKey(cb));
    }

    /// <summary>
    /// 组合按键检测
    /// </summary>
    IEnumerator CheckComboKey(CustomButton cb)
    {
        if (currentSkill.hasComboKey)
        {
            float checkTime = 0.2f;
            while (checkTime > 0f)
            {
                checkTime -= 0.1f;
                if (comboUp || comboDown) break;
                yield return new WaitForSeconds(0.1f);
            }
            if (comboUp)
            {
                comboUp = false;
                currentSkill = ResMgr.Instance.Load<SkillData>(currentSkill.upKeySkillID);
            }
            else if (comboDown)
            {
                comboDown = false;
                currentSkill = ResMgr.Instance.Load<SkillData>(currentSkill.downKeySkillID);
            }
        }
        CheckIfChangedByBadge();
        skillSystem.AttackUstSkill(currentSkill, cb);
    }

    /// <summary>
    /// 检查技能是否被护符技能所覆盖
    /// </summary>
    private void CheckIfChangedByBadge()
    {
        if (currentSkill.canBeChangeByBadge && SkillMgr.Instance.badgeSkill.ContainsKey(currentSkill.skillIDIfChanged) && currentSkill.skillIDIfChanged != null)
        {
            currentSkill = ResMgr.Instance.Load<SkillData>(currentSkill.skillIDIfChanged);
            Debug.Log("原技能被护符技能覆盖");
        }
    }
    
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPress", CheckKeyDown);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xPressing", CheckKeyPress);
        EventCenter.Instance.RemoveEventListener<KeyCode>("xUp", CheckKeyUp);
    }

}
