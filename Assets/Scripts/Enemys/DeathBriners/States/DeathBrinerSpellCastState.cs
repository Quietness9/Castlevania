using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerSpellCastState : EntityState
{

    DeathBriner _deathBriner; 
    float _spellCastTimer;
    int _attackCount;

    public DeathBrinerSpellCastState(Character character,DeathBriner deathBriner, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _deathBriner=deathBriner;
    }

    public override void Enter()
    {
        base.Enter();
        _spellCastTimer = _deathBriner.Data.SpellAttackCooldown;
        _attackCount = _deathBriner.Data.SpellAttackCount;
    }

    public override void Exit()
    {
        base.Exit();
        _deathBriner.LastTimeCast=Time.time;
    }

    public override void Update()
    {
        base.Update();

        _spellCastTimer -=Time.deltaTime;

        if (_attackCount <= 0)
        {
            baseStateMachine.ChangeState(_deathBriner.TeleportState);
        }

        if (IsCanAttackSpellCast())
        {
            _deathBriner.CreateSpellCast();
        }
        
    }

    /// <summary>
    /// ≈–∂œπÌ ÷ «∑Òƒ‹πªπ•ª˜
    /// </summary>
    /// <returns></returns>
    private bool IsCanAttackSpellCast()
    {
        if (_spellCastTimer <= 0 && _attackCount > 0)
        {
            _spellCastTimer = _deathBriner.Data.SpellAttackCooldown;
            _attackCount--;
            return true;
        }

        return false;
    }
}
