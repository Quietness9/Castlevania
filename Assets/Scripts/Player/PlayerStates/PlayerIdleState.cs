public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.Animator_CT.SetFloat("yVelocity", player.Rb.velocity.y);

        if (player.Hor != 0&&player.IsGroundCheck())
        {
            baseStateMachine.ChangeState(player.MoveState);
        }


    }

}



