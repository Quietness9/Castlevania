using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();

        }

        public override void Update()
        {
            base.Update();

            player.Animator_CT.SetFloat("yVelocity", player.Rb.velocity.y);

            if (player.Hor != 0)
            {
                baseStateMachine.ChangeState(player.MoveState);
            }


        }

        public override void Exit()
        {
            base.Exit();
        }


    }



