using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace State.PlayerState
{
    public class PlayerState : EntityState
    {
        protected Player player;
        protected Rigidbody2D rb;
        public PlayerState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
        {
            player = character as Player;
            rb = character.Rb;


            player.PlayerInput.JumpUpEvent += ChangeJumpStateHandle;
        }

        #region 地面状态转换别的状态
        private void ChangeJumpStateHandle()
        {
            Debug.Log(player.IsJumping+"  "+ player.LastOnGroundTime);

            if (player.LastOnGroundTime > 0 && !player.IsJumping)
            {
                baseStateMachine.ChangeState(player.JumpState);
            }
        }

        #endregion


    }
}


