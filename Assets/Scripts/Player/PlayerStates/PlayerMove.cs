using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerMove : PlayerState
{

    public PlayerMove(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.IsGroundCheck();
        Move();

        if (player.Hor == 0)
        {
            baseStateMachine.ChangeState(player.IdleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    

    private void Move()
    {   

        float targetSpeed = player.Hor * player.MoveData.MaxSpeed;

        float accelRate;

        if (player.LastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player.MoveData.AccelForce : player.MoveData.DecreForce;
        }

        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player.MoveData.AccelForce * player.MoveData.AccelInAir 
                : player.MoveData.DecreForce * player.MoveData.DecreInAir;
        }

        if(player.MoveData.doConserveMomentum && Mathf.Abs(player.Rb.velocity.x) > Mathf.Abs(targetSpeed) && 
            Mathf.Sign(player.Rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f &&player.LastOnGroundTime < 0)
        {
            accelRate = 0;
        }

        float speedDif = targetSpeed - player.Rb.velocity.x;
        float movement = speedDif * accelRate;

        player.Rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }
}
