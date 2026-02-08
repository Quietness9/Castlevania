using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerSpellCastState : EntityState
{

    DeathBriner _deathBriner; 

    public DeathBrinerSpellCastState(Character character,DeathBriner deathBriner, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _deathBriner=deathBriner;
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
