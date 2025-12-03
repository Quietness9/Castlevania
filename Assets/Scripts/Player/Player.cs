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
    public Vector2 CounterAttackOffset;
    public float SwordReturnForce;

    [Header("敌人眩晕")]
    public Vector2 StunnedForce;
    public float StunnedDuration;

    public float Hor { get;private set; }
    public float Vert { get;private set; }
    public GameObject SwordObj { get;private set; }

    //跳跃
    public bool IsJumpCut { get;set; }
    public bool IsJumpFalling { get;set; }
    public bool IsJumping { get;set; }


    //Timer
    public float LastOnGroundTime { get;set; }


    #region 状态

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get;private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackHoleState BlackHoleState { get; private set; }

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
        BlackHoleState = new PlayerBlackHoleState(this, CharacterStateMachine, "Jump");

    }

    private void Start()
    {
        GlobalReferencesManager.Instance.GamePlayer = this;

        InitPlayer();
        EventSubscribe();
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
        EventUnsubscribe();
    }

    /// <summary>
    /// 获得新剑
    /// </summary>
    public void GetNewSword(GameObject newSword)
    {
        SwordObj = newSword;
    }

    /// <summary>
    /// 抓住扔出的的剑
    /// </summary>
    public void CatchSword()
    {
        CharacterStateMachine.ChangeState(CatchSwordState);

        if (SwordObj)
        {
            Destroy(SwordObj);
        }
    }

    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        IsFacingRight = true;
        Direction = 1;

        CharacterStateMachine.InitState(IdleState);
        
    }

    /// <summary>
    /// 事件订阅（键盘或鼠标）
    /// </summary>
    private void EventSubscribe()
    {
        if (PlayerInput == null)
        {
            Debug.LogWarning("PlayerInput is null");
            return;
        }

        PlayerInput.MoveEvent += GetDirectionHandle;

        PlayerInput.JumpUpEvent += ChangeJumpStateHandle;
        PlayerInput.AttackEvent += ChangeAttackStateHandle;
        PlayerInput.CounterAttackEvent += ChangeCounterAttackStateHandle;
        PlayerInput.AimSwordEvent += ChangeAimSwordStateHandle;
        PlayerInput.CancelSwordEvent += ChangeIdleStateHandle;
    }

    /// <summary>
    /// 取消事件订阅
    /// </summary>
    private void EventUnsubscribe()
    {
        if (PlayerInput == null)
        {
            Debug.LogWarning("PlayerInput is null");
            return;
        }

        PlayerInput.MoveEvent -= GetDirectionHandle;

        PlayerInput.JumpUpEvent -= ChangeJumpStateHandle;
        PlayerInput.AttackEvent -= ChangeAttackStateHandle;
        PlayerInput.CounterAttackEvent -= ChangeCounterAttackStateHandle;
        PlayerInput.AimSwordEvent -= ChangeAimSwordStateHandle;
        PlayerInput.CancelSwordEvent -= ChangeIdleStateHandle;
    }

    #region EventHandle

    /// <summary>
    /// 改变方向订阅
    /// </summary>
    /// <param name="direction"></param>
    private void GetDirectionHandle(Vector2 moveDire)
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
        if (!SwordObj)
        {
            CharacterStateMachine.ChangeState(AimSwordState);
        }
        else
        {
            SwordObj.GetComponent<SwordController>().ReturnSword();
        }
        
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