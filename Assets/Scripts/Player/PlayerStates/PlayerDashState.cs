public class PlayerDashState : PlayerState
{
    public PlayerDashState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.Instance.CloneSkill.CreateClonePlayer();
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
            player.CharacterStateMachine.ChangeState(player.IdleState);
        }
    }
}



