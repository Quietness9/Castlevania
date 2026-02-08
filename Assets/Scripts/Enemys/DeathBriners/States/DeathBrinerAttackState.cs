using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerAttackState : EnemyAttackState
{
    DeathBriner _deathBriner;
    float _chanceTeleport;

    public DeathBrinerAttackState(Character character, DeathBriner deathBriner, StateMachine stateMachine, string animationName) : base(character, deathBriner, stateMachine, animationName)
    {
        _deathBriner = deathBriner;
        _chanceTeleport = deathBriner.Data.DefaultChanceTeleport;
    }

    public override void Enter()
    {
        base.Enter();
        _chanceTeleport+= Random.Range(_deathBriner.Data.IncreaseTeleportRatio.x, _deathBriner.Data.IncreaseTeleportRatio.y+1);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void AttackChangeState()
    {
        if (triggerFinish)
        {
            if (_deathBriner.Data.TeleportLimit <= _chanceTeleport)
            {
                _chanceTeleport = _deathBriner.Data.DefaultChanceTeleport;
                baseStateMachine.ChangeState(_deathBriner.TeleportState);
            }
            else
            {
                baseStateMachine.ChangeState(_deathBriner.BattleState);
            }
        }
    }
}
