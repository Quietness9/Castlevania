using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EntityState
{
    protected Enemy enemy;

    public EnemyAttackState(Character character,Enemy enemy, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        this.enemy= enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        AttackChangeState();
    }

    /// <summary>
    /// 从攻击状态转为其他状态
    /// </summary>
    protected virtual void AttackChangeState()
    {
        if (triggerFinish)
        {
            baseStateMachine.ChangeState(enemy.BattleState);
        }
    }
}
