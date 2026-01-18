using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerMoveState : EnemyMoveState
{
    public DeathBrinerMoveState(Character character, DeathBriner deathBriner, StateMachine stateMachine, string animationName) : base(character, deathBriner, stateMachine, animationName)
    {
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
    }
}
