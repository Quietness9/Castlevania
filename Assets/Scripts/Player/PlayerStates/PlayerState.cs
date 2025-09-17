using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : EntityState
{
    protected Player player;
    protected Rigidbody2D rb;
    public PlayerState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        player = character as Player;
        rb=character.Rb;

        player.PlayerInput.JumpDownEvent += ChangeJumpStateHandle;
    }

    #region 地面状态转换别的状态
    private void ChangeJumpStateHandle()
    {
        if (player.LastOnGroundTime > 0 && !player.IsJumping)
        {
            baseStateMachine.ChangeState(player.JumpState);
        }
    }

    #endregion

    
}
