using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    PlayerMoveData _moveData;
    float gravityScale;

    public PlayerJumpState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _moveData=player.MoveData;
        gravityScale=rb.gravityScale;

        Jump();
    }

    public override void Update()
    {
        base.Update();
        player.Animator_CT.SetFloat("yVelocity", rb.velocity.y);


        JumpController();
        SelectGravity();

        if (!player.IsJumping&&!player.IsJumpFalling)
        {
            baseStateMachine.ChangeState(player.IdleState);
        }
    }

  
    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = gravityScale;
        rb.velocity=new Vector3(rb.velocity.x,0);
        player.Animator_CT.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Jump()
    {
        player.IsJumping = true;
        player.IsJumpFalling = false;

        //ȷ�����ܶ����Ծ
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
            player.IsJumpFalling = false;
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
        else if (rb.velocity.y < 0)
        {
            SetGravityScale(_moveData.FallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, -_moveData.MaxFallSpeed));
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

    private void SetGravityScale(float mult) => rb.gravityScale *= mult;

    #region JumpHande


    #endregion
}
