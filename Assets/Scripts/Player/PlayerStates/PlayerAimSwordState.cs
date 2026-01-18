using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{

    Vector2 _mousePosition;

    public PlayerAimSwordState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillMgr.Instance.SwordSkill.ActiveDots();
        player.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
        SkillMgr.Instance.SwordSkill.HideDots();
    }

    public override void Update()
    {
        base.Update();
        
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > _mousePosition.x && player.IsFacingRight)
        {
            player.TurnDirection();
        }else if (player.transform.position.x < _mousePosition.x && !player.IsFacingRight)
        {
            player.TurnDirection();
        }

    }
}
