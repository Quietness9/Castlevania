using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EntityState
{
    Skeleton _skeleton;

    public SkeletonAttackState(Character character,Skeleton skeleton, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();
        if (triggerFinish)
        {
            baseStateMachine.ChangeState(_skeleton.BattleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
