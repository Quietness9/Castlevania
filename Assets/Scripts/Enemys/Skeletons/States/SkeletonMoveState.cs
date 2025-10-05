using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{

    public SkeletonMoveState(Character character,Skeleton skeleton, StateMachine stateMachine, string animationName) : base(character, skeleton, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        timer=skeleton.MoveTime;
    }

    public override void Update()
    {
        base.Update();
        if (timer < 0.01f&&skeleton.IsGroundCheck())
        {
            skeleton.TurnDirection();
            baseStateMachine.ChangeState(skeleton.IdleState);
        }

        if (skeleton.IsWallCheck()||!skeleton.IsGroundCheck())
        {
            skeleton.TurnDirection();
        }

        skeleton.SetVelocity(skeleton.Direction*skeleton.MoveSpeed,skeleton.Rb.velocity.y);
    }

    public override void Exit() 
    {
        base.Exit(); 
    }
}
