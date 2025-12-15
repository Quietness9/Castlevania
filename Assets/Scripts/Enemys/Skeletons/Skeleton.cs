using UnityEngine;

public class Skeleton : Enemy
{

    public BoxCollider2D Bd2d { get; private set; }

    #region 状态
    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonMoveState MoveState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonStunnedState StunnedState { get; private set; }

    public SkeletonDeathState DeathState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        IdleState = new SkeletonIdleState(this, this, CharacterStateMachine, "Idle");
        MoveState = new SkeletonMoveState(this, this, CharacterStateMachine, "Move");
        BattleState = new SkeletonBattleState(this, this, CharacterStateMachine, "Move");
        AttackState = new SkeletonAttackState(this, this, CharacterStateMachine, "Attack");
        StunnedState = new SkeletonStunnedState(this, this, CharacterStateMachine, "Stunned");
        DeathState = new SkeletonDeathState(this, this, CharacterStateMachine, "Die");

        Bd2d = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        EventSubscribe();
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

    private void OnDisable()
    {
        EventUnsubscribe();
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

    private void InitSkeleton()
    {
        IsFacingRight = true;
        CharacterStateMachine.InitState(IdleState);
    }

    /// <summary>
    /// 事件订阅
    /// </summary>
    private void EventSubscribe()
    {
        if (Attribute == null)
        {
            Debug.LogWarning("Attribute is null");
            return;
        }

        Attribute.OnDieEvent += ChangeDieStateHandle;
    }

    /// <summary>
    /// 取消事件订阅
    /// </summary>
    private void EventUnsubscribe()
    {
        if (Attribute == null)
        {
            Debug.LogWarning("Attribute is null");
            return;
        }

        Attribute.OnDieEvent -= ChangeDieStateHandle;
    }

    /// <summary>
    /// 切换到死亡状态并掉落物品
    /// </summary>
    private void ChangeDieStateHandle()
    {
        Debug.Log("Skeleton Die");
        CharacterStateMachine.ChangeState(DeathState);
        ItemDrop.DropItem();
    }
}
