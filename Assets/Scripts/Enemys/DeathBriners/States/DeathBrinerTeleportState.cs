using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerTeleportState : EntityState
{
    DeathBriner _deathBriner;
    public DeathBrinerTeleportState(Character character, DeathBriner deathBriner,StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _deathBriner = deathBriner;
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

        if (triggerFinish)
        {
            
            if (_deathBriner.IsCanSpellCast())
            {
                baseStateMachine.ChangeState(_deathBriner.SpellCastState);
            }
            else
            {
                baseStateMachine.ChangeState(_deathBriner.BattleState);
            } 
        }
    }
}
