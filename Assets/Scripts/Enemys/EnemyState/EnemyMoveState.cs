using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyGroundState
{
    public EnemyMoveState(Character character, Enemy enemy, StateMachine stateMachine, string animationName) : base(character, enemy, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.EnemyStateData.MoveTime;
        
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
            enemy.TurnDirection();
            baseStateMachine.ChangeState(enemy.IdleState);
        }

        if (enemy.IsWallCheck() || !enemy.IsGroundCheck())
        {
            enemy.TurnDirection();
        }

        enemy.SetVelocity(enemy.Direction * enemy.EnemyStateData.MoveSpeed, enemy.Rb.velocity.y);
    }
}
