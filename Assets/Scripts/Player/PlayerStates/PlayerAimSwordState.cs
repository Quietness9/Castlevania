using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.Instance.SwordSkill.ActiveDots();
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.Instance.SwordSkill.HideDots();
    }

    public override void Update()
    {
        base.Update();
    }
}
