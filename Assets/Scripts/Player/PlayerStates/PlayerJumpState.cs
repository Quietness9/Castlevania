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
        //ȷ�����ܶ����Ծ
        player.LastPressedJumpTime = 0;
        player.LastOnGroundTime = 0;

        //����������ʱ�����ǼӴ���ʩ�ӵ���
        //����ζ���������Ǿ�������������ͬ����
        //�����Ƚ���ҵ�Y���ٶ�����Ϊ0���ܻ������ͬ��Ч�������ҷ���������ţ�D��
        float force = _moveData.JumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// ��Ծ����
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
    /// ���ݲ�ͬ״̬��ѡ���ڲ�ͬ����
    /// </summary>
    private void SelectGravity()
    {
        if (rb.velocity.y < 0 && player.Vert < 0)
        {
            //��������
            SetGravityScale(_moveData.FastFallGravityMult);
            //�������׹���ٶȣ����Ե����ǴӺ�Զ�ĵط�׹��ʱ�����ǲ�����ٵ����ĸ��ٶ�
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
