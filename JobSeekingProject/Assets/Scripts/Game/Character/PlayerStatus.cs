using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : BaseManager<PlayerStatus>, IObserver
{
    #region 角色属性
    /// <summary>
    /// 读档或者死亡时的重生地点
    /// </summary>
    public Vector2 respawnPos { get; set; }
    /// <summary>
    /// 当前血量
    /// </summary>
    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value < 0)
                value = 0;
            if (value > MaxHealth)
                value = MaxHealth;
            currentHealth = value;
        }
    }
    /// <summary>
    /// 最大血量
    /// </summary>
    public int MaxHealth { get; set; }
    /// <summary>
    /// 最大法力
    /// </summary>
    public float MaxSp { get; set; }
    /// <summary>
    /// 法力
    /// </summary>
    public float SP { get; set; }
    /// <summary>
    /// 基础攻击力
    /// </summary>
    public float BaseATK { get; set; }
    /// <summary>
    /// 法术攻击系数
    /// </summary>
    public float MagicAtkRate { get; set; }
    /// <summary>
    /// 法术消耗系数
    /// </summary>
    public float MagicCostRate { get; set; }
    /// <summary>
    /// 后坐力系数
    /// </summary>
    public float RecoilForceRate { get; set; }
    /// <summary>
    /// 受击后退距离系数
    /// </summary>
    public float FallBackRate { get; set; }
    /// <summary>
    /// 受击后无敌时间系数
    /// </summary>
    public float HitRecoverRate { get; set; }
    /// <summary>
    /// 攻击间隔系数
    /// </summary>
    public float AttackIntervalRate { get; set; }
    /// <summary>
    /// 攻击距离系数
    /// </summary>
    public float AttackDistanceRate { get; set; }
    /// <summary>
    /// 移动速度
    /// </summary>
    private float speed;
    public float Speed { get { return speed * SpeedRate; } set { speed = value; } }
    /// <summary>
    /// 跳跃高度
    /// </summary>
    public float JumpForce { get; private set; }
    /// <summary>
    /// 冲刺间隔系数
    /// </summary>
    public float SprintCDRate { get; set; }
    /// <summary>
    /// 金钱增量系数
    /// </summary>
    public float MoneyRate { get; set; }
    /// <summary>
    /// 普通攻击系数
    /// </summary>
    public float AtkRate { get; set; }
    /// <summary>
    /// 打击力系数（怪物受击后退力）
    /// </summary>
    public float KnockBackRate { get; set; }
    /// <summary>
    /// 治愈速度系数
    /// </summary>
    public float HealingSpeedRate { get; set; }
    /// <summary>
    /// 治愈量系数
    /// </summary>
    public float HealingAmountRate { get; set; }
    /// <summary>
    /// 回蓝量系数
    /// </summary>
    public float SPIncreaseRate { get; set; }
    /// <summary>
    /// 速度系数
    /// </summary>
    public float SpeedRate { get; set; }
    /// <summary>
    /// 金币拾取范围
    /// </summary>
    public int MoneyPickUpRate { get; set; }
    #endregion

    #region 角色状态
    /// <summary>
    /// 读取基础系数
    /// </summary>
    private bool firstTimeSetAttri = true;
    public bool IsAlive { get; set; }
    public bool IsForzen { get; set; }
    public bool InputEnable { get; set; }
    public bool OnGround { get; set; }
    public bool IsTouchingWall { get; set; }
    public bool IsWallSliding { get; set; }
    public bool IsRunning { get; set; }
    public bool IsHit { get; set; }
    public bool IsDying { get; set; }
    public bool IsFacingRight { get; set; } = true;
    public bool EnableGravity { get; set; } = true;
    public bool CanDoubleJump { get; set; }
    public bool CanShowPosInMap { get; set; }
    public bool CanFlip { get; set; } = true;

    public bool CanBeHurt { get; set; } = true;
    #endregion

    /// <summary>
    /// 数据初始化
    /// </summary>
    public void Init()
    {
        //添加玩家数据根源观察者
        GameDataMgr.Instance.AttachPlayerData(this);
    }

    /// <summary>
    /// 观察者接收函数
    /// </summary>
    /// <param name="sub"></param>
    public void UpdateData(ISubject sub)
    {
        Player temp = sub as Player;
        #region 基础属性
        respawnPos = new Vector2(temp.respawnPosX, temp.respawnPosY);
        MaxHealth = temp.MaxHp;
        CurrentHealth = temp.HP;
        Debug.Log("当前生命值为" + CurrentHealth.ToString());
        if (CurrentHealth <= 0)
        {
            Die();
        }
        MaxSp = temp.MaxSp;
        SP = temp.SP;
        Speed = temp.Speed;
        JumpForce = temp.JumpForce;
        BaseATK = temp.BaseATK;
        #endregion
        
        #region 基础加成数值
        //只有这堆基础加成数值只需读一次（因为不会变)
        if (firstTimeSetAttri)
        {
            firstTimeSetAttri = false;
            MagicAtkRate = temp.MagicAtkBaseRate;
            MagicCostRate = temp.MagicCostBaseRate;
            RecoilForceRate = temp.RecoilForceBaseRate;
            FallBackRate = temp.FallBackBaseRate;
            HitRecoverRate = temp.HitRecoverBaseRate;
            AttackIntervalRate = temp.AttackIntervalBaseRate;
            AttackDistanceRate = temp.AttackDistanceBaseRate;
            SprintCDRate = temp.SprintCDBaseRate;
            AtkRate = temp.AtkBaseRate;
            KnockBackRate = temp.KnockBackBaseRate;
            HealingSpeedRate = temp.HealingSpeedBaseRate;
            HealingAmountRate = temp.HealingAmountBaseRate;
            SPIncreaseRate = temp.SPIncreaseBaseRate;
            SpeedRate = temp.SpeedBaseRate;
            MoneyRate = temp.MoneyBaseRate;
        }
        #endregion

        #region 状态判断
        IsDying = currentHealth > 2 ? false : true;
        #endregion
    }

    /// <summary>
    /// 提供给Buff更改属性的方法（自加自减）
    /// </summary>
    /// <param name="info"> 改哪个属性 </param>
    /// <param name="amount"> 改多少 </param>
    public void ChangeAttri(PlayerInfoType info, float amount)
    {
        switch (info)
        {
            case PlayerInfoType.法术攻击基础系数:
                MagicAtkRate += amount;
                break;
            case PlayerInfoType.法术消耗基础系数:
                MagicCostRate += amount;
                break;
            case PlayerInfoType.后坐力基础系数:
                RecoilForceRate += amount;
                break;
            case PlayerInfoType.受击后退距离基础系数:
                FallBackRate += amount;
                break;
            case PlayerInfoType.受击后无敌时间基础系数:
                HitRecoverRate += amount;
                break;
            case PlayerInfoType.攻击间隔基础系数:
                AttackIntervalRate += amount;
                break;
            case PlayerInfoType.攻击距离基础系数:
                AttackDistanceRate += amount;
                break;
            case PlayerInfoType.冲刺间隔基础系数:
                SprintCDRate += amount;
                break;
            case PlayerInfoType.普通攻击基础系数:
                AtkRate += amount;
                break;
            case PlayerInfoType.打击力基础系数:
                KnockBackRate += amount;
                break;
            case PlayerInfoType.治愈速度基础系数:
                HealingSpeedRate += amount;
                break;
            case PlayerInfoType.治愈量基础系数:
                HealingAmountRate += amount;
                break;
            case PlayerInfoType.回蓝量基础系数:
                SPIncreaseRate += amount;
                break;
            case PlayerInfoType.速度基础系数:
                SpeedRate += amount;
                break;
            case PlayerInfoType.地图显示位置:
                PlayerStatus.Instance.CanShowPosInMap = amount > 0 ? true : false;
                break;
            case PlayerInfoType.金币拾取范围:
                ChangeMoneyPickUpRate(amount);
                break;
            case PlayerInfoType.最大血量:
                GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.最大血量, amount);
                break;
            case PlayerInfoType.基础攻击力:
                BaseATK += amount;
                break;
        }
        Debug.Log(info);
    }

    /// <summary>
    /// 治愈/受伤（技能系统调用或者敌人调用）
    /// </summary>
    public void ChangeCurrentHealth(int amount)
    {
        GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.当前血量, amount);
    }

    /// <summary>
    /// 消耗/回复灵魂力（技能系统调用）
    /// </summary>
    public void ChangeSP(float amount)
    {
        GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.法力, amount);
    }

    /// <summary>
    /// 改变金钱拾取范围
    /// </summary>
    private void ChangeMoneyPickUpRate(float amount)
    {
        CapsuleCollider2D cc = GameManager.Instance.playerGO.transform.Find("GeoCollecter").GetComponent<CapsuleCollider2D>();
        if (amount > 0)
        {
            cc.direction = CapsuleDirection2D.Horizontal;
            cc.offset = new Vector2(0, 1);
            cc.size = new Vector2(20, 10);
            Debug.Log("金钱拾取范围变大了");
        }
        else
        {
            cc.direction = CapsuleDirection2D.Vertical;
            cc.offset = new Vector2(0, -0.2f);
            cc.size = new Vector2(1, 1.5f);
            Debug.Log("金钱拾取范围恢复正常了");
        }
    }

    /// <summary>
    /// 获得/消费金钱（给商店系统和拾取时金币调用）
    /// </summary>
    public void ChangeMoney(MoneyDetails md)
    {
        float total = md.moneyAmount;
        if(md.moneySource == MoneyDetails.Source.PickUp)
        {
            total = md.moneyAmount * MoneyRate;
        }
        GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.金钱, total);
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    private void Die()
    {
        //生成特效

        //销毁GO

        //修改存活状态
        IsAlive = false;
        InputEnable = false;
    }

    /// <summary>
    /// 提供给武器升级系统的方法
    /// </summary>
    /// <param name="amount"></param>
    public void UpgradeSword(int amount)
    {
        GameDataMgr.Instance.ChangePlayerAttri(PlayerInfoType.基础攻击力, amount);
    }

    /// <summary>
    /// 更新复活坐标
    /// </summary>
    public void UpdateRespawnPos(Vector2 pos)
    {
        respawnPos = pos;
        GameDataMgr.Instance.UpdateRespawnPos(pos);
    }

}
