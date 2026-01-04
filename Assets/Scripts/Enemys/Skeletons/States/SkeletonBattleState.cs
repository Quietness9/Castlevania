using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkeletonBattleState : EntityState
{

    Transform _playerTransform;
    Skeleton _skeleton;
    float _moveDir;

    public SkeletonBattleState(Character character,Skeleton skeleton, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        if (_playerTransform == null)
        {
            _playerTransform=GlobalReferencesManager.Instance.GamePlayer.transform;
        }

        if (_playerTransform.GetComponent<PlayerAttribute>().IsDie)
        {
            baseStateMachine.ChangeState(_skeleton.MoveState);
        }

    }

    public override void Update()
    {
        base.Update();

        if (_skeleton.IsPlayerDetected())
        {
            timer=_skeleton.EnemyStateData.BattleTime;
            if (_skeleton.IsPlayerDetected().distance < _skeleton.AttackCheckRadius&&CanAttack())
            {
                baseStateMachine.ChangeState(_skeleton.AttackState);
            }

        }
        else
        {
            if (timer < 0 || Vector2.Distance(_playerTransform.position, _skeleton.transform.position) > _skeleton.EnemyStateData.IgnoreDistance)
            {
                baseStateMachine.ChangeState(_skeleton.IdleState);
            }
        }

        if (_playerTransform.position.x > _skeleton.transform.position.x)
        {
            _moveDir = 1;
        }else if (_playerTransform.transform.position.x < _skeleton.transform.position.x)
        {
            _moveDir = -1;
        }

        if(!_skeleton.IsGroundCheck())
        {
            _moveDir *= -1;
        }

        if (Vector2.Distance(_playerTransform.position, _skeleton.transform.position) > 0.5)
        {
            _skeleton.SetVelocity(_moveDir* _skeleton.EnemyStateData.MoveSpeed, _skeleton.Rb.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    /// <summary>
    /// ÅÐ¶ÏÊÇ·ñ¿ÉÒÔ¹¥»÷
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        if(Time.time > _skeleton.AttackLastTime + _skeleton.AttackCooldown)
        {
            _skeleton.AttackCooldown=Random.Range(_skeleton.EnemyStateData.AttackCooldownOffset.x,
                _skeleton.EnemyStateData.AttackCooldownOffset.y);

            _skeleton.AttackLastTime = Time.time;
            return true;
        }

        return false;
    }
}
