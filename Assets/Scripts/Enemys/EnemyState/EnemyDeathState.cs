using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EntityState
{
    protected Enemy enemy;

    public EnemyDeathState(Character character,Enemy enemy, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
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
    }
}
