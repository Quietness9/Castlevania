using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleState : EntityState
{
    protected Transform playerTransform;
    protected Enemy enemy;
    protected float moveDir;

    public EnemyBattleState(Character character,Enemy enemy, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (playerTransform == null)
        {
            playerTransform = GlobalReferencesManager.Instance.GamePlayer.transform;
        }

        if (enemy.IsPlayerDetected() == false)
        {
            enemy.TurnDirection();
        }


        if (playerTransform.GetComponent<PlayerAttribute>().IsDie)
        {
            baseStateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            timer = enemy.EnemyStateData.BattleTime;
            if (enemy.IsPlayerDetected().distance < enemy.AttackCheckRadius && CanAttack())
            {
                baseStateMachine.ChangeState(enemy.AttackState);
            }

        }
        else
        {
            if (timer < 0 || Vector2.Distance(playerTransform.position, enemy.transform.position) > enemy.EnemyStateData.IgnoreDistance)
            {
                baseStateMachine.ChangeState(enemy.IdleState);
            }
        }

        if (playerTransform.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (playerTransform.transform.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (!enemy.IsGroundCheck())
        {
            moveDir *= -1;
        }

        if (enemy.IsPlayerDetected()&& enemy.IsPlayerDetected().distance>enemy.AttackCheckRadius-0.1f)
        {
            enemy.SetVelocity(moveDir * enemy.EnemyStateData.MoveSpeed, enemy.Rb.velocity.y);
        }
    }

    /// <summary>
    /// ÅÐ¶ÏÊÇ·ñ¿ÉÒÔ¹¥»÷
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        if (Time.time > enemy.AttackLastTime + enemy.AttackCooldown)
        {
            enemy.AttackCooldown = Random.Range(enemy.EnemyStateData.AttackCooldownOffset.x,
                enemy.EnemyStateData.AttackCooldownOffset.y);

            enemy.AttackLastTime = Time.time;
            return true;
        }

        return false;
    }
}
