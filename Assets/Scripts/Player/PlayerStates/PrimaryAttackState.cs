using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State.PlayerState
{
    public class PrimaryAttackState : PlayerState
    {
        int _comboCounter;
        float _attackLastTime;
        float _comboWindow = 2;//连击持续时间

        public PrimaryAttackState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            //Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackCheckRadius);
            if (_comboCounter > 2 || Time.time > _attackLastTime + _comboWindow)
            {
                _comboCounter = 0;
            }

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}


