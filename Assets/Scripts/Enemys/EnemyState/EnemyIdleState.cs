using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyGroundState
{
    public EnemyIdleState(Character character, Enemy enemy, StateMachine stateMachine, string animationName) : base(character, enemy, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.EnemyStateData.IdleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (timer < 0.01f && enemy.IsGroundCheck())
        {
            baseStateMachine.ChangeState(enemy.MoveState);
        }
    }
}
