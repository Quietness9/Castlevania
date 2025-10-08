using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    public PlayerCounterAttackState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityZero();
        timer = player.CounterAttackDuration;

        player.Animator_CT.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy.IsSetStunnedState())
            {
                timer = 4f;
                player.Animator_CT.SetBool("SuccessfulCounterAttack", true);
            }
        }

        if (timer < 0.01f || triggerFinish)
        {
            baseStateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


}
