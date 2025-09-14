using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerMove : PlayerState
{
    PlayerMoveData _moveData;

    public PlayerMove(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _moveData=player.MoveData;
    }

    public override void Update()
    {
        base.Update();
        Move();

        if (player.Hor == 0)
        {
            FrictionPlayer();
            baseStateMachine.ChangeState(player.IdleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    
    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {   

        float targetSpeed = player.Hor * _moveData.MaxMoveSpeed;

        float accelRate;

        if (player.LastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _moveData.AccelForce : _moveData.DecreForce;
        }

        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _moveData.AccelForce * _moveData.AccelInAir 
                : _moveData.DecreForce * _moveData.DecreInAir;
        }

        //在跳跃的顶点增加加速度和最大速度，使跳跃感觉更有弹性，反应灵敏和自然
        if ((player.IsJumping||player.IsJumpFalling) && Mathf.Abs(rb.velocity.y) < _moveData.JumpHangTimeThreshold)
        {
            accelRate*=_moveData.JumpHangAccelerationMult;
            targetSpeed*=_moveData.JumpHangMaxSpeedMult;
        }

        if(_moveData.doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && 
            Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f &&player.LastOnGroundTime < 0)
        {
            accelRate = 0;
        }

        float speedDif = targetSpeed - rb.velocity.x;
        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    /// <summary>
    /// 增加玩家摩擦力
    /// </summary>
    private void FrictionPlayer()
    {
        if (player.LastOnGroundTime > 0 && Mathf.Abs(player.Hor) < 0.01f)
        {
            float force = _moveData.FrictionForce;
            force *= Mathf.Sign(rb.velocity.x);

            player.Rb.AddForce(Vector2.right * -force, ForceMode2D.Impulse);

        }
    }
}
