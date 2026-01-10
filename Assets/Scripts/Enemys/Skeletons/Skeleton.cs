using UnityEngine;

public class Skeleton : Enemy
{

    public BoxCollider2D Bd2d { get; private set; }

    #region 状态
    public SkeletonStunnedState StunnedState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        StunnedState = new SkeletonStunnedState(this, this, CharacterStateMachine, "Stunned");

        Bd2d = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        InitSkeleton();
    }

    private void Update()
    {
        CharacterStateMachine.CurrentState.Update();
    }

    #region 骷髅创建状态

    protected override EnemyIdleState CreateIdleState()=> new SkeletonIdleState(this, this, CharacterStateMachine, "Idle");
    protected override EnemyMoveState CreateMoveState() => new SkeletonMoveState(this, this, CharacterStateMachine, "Move");
    protected override EnemyAttackState CreateAttackState() => new SkeletonAttackState(this, this, CharacterStateMachine, "Attack");
    protected override EnemyBattleState CreateBattleState() => new SkeletonBattleState(this, this, CharacterStateMachine, "Move");
    protected override EnemyDeathState CreateDeathState() => new SkeletonDeathState(this, this, CharacterStateMachine, "Die");

    #endregion

    /// <summary>
    /// 判断是否可以切换Stunned状态
    /// </summary>
    /// <returns></returns>
    public override bool IsSetStunnedState()
    {
        if (base.IsSetStunnedState())
        {
            CharacterStateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 初始化骷髅
    /// </summary>
    private void InitSkeleton()
    {
        IsFacingRight = true;
        CharacterStateMachine.InitState(IdleState);
    }
}
