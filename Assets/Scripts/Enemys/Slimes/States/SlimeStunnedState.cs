using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EntityState
{
    Slime _slime;
    Player _player;


    public SlimeStunnedState(Character character, Slime slime, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _slime = slime;
    }

    public override void Enter()
    {
        base.Enter();

        if (_player == null)
        {
            _player = GlobalReferencesMgr.Instance.GamePlayer;
        }

        _slime.Fx.InvokeRepeating("RedColorBlink", 0, _slime.Fx.FxData.RepeatTime);
        timer = _player.StunnedDuration;
        _slime.Rb.AddForce(new Vector2(_player.StunnedForce.x * _player.Direction * _slime.EnemyStateData.StunnedMul,
            _player.StunnedForce.y * _slime.EnemyStateData.StunnedMul), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        base.Exit();
        _slime.Fx.CancelColorChange();
    }

    public override void Update()
    {
        base.Update();
        if (timer < 0.01f)
        {
            baseStateMachine.ChangeState(_slime.BattleState);
        }
    }
}
