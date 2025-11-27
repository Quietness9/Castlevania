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
            _moveData = player.MoveData;
            gravityScale = player.Rb.gravityScale;

            Jump();
        }



        public override void Exit()
        {
            base.Exit();

            player.Rb.gravityScale = gravityScale;
            player.Rb.velocity = new Vector3(player.Rb.velocity.x, 0);

            player.IsJumping = false;

            player.Animator_CT.SetFloat("yVelocity", player.Rb.velocity.y);
        }
        public override void Update()
        {
            base.Update();
            player.Animator_CT.SetFloat("yVelocity", player.Rb.velocity.y);


            JumpController();
            SelectGravity();

            if (!player.IsJumping && !player.IsJumpFalling)
            {
                baseStateMachine.ChangeState(player.IdleState);
            }
        }

        private void Jump()
        {
            player.IsJumping = true;
            player.IsJumpFalling = false;

            //确保不能多次跳跃
            player.LastOnGroundTime = 0;

            //当我们下落时，我们加大所施加的力
            //这意味着我们总是觉得我们跳了相同的量
            //（事先将玩家的Y轴速度设置为0可能会产生相同的效果，但我发现这更优雅：D）
            float force = _moveData.JumpForce;
            if (player.Rb.velocity.y < 0)
                force -= player.Rb.velocity.y;


            player.Rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        /// <summary>
        /// 跳跃控制
        /// </summary>
        private void JumpController()
        {
            if (player.IsJumping && player.Rb.velocity.y < 0)
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
        /// 根据不同状态，选择在不同重力
        /// </summary>
        private void SelectGravity()
        {
            if (player.Rb.velocity.y < 0 && player.Vert < 0)
            {
                //增加重力
                SetGravityScale(_moveData.FastFallGravityMult);
                //限制最大坠落速度，所以当我们从很远的地方坠落时，我们不会加速到疯狂的高速度
                player.Rb.velocity = new Vector2(player.Rb.velocity.x, Mathf.Min(player.Rb.velocity.y, -_moveData.MaxFastFallSpeed));
            }
            else if (player.Rb.velocity.y < 0)
            {
                SetGravityScale(_moveData.FallGravityMult);
                player.Rb.velocity = new Vector2(player.Rb.velocity.x, Mathf.Min(player.Rb.velocity.y, -_moveData.MaxFallSpeed));
            }
            else if ((player.IsJumping || player.IsJumpFalling) && Mathf.Abs(player.Rb.velocity.y) < _moveData.JumpHangTimeThreshold)
            {
                SetGravityScale(_moveData.JumpHangGravityMult);
            }
            else
            {
                SetGravityScale(1);
            }
        }

        private void SetGravityScale(float mult) => player.Rb.gravityScale *= mult;
    }



