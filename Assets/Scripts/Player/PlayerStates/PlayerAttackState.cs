using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class PlayerAttackState : PlayerState
    {
        int _comboCounter;
        float _attackLastTime;
        float _comboWindow = 2;//连击持续时间

        public PlayerAttackState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityZero();

            if (_comboCounter > 2 || Time.time > _attackLastTime + _comboWindow)
            {
                _comboCounter = 0;
            }

            player.Animator_CT.SetInteger("ComboCounter", _comboCounter);
            //使移动时可以轻微移动，让动作更加丝滑
            player.Rb.AddForce(Vector2.right * player.Direction * player.AttackMovement[_comboCounter], ForceMode2D.Impulse);

            player.Animator_CT.speed = player.AnimationSpeed;
        }

        public override void Update()
        {
            base.Update();


            if (triggerFinish)
            {
                player.CharacterStateMachine.ChangeState(player.IdleState);
            }


        }

        public override void Exit()
        {
            base.Exit();
            player.Animator_CT.speed = 1f;
            _comboCounter++;

            _attackLastTime= Time.time;
        }
    }



