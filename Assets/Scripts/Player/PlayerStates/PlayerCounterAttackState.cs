using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    bool _canCreateClone;

    public PlayerCounterAttackState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityZero();
        timer = player.CounterAttackDuration;
        _canCreateClone = true;

        player.Animator_CT.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy)&&enemy.IsSetStunnedState())
            {
                timer = 4f;
                player.Animator_CT.SetBool("SuccessfulCounterAttack", true);
                if (_canCreateClone)
                {
                    _canCreateClone = false;
                    SkillManager.Instance.CloneSkill.CreateCloneOnCounterAttack(enemy.transform, -player.Direction * player.CounterAttackOffset);
                }
            }
        }

        if (timer < 0.01f || triggerFinish)
        {
            baseStateMachine.ChangeState(player.IdleState);
        }
    }

    


}
