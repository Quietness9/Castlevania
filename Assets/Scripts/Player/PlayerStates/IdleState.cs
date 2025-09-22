using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State.PlayerState
{
    public class IdleState : PlayerState
    {
        public IdleState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();

        }

        public override void Update()
        {
            base.Update();

            player.Animator_CT.SetFloat("yVelocity", rb.velocity.y);

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

}

