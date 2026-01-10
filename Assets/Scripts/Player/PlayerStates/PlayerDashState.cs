using UnityEngine;

public class PlayerDashState : PlayerState
{
   
    public PlayerDashState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.Instance.CloneSkill.CreateCloneOnDashStart(player.transform);
        player.Attribute.MakeInvincible();
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.Instance.CloneSkill.CreateCloneOnDashEnd(player.transform);
        player.Attribute.CancelInvincible();
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



