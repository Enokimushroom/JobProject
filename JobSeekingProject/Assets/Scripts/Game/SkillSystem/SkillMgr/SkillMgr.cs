using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class SkillMgr : BaseManager<SkillMgr>
{
    public Dictionary<string, SkillData> FixSkill { get; set; } = new Dictionary<string, SkillData>();
    public Dictionary<string, SkillData> badgeSkill { get; set; } = new Dictionary<string, SkillData>();
    public Dictionary<string, GameObject> excutingSkill { get; set; } = new Dictionary<string, GameObject>();
    /// <summary>
    /// 加载技能时使用的暂时按键
    /// </summary>
    private CustomButton tempCB;
    /// <summary>
    /// 释放技能后记录此技能以判断连击
    /// </summary>
    private SkillData lastSkill;
    /// <summary>
    /// 释放技能后记录技能按键以判断连击
    /// </summary>
    private CustomButton lastCB;

    /// <summary>
    /// 初始化固定技能（护符技能由护符管理器调用此类中的方法）
    /// </summary>
    public void Init()
    {
        //基础技能，平A，回血，灵魂攻击等
        List<ItemInfo> baseSkill = GameDataMgr.Instance.playerInfo.fixItem;
        for (int i = 0; i < baseSkill.Count; ++i)
        {
            Item tempBase = GameDataMgr.Instance.GetItemInfo(baseSkill[i].id);
            if (tempBase.skillID != string.Empty)
                AddSkill(tempBase.skillID);
        }

        //附加技能，冲刺，超级冲刺，二段跳等
        List<ItemInfo> extraSkill = GameDataMgr.Instance.playerInfo.skillItem;
        for (int i = 0; i < extraSkill.Count; ++i)
        {
            Item tempExtra = GameDataMgr.Instance.GetItemInfo(extraSkill[i].id);
            if (tempExtra.skillID != string.Empty)
                AddSkill(tempExtra.skillID);
        }
    }

    /// <summary>
    /// 重新读取最新的护符技能（供护符管理器调用）
    /// </summary>
    public void GetLastestSkill(List<string> list)
    {
        badgeSkill.Clear();
        for(int i = 0; i < list.Count; ++i)
        {
            AddSkill(list[i]);
        }
        foreach(SkillData sk in badgeSkill.Values)
        {
            if (!excutingSkill.ContainsKey(sk.skillID) && sk.changeAtr && sk.key == ReflectKey.none)
            {
                GeneratePassiveSkill(sk);
            }
        }
    }

    /// <summary>
    /// 按类型添加技能到相应的技能字典
    /// </summary>
    public void AddSkill(string id)
    {
        if (id == string.Empty) return;
        SkillData tempSkill = ResMgr.Instance.Load<SkillData>(id);
        if (tempSkill == null) return;
        if (tempSkill.skillTag == SkillTag.FixSkill)
        {
            if (tempSkill.key != ReflectKey.none)
            {
                switch (tempSkill.key)
                {
                    case ReflectKey.attack:
                        tempCB = KeyCodeMgr.Instance.Attack;
                        break;
                    case ReflectKey.jump:
                        tempCB = KeyCodeMgr.Instance.Jump;
                        break;
                    case ReflectKey.sprint:
                        tempCB = KeyCodeMgr.Instance.Sprint;
                        break;
                    case ReflectKey.superSprint:
                        tempCB = KeyCodeMgr.Instance.SuperSprint;
                        break;
                    case ReflectKey.recover:
                        tempCB = KeyCodeMgr.Instance.Recover;
                        break;
                }
                if (tempCB.SkillID == null) tempCB.AttachSkill(id);
            }
            if (!FixSkill.ContainsKey(id)) FixSkill.Add(id, tempSkill);
            tempSkill.owner = GameManager.Instance.playerGO;
            //处理此技能所附带的连击技能
            while (tempSkill.isBatter)
            {
                SkillData batter = ResMgr.Instance.Load<SkillData>(tempSkill.nextBatterID);
                if (!FixSkill.ContainsKey(batter.skillID)) FixSkill.Add(batter.skillID, batter);
                batter.owner = GameManager.Instance.playerGO;
                batter.coolDownTime = -batter.coolTime;
                if (batter.nextBatterID == tempSkill.skillID) break;
                tempSkill = batter;
            }
            //只有平砍是需要单独处理组合按键，其他有组合按键的技能都由物品带来，无需额外处理
            if (tempSkill.hasComboKey && tempSkill.key == ReflectKey.attack)
            {
                SkillData up = ResMgr.Instance.Load<SkillData>(tempSkill.upKeySkillID);
                if (up != null && !FixSkill.ContainsKey(up.skillID)) FixSkill.Add(up.skillID, up);
                up.owner = GameManager.Instance.playerGO;
                up.coolDownTime = -up.coolTime;
                SkillData down = ResMgr.Instance.Load<SkillData>(tempSkill.downKeySkillID);
                if (down != null && !FixSkill.ContainsKey(down.skillID)) FixSkill.Add(down.skillID, down);
                down.owner = GameManager.Instance.playerGO;
                down.coolDownTime = -down.coolTime;
            }
            if (tempSkill.canBeChangeByBadge)
            {
                SkillData that = ResMgr.Instance.Load<SkillData>(tempSkill.skillIDIfChanged);
                that.owner = GameManager.Instance.playerGO;
            }
        }
        else
        {
            if (!badgeSkill.ContainsKey(id)) badgeSkill.Add(id, tempSkill);
            tempSkill.owner = GameManager.Instance.playerGO;
        }
        if (tempSkill.hasCoolTime)
            tempSkill.coolDownTime = -tempSkill.coolTime;
    }

    /// <summary>
    /// 在执行字典中抹去相应的技能（技能释放器销毁时调用）
    /// </summary>
    public void RemoveExcutingSkill(string id)
    {
        if (excutingSkill.ContainsKey(id))
            excutingSkill.Remove(id);
    }

    /// <summary>
    /// 检测是否连击释放
    /// </summary>
    public SkillData CheckBatter(SkillData thisSkill,CustomButton cb)
    {
        if (lastSkill == null || !lastSkill.isBatter) return thisSkill;
        else
        {
            if (lastSkill.coolDownTime <= Time.time && lastSkill.loseComboTime >= Time.time && lastCB == cb && PlayerStatus.Instance.OnGround)
            {
                string skillID = lastSkill.nextBatterID;
                SkillData newSkill = ResMgr.Instance.Load<SkillData>(skillID);
                lastCB = cb;
                if (newSkill != null) return newSkill;
            }
            lastCB = cb;
            return thisSkill;
        }
    }

    /// <summary>
    /// 遍历检查释放条件
    /// </summary>
    public bool CheckSkillCondition(SkillData data)
    {
        if (data.releaseCondition.Length == 0) return true;
        IReleaseCondition[] conditions = DeployerConfigFactory.CreateReleaseCondition(data);
        foreach(IReleaseCondition condition in conditions)
        {
            if (!condition.CheckCondition(data))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 生成技能
    /// </summary>
    public GameObject GenerateSkill(SkillData data)
    {
        //创建技能预制件
        GameObject skillGo = ResMgr.Instance.Load<GameObject>(data.prefabName);

        //调整位置和方向
        int temp = PlayerStatus.Instance.IsFacingRight ? 1 : -1;
        if (data.key == ReflectKey.attack && data.offsetX != 0)
        {
            skillGo.transform.position = data.owner.transform.position + new Vector3(temp * data.offsetX, 0, 0) * PlayerStatus.Instance.AttackDistanceRate;
        }
        else
        {
            skillGo.transform.position = data.owner.transform.position;
        }
        if (PlayerStatus.Instance.IsWallSliding)
            temp *= -1;
        skillGo.transform.localScale = new Vector3(temp * skillGo.transform.localScale.x,skillGo.transform.localScale.y, skillGo.transform.localScale.x);

        //传递技能数据
        Deployer deployer = skillGo.GetComponent<Deployer>();
        deployer.SkillData = data;//内部创建算法对象
        deployer.DeploySkill();//内部执行算法对象
        deployer.SetCoolDown();
        deployer.SetComboTime();
        lastSkill = data;

        return skillGo;
    }

    public void GeneratePassiveSkill(SkillData data)
    {
        GameObject skillGo = ResMgr.Instance.Load<GameObject>(data.prefabName);
        if (!excutingSkill.ContainsKey(data.skillID))
            excutingSkill.Add(data.skillID, skillGo);
        Deployer deployer = skillGo.GetComponent<Deployer>();
        deployer.transform.position = data.owner.transform.position;
        deployer.SkillData = data;
        deployer.DeploySkill();
        deployer.SetCoolDown();
    }

    public void Reset()
    {
        excutingSkill.Clear();
        FixSkill.Clear();
    }
}
