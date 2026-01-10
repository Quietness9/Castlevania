using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundState : EntityState
{
    protected Enemy enemy;

    public EnemyGroundState(Character character,Enemy enemy, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        this.enemy = enemy;
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

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, GlobalReferencesManager.Instance.GamePlayer.transform.position) < enemy.EnemyStateData.IgnoreDistance)
        {
            baseStateMachine.ChangeState(enemy.BattleState);
        }
    }
}
