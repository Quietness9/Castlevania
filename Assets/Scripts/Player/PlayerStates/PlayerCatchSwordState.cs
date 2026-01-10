using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    Transform _swordTransform=>player.SwordObj.transform;

    public PlayerCatchSwordState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.transform.position.x > _swordTransform.position.x && player.IsFacingRight)
        {
            player.TurnDirection();
        }
        else if (player.transform.position.x < _swordTransform.position.x && !player.IsFacingRight)
        {
            player.TurnDirection();
        }

        player.PlayerFx.PlayDustFx();
        player.PlayerFx.ScreenShakeFx(player.PlayerFx.PlayerFxData.ShakeSwordImpact);


        player.Rb.AddForce(new Vector2(player.SwordReturnForce * -player.Direction, player.Rb.velocity.y), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerFinish)
        {
            baseStateMachine.ChangeState(player.IdleState);
        }
    }
}
