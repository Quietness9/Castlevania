using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    PlayerMoveData _moveData;

    public PlayerJumpState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        player.PlayerInput.JumpDownEvent += JumpDownHandle;
        player.PlayerInput.JumpUpEvent += JumpUpHandle;
    }

    public override void Enter()
    {
        base.Enter();
        _moveData=player.MoveData;
    }

    public override void Update()
    {
        base.Update();
        player.Animator_CT.SetFloat("yVelocity", rb.velocity.y);

        JumpController();
        SelectGravity();

        //if (player.IsGroundCheck())
        //{
        //    baseStateMachine.ChangeState(player.IdleState);
        //}
    }

  
    public override void Exit()
    {
        base.Exit();
    }

    private void Jump()
    {
        //确保不能多次跳跃
        player.LastPressedJumpTime = 0;
        player.LastOnGroundTime = 0;

        //当我们下落时，我们加大所施加的力
        //这意味着我们总是觉得我们跳了相同的量
        //（事先将玩家的Y轴速度设置为0可能会产生相同的效果，但我发现这更优雅：D）
        float force = _moveData.JumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 跳跃控制
    /// </summary>
    private void JumpController()
    {
        if (player.IsJumping && rb.velocity.y < 0)
        {
            player.IsJumping = false;
            player.IsJumpFalling = true;
        }

        if (player.LastOnGroundTime > 0 && !player.IsJumping)
        {
            player.IsJumpCut = false;
            player.IsJumpFalling = false;
        }

        if (CanJump() && player.LastPressedJumpTime > 0)
        {
            player.IsJumping = true;
            player.IsJumpCut = false;
            player.IsJumpFalling = false;
            Jump();
        }
    }

    /// <summary>
    /// 根据不同状态，选择在不同重力
    /// </summary>
    private void SelectGravity()
    {
        if (rb.velocity.y < 0 && player.Vert < 0)
        {
            //增加重力
            SetGravityScale(_moveData.FastFallGravityMult);
            //限制最大坠落速度，所以当我们从很远的地方坠落时，我们不会加速到疯狂的高速度
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, -_moveData.MaxFastFallSpeed));
        }
        else if (player.IsJumpCut)
        {
            SetGravityScale(_moveData.JumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, -_moveData.MaxFallSpeed));
        }
        else if (rb.velocity.y < 0)
        {
            SetGravityScale(_moveData.FallGravityMult);
        }
        else if ((player.IsJumping || player.IsJumpFalling) && Mathf.Abs(rb.velocity.y) < _moveData.JumpHangTimeThreshold)
        {
            SetGravityScale(_moveData.JumpHangGravityMult);
        }
        else
        {
            SetGravityScale(1);
        }
    }

    private void SetGravityScale(float mult) => rb.gravityScale = _moveData.GravityScale * mult;

    #region JumpHande

    private void JumpDownHandle()
    {
        player.LastPressedJumpTime = _moveData.jumpInputBufferTime;
    }

    private void JumpUpHandle()
    {
        if (CanJumpCut())
            player.IsJumpCut = true;
    }

    #endregion

    private bool CanJumpCut() => player.IsJumping && rb.velocity.y > 0;

    private bool CanJump() => player.LastOnGroundTime > 0 && !player.IsJumping;
}
