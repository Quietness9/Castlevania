using GameInputSystem;
using System;
using UnityEngine;

public class Player : Character
{
    [Header("可配置数据")]
    public PlayerInputReader PlayerInput;
    public PlayerMoveData MoveData;
    public PlayerCurrencyData CurrencyData;

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

    //默认值
    float _defaultMoveSpeed;
    float _defaultJumpForce;
    float _defaultDashForce;

    //移动
    public float Hor { get;private set; }
    public float Vert { get;private set; }

    //跳跃
    public bool IsJumpCut { get;set; }
    public bool IsJumpFalling { get;set; }
    public bool IsJumping { get;set; }


    //Timer
    public float LastOnGroundTime { get;set; }

    #region 组件
    public GameObject SwordObj { get;private set; }

    public CapsuleCollider2D Clc2d { get; private set; }

    public PlayerFx PlayerFx {  get; private set; }

    #endregion

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
    public PlayerDeathState DeathState { get; private set; }


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
        DeathState = new PlayerDeathState(this, CharacterStateMachine, "Die");

        Clc2d = GetComponent<CapsuleCollider2D>();

        PlayerFx = Fx as PlayerFx;

        GameEventMgr.OnSaveGame += SaveGameData;
        GameEventMgr.OnLoadGame += LoadGameData;
    }
    private void OnEnable()
    {       
        EventSubscribe();
    }

    private void Start()
    {
        InitPlayer();
        GlobalReferencesMgr.Instance.GamePlayer = this;
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

    private void OnDisable()
    {
        EventUnsubscribe();
    }

    private void OnDestroy()
    {
        GameEventMgr.OnSaveGame -= SaveGameData;
        GameEventMgr.OnLoadGame -= LoadGameData;
    }

    public override void SlowCharacterSpeed(float slowRatio)
    {
        base.SlowCharacterSpeed(slowRatio);
        MoveData.MaxMoveSpeed *= (1 - slowRatio);
        MoveData.JumpForce *= (1 - slowRatio);
        MoveData.DashForce *= (1 - slowRatio);

    }

    public override void ReturnCharacterDefaultSpeed()
    {
        base.ReturnCharacterDefaultSpeed();
        MoveData.MaxFallSpeed = _defaultMoveSpeed;
        MoveData.JumpForce = _defaultJumpForce;
        MoveData.DashForce = _defaultDashForce;
    }


    /// <summary>
    /// 判断是否有足够的货币
    /// </summary>
    /// <param name="goldCoin"></param>
    /// <param name="soul"></param>
    /// <returns></returns>
    public bool HaveEnoughMoney(int goldCoin,int soul)
    {
        if (CurrencyData.GoldCoin < goldCoin||CurrencyData.Soul<soul)
        {
            Debug.Log("货币不足现持有货币为" + CurrencyData.GoldCoin+"--"+CurrencyData.Soul);
            return false;
        }

        CurrencyData.ReduceGoldCoin(goldCoin);
        CurrencyData.ReduceSoul(soul);

        return true;
    }

    #region 剑

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

    #endregion

    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        Direction = 1;
        IsFacingRight = true;
        _defaultMoveSpeed = MoveData.MaxMoveSpeed;
        _defaultJumpForce =MoveData.JumpForce;
        _defaultDashForce=MoveData.DashForce;

        CharacterStateMachine.InitState(IdleState);
        
    }


    #region EventHandle

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

        PlayerInput.OnMoveEvent += GetDirectionHandle;
        PlayerInput.OnJumpUpEvent += ChangeJumpStateHandle;
        PlayerInput.OnAttackEvent += ChangeAttackStateHandle;

        PlayerInput.OnAimSwordEvent += ChangeAimSwordStateHandle;
        PlayerInput.OnCancelSwordEvent += ChangeIdleStateHandle;

        PlayerInput.OnUseFlaskEvent += UseFlaskHandle;

        if (Attribute == null)
        {
            Debug.LogWarning("Attribute is null");
            return;
        }

        Attribute.OnDieEvent += ChangDieStateHandle;

        if (SaveAndLoadMgr.Instance == null)
        {
            Debug.LogWarning("SaveAndLoadManager is null");
            return;
        }
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

        PlayerInput.OnMoveEvent -= GetDirectionHandle;
        PlayerInput.OnJumpUpEvent -= ChangeJumpStateHandle;
        PlayerInput.OnAttackEvent -= ChangeAttackStateHandle;

        PlayerInput.OnAimSwordEvent -= ChangeAimSwordStateHandle;
        PlayerInput.OnCancelSwordEvent -= ChangeIdleStateHandle;


        PlayerInput.OnUseFlaskEvent -= UseFlaskHandle;


        if (Attribute == null)
        {
            Debug.LogWarning("Attribute is null");
            return;
        }

        Attribute.OnDieEvent -= ChangDieStateHandle;

    }


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

    /// <summary>
    /// 使用药瓶道具订阅
    /// </summary>
    private void UseFlaskHandle()
    {
        EquipmentItemData equipment=InventoryController.Instance.GetEquipment(EquipmentItemType.Flask);
        if (equipment != null)
        {
            if (InventoryController.Instance.CanUseEquipment(EquipmentItemType.Flask, equipment))
            {
                InGameUICtr.Instance.FlaskImageCooldown();
                equipment.UseEquipmentEffect(transform);
            }
            
        }
    }

    #region 状态转换别的状态

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
    /// 转换为剑的瞄准状态
    /// </summary>
    private void ChangeAimSwordStateHandle()
    {
        if(SkillMgr.Instance.SwordSkill.IsLock==false)
            return;

        if (!SwordObj)
        {
            CharacterStateMachine.ChangeState(AimSwordState);
        }
        else
        {
            SwordObj.GetComponent<SwordCtr>().ReturnSword();
        }
        
    }

    /// <summary>
    /// 转变为空闲状态
    /// </summary>
    private void ChangeIdleStateHandle()
    {
        CharacterStateMachine.ChangeState(IdleState);
    }

    /// <summary>
    /// 转为死亡状态并掉落物品
    /// </summary>
    private void ChangDieStateHandle()
    {
        Debug.Log("Player Die");
        CharacterStateMachine.ChangeState(DeathState);
        ItemDrop.DropItem();
        CurrencyData.ReduceGoldCoin(coldCoin);
        CurrencyData.ReduceGoldCoin(soul);
    }

    #endregion

    #endregion

    #region 游戏保存

    public void LoadGameData(GameData data)
    {
        CurrencyData.SetCurrency(data.GoldCoin, data.Soul);
    }

    public void SaveGameData(GameData data)
    {
        data.GoldCoin = CurrencyData.GoldCoin;
        data.Soul = CurrencyData.Soul;
    }

    #endregion
}