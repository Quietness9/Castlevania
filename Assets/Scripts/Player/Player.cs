using GameInputSystem;
using System;
using UnityEngine;

public class Player : Character
{
    [Header("可配置数据")]
    public PlayerInputReader PlayerInput;
    public PlayerMoveData MoveData;

    [Header("动画配置")]
    public float AnimationSpeed;

    [Header("攻击")]
    public Vector2[] AttackMovement;
    public float CounterAttackDuration;

    [Header("EnemyStunned")]
    public Vector2 StunnedForce;
    public float StunnedDuration;

    public float Hor { get;private set; }
    public float Vert { get;private set; }

    //跳跃
    public bool IsJumpCut { get;set; }
    public bool IsJumpFalling { get;set; }
    public bool IsJumping { get;set; }


    //Timer
    public float LastOnGroundTime { get;set; }


    public SkillManager PlayerSkill { get; private set; }

    #region 状态

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get;private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        IdleState = new PlayerIdleState(this, CharacterStateMachine, "Idle");
        MoveState = new PlayerMoveState(this, CharacterStateMachine, "Move");
        JumpState = new PlayerJumpState(this, CharacterStateMachine, "Jump");
        DashState = new PlayerDashState(this, CharacterStateMachine, "Dash");
        AttackState = new PlayerAttackState(this, CharacterStateMachine, "Attack");
        CounterAttackState = new PlayerCounterAttackState(this, CharacterStateMachine, "CounterAttack");
        AimSwordState = new PlayerAimSwordState(this, CharacterStateMachine, "AimSword");
        CatchSwordState = new PlayerCatchSwordState(this, CharacterStateMachine, "CatchSword");

    }

    private void Start()
    {
        GlobalReferencesManager.Instance.GamePlayer = this;
        PlayerSkill = SkillManager.Instance;

        InitPlayer();
        ChangeStateSubscribe();
    }

    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;      

        if (!IsJumping)
        {
            if (base.IsGroundCheck())
            {
                LastOnGroundTime = MoveData.coyoteTime;
            }
        } 


        CharacterStateMachine.CurrentState.Update();
    }

    

    private void OnDestroy()
    {
        PlayerInput.MoveEvent -= GetDirectionHandle;
    }

    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        IsFacingRight = true;
        Direction = 1;

        CharacterStateMachine.InitState(IdleState);
        PlayerInput.MoveEvent += GetDirectionHandle;
    }

    /// <summary>
    /// 状态改变订阅（键盘或鼠标）
    /// </summary>
    private void ChangeStateSubscribe()
    {
        PlayerInput.JumpUpEvent += ChangeJumpStateHandle;
        PlayerInput.AttackEvent += ChangeAttackStateHandle;
        PlayerInput.CounterAttackEvent += ChangeCounterAttackStateHandle;
        PlayerInput.AimSwordEvent += ChangeAimSwordStateHandle;
        PlayerInput.CancelSwordEvent += ChangeIdleStateHandle;
    }

    #region EventHandle

    /// <summary>
    /// 改变方向订阅
    /// </summary>
    /// <param name="direction"></param>
    public void GetDirectionHandle(Vector2 moveDire)
    {
        Hor = moveDire.x;
        Vert = moveDire.y;

        if (Hor*Direction<0)
        {
            TurnDirection();
        }
    }

    #region 地面状态转换别的状态

    /// <summary>
    /// 转换到跳跃状态
    /// </summary>
    private void ChangeJumpStateHandle()
    {

        if (LastOnGroundTime > 0 && !IsJumping)
        {
            CharacterStateMachine.ChangeState(JumpState);
        }
    }

    /// <summary>
    /// 转换攻击状态
    /// </summary>
    private void ChangeAttackStateHandle()
    {
        CharacterStateMachine.ChangeState(AttackState);
    }

    /// <summary>
    /// 转换为连击状态
    /// </summary>
    private void ChangeCounterAttackStateHandle()
    {
        CharacterStateMachine.ChangeState(CounterAttackState);
    }

    /// <summary>
    /// 转换为剑的瞄准状态
    /// </summary>
    private void ChangeAimSwordStateHandle()
    {
        CharacterStateMachine.ChangeState(AimSwordState);
    }

    /// <summary>
    /// 转变为空闲状态
    /// </summary>
    private void ChangeIdleStateHandle()
    {
        CharacterStateMachine.ChangeState(IdleState);
    }

    #endregion

    #endregion
}