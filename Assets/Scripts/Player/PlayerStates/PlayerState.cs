using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;




    public class PlayerState : EntityState
    {
        protected Player player;
        public PlayerState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
        {
            player = character as Player;

            ChangeStateSubscribe();
            
        }

        /// <summary>
        /// 状态改变订阅（键盘或鼠标）
        /// </summary>
        private void ChangeStateSubscribe()
        {
            player.PlayerInput.JumpUpEvent += ChangeJumpStateHandle;
            player.PlayerInput.AttackEvent += ChangeAttackStateHandle;
        }

        #region 地面状态转换别的状态
        private void ChangeJumpStateHandle()
        {

            if (player.LastOnGroundTime > 0 && !player.IsJumping)
            {
                baseStateMachine.ChangeState(player.JumpState);
            }
        }

        private void ChangeAttackStateHandle()
        {
            baseStateMachine.ChangeState(player.AttackState);
        }

        #endregion


    }



