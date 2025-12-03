public class PlayerDashState : PlayerState
{
    public PlayerDashState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.Instance.CloneSkill.CreateCloneOnDashStart(player.transform);
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.Instance.CloneSkill.CreateCloneOnDashEnd(player.transform);
    }

    public override void Update()
    {
        base.Update();

        if (triggerFinish)
        {
            player.CharacterStateMachine.ChangeState(player.IdleState);
        }
    }
}



