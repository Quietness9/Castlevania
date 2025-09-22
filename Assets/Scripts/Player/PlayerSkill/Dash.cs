using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSkill
{

    public class Dash : Skill
    {
        public float DashForce;


        protected override void Awake()
        {
            base.Awake();
            player.PlayerInput.DashEvent += DashHandle;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            player.PlayerInput.DashEvent -= DashHandle;
        }


        private void DashHandle()
        {
            if (CanUseSkill())
            {
                player.Rb.AddForce(Vector2.right*player.Direction*DashForce,ForceMode2D.Impulse);
                player.CharacterStateMachine.ChangeState(player.DashState);
            }
        }
    }

}


