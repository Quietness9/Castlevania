using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SkeletonIdleState : SkeletonGroundState
{
    

    public SkeletonIdleState(Character character,Skeleton skeleton, StateMachine stateMachine, string animationName) : base(character,skeleton, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        timer =skeleton.IdleTime;
    }

    public override void Update()
    {
        base.Update();

        if (timer < 0.01f&&skeleton.IsGroundCheck())
        {
            baseStateMachine.ChangeState(skeleton.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
