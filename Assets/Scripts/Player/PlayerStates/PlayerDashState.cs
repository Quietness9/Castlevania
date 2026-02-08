using UnityEngine;

public class PlayerDashState : PlayerState
{
   
    public PlayerDashState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillMgr.Instance.CloneSkill.CreateCloneOnDashStart(player.transform);
        player.Attribute.SetInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        SkillMgr.Instance.CloneSkill.CreateCloneOnDashEnd(player.transform);
        player.Attribute.SetInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (triggerFinish)
        {
            player.CharacterStateMachine.ChangeState(player.IdleState);
        }

        player.PlayerFx.CreateDashShadow();
    }
}



