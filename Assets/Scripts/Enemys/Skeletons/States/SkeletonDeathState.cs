using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EntityState
{
    Skeleton _skeleton;
    public SkeletonDeathState(Character character, Skeleton skeleton,StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        _skeleton.Bd2d.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
