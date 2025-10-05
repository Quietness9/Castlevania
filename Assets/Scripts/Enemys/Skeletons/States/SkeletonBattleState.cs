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


    }

    public override void Update()
    {
        base.Update();

        

        if (_skeleton.IsPlayerDetected())
        {
            timer=_skeleton.BattleTime;
            if (_skeleton.IsPlayerDetected().distance < _skeleton.AttackCheckRadius&&CanAttack())
            {
                baseStateMachine.ChangeState(_skeleton.AttackState);
            }

        }
        else
        {
            if (timer < 0 || Vector2.Distance(_playerTransform.position, _skeleton.transform.position) > 5)
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

        if (Vector2.Distance(_playerTransform.position, _skeleton.transform.position) > 0.8)
        {
            _skeleton.SetVelocity(_moveDir* _skeleton.MoveSpeed, _skeleton.Rb.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time > _skeleton.AttackLastTime + _skeleton.AttackCooldown)
        {
            _skeleton.AttackCooldown=Random.Range(_skeleton.AttackCooldownOffset.x,_skeleton.AttackCooldownOffset.y);
            _skeleton.AttackLastTime = Time.time;
            return true;
        }

        return false;
    }
}
