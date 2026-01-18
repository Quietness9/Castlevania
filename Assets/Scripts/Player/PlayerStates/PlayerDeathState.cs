using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InGameUICtr.Instance.PlayerDeathEffect();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
