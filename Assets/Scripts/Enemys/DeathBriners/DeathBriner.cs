using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner : Enemy
{
    public BoxCollider2D Bd2d { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Bd2d = GetComponent<BoxCollider2D>();
    }


    protected override void Start()
    {
        base.Start();
        InitDeathBriner();
    }

    private void Update()
    {
        CharacterStateMachine.CurrentState.Update();
    }

    /// <summary>
    /// 初始化死亡布林
    /// </summary>
    private void InitDeathBriner()
    {
        IsFacingRight = false;
        CharacterStateMachine.InitState(IdleState);
    }

    #region 死亡布林创建状态

    protected override EnemyAttackState CreateAttackState() => new DeathBrinerAttackState(this, this, CharacterStateMachine, "Attack");
    protected override EnemyBattleState CreateBattleState() => new DeathBrinerBattleState(this, this, CharacterStateMachine, "Move");
    protected override EnemyDeathState CreateDeathState() => new DeathBrinerDeathState(this, this, CharacterStateMachine, "Death");
    protected override EnemyIdleState CreateIdleState() => new DeathBrinerIdleState(this, this, CharacterStateMachine, "Idle");
    protected override EnemyMoveState CreateMoveState() => new DeathBrinerMoveState(this, this, CharacterStateMachine, "Move");
    

    #endregion
}
