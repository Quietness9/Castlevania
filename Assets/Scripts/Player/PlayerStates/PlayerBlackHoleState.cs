using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    float _defaultGravity;
    bool _isCreateBlackHole;

    public PlayerBlackHoleState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _defaultGravity=player.Rb.gravityScale;
        player.Rb.gravityScale = 0;
        _isCreateBlackHole=true;
        timer = SkillManager.Instance.BlackSkill.PlayerBlackHoleData.FlyTime;
    }

    public override void Exit()
    {
        base.Exit();
        player.Rb.gravityScale = _defaultGravity;
        player.Rb.velocity=new Vector2(player.Rb.velocity.x,0);

        player.Animator_CT.SetFloat("yVelocity", player.Rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        player.Animator_CT.SetFloat("yVelocity", player.Rb.velocity.y);

        if (timer > 0)
        {
            player.Rb.velocity = new Vector2(0, SkillManager.Instance.BlackSkill.PlayerBlackHoleData.FlySpeed);
        }

        if (timer < 0)
        {
            
            if (_isCreateBlackHole)
            {
                player.Rb.velocity=Vector2.zero;
                SkillManager.Instance.BlackSkill.CreateBlackHole();
                player.CharacterTransparent(true);
                _isCreateBlackHole = false;
            }
        }

        if (SkillManager.Instance.BlackSkill.IsEnd)
        {
            player.Rb.velocity = new Vector2(0, -SkillManager.Instance.BlackSkill.PlayerBlackHoleData.LandSpeed);
        }

        if (SkillManager.Instance.BlackSkill.IsEnd&&player.IsGroundCheck())
        {
            player.CharacterStateMachine.ChangeState(player.IdleState);
        }

    }
}
