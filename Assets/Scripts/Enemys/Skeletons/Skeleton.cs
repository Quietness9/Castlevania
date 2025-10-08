public class Skeleton : Enemy
{

    #region 状态
    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonMoveState MoveState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonStunnedState StunnedState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        IdleState = new SkeletonIdleState(this, this, CharacterStateMachine, "Idle");
        MoveState = new SkeletonMoveState(this, this, CharacterStateMachine, "Move");
        BattleState = new SkeletonBattleState(this, this, CharacterStateMachine, "Move");
        AttackState = new SkeletonAttackState(this, this, CharacterStateMachine, "Attack");
        StunnedState = new SkeletonStunnedState(this, this, CharacterStateMachine, "Stunned");
    }

    private void Start()
    {
        InitSkeleton();
    }

    private void Update()
    {
        CharacterStateMachine.CurrentState.Update();
    }

    private void InitSkeleton()
    {
        IsFacingRight = true;

        CharacterStateMachine.InitState(IdleState);
    }

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
}
