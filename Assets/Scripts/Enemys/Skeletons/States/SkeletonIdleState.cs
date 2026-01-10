using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SkeletonIdleState : EnemyIdleState
{

    public SkeletonIdleState(Character character,Skeleton skeleton, StateMachine stateMachine, string animationName) : base(character,skeleton, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
