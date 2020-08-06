using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttack stateData;
    protected Projectile projectileScript;

    public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition,D_RangeAttack stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        PoolMgr.Instance.GetObj(stateData.projectileName, (o) =>
        {
            o.transform.position = attackPosition.position;
            o.transform.rotation = attackPosition.rotation;
            projectileScript = o.GetComponent<Projectile>();
            projectileScript.SetProjectile(stateData.projectileName, stateData.projectileSpeed, stateData.projectileHeight, stateData.projectileDamage);
            Vector2 force = AddForceCalculate.CalculateFroce(attackPosition.transform, GameObject.FindWithTag("Player").transform, stateData.projectileHeight);
            o.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        });
    }


}
