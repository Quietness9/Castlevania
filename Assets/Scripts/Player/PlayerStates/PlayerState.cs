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
        /// ×´Ì¬¸Ä±ä¶©ÔÄ£¨¼üÅÌ»òÊó±ê£©
        /// </summary>
        private void ChangeStateSubscribe()
        {
            player.PlayerInput.JumpUpEvent += ChangeJumpStateHandle;
            player.PlayerInput.AttackEvent += ChangeAttackStateHandle;
            player.PlayerInput.CounterAttackEvent += ChangeCounterAttackStateHandle;
        }

        #region µØÃæ×´Ì¬×ª»»±ðµÄ×´Ì¬

        /// <summary>
        /// ×ª»»µ½ÌøÔ¾×´Ì¬
        /// </summary>
        private void ChangeJumpStateHandle()
        {

            if (player.LastOnGroundTime > 0 && !player.IsJumping)
            {
                baseStateMachine.ChangeState(player.JumpState);
            }
        }

        /// <summary>
        /// ×ª»»¹¥»÷×´Ì¬
        /// </summary>
        private void ChangeAttackStateHandle()
        {
            baseStateMachine.ChangeState(player.AttackState);
        }

        private void ChangeCounterAttackStateHandle()
    {
        baseStateMachine.ChangeState(player.CounterAttackState);
    }
        #endregion


    }



