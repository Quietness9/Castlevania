using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update()
    {
        base.Update();

        if (player.Hor!=0)
        {
            baseStateMachine.ChangeState(player.MoveState);
        }

        
    }

    public override void Exit()
    {
        base.Exit();
    }

    
}
