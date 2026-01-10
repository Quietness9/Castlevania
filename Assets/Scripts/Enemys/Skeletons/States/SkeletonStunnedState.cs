using UnityEngine;

public class SkeletonStunnedState : EntityState
{
    Skeleton _skeleton;
    Player _player;
    public SkeletonStunnedState(Character character, Skeleton skeleton, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        if (_player == null)
        {
            _player = GlobalReferencesManager.Instance.GamePlayer;
        }

        _skeleton.Fx.InvokeRepeating("RedColorBlink", 0, _skeleton.Fx.FxData.RepeatTime);
        timer = _player.StunnedDuration;
        _skeleton.Rb.AddForce(new Vector2(_player.StunnedForce.x * _player.Direction * _skeleton.EnemyStateData.StunnedMul,
            _player.StunnedForce.y * _skeleton.EnemyStateData.StunnedMul), ForceMode2D.Impulse);

    }

    public override void Update()
    {
        base.Update();
        if (timer < 0.01f)
        {
            baseStateMachine.ChangeState(_skeleton.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _skeleton.Fx.CancelColorChange();
    }
}
