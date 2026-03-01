using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStateData EnemyStateData;
    public Transform PlayerCheck;

    [SerializeField] protected GameObject CounterImage;
    protected bool canStunned;

    protected float defaultMoveSpeed;

    //攻击
    float _attackLastTime;
    float _attackCooldown;

    #region 敌人通用状态

    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyBattleState BattleState { get; private set; }
    public EnemyDeathState DeathState { get; private set; }


    #endregion


    protected override void Awake()
    {
        base.Awake();
        IdleState = CreateIdleState();
        MoveState = CreateMoveState();
        AttackState = CreateAttackState();
        BattleState = CreateBattleState();
        DeathState = CreateDeathState();
    }

    private void OnEnable()
    {
        EventSubscribe();
    }

    protected virtual void Start()
    {
        defaultMoveSpeed = EnemyStateData.MoveSpeed;
    }

    private void OnDisable()
    {
        EventUnsubscribe();
    }

    #region 创建状态的虚函数

    protected virtual EnemyIdleState CreateIdleState() => new EnemyIdleState(this, this, CharacterStateMachine, "Idle");
    protected virtual EnemyMoveState CreateMoveState() => new EnemyMoveState(this, this, CharacterStateMachine, "Move");
    protected virtual EnemyAttackState CreateAttackState() => new EnemyAttackState(this, this, CharacterStateMachine, "Attack");
    protected virtual EnemyBattleState CreateBattleState() => new EnemyBattleState(this, this, CharacterStateMachine, "Move");
    protected virtual EnemyDeathState CreateDeathState() => new EnemyDeathState(this, this, CharacterStateMachine, "Death");

    #endregion

    #region 状态切换和事件订阅

    /// <summary>
    /// 事件订阅
    /// </summary>
    protected virtual void EventSubscribe()
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
    protected virtual void EventUnsubscribe()
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
    protected virtual void ChangeDieStateHandle()
    {
        CharacterStateMachine.ChangeState(DeathState);
        DropItemAndCurrency(coldCoin, soul);
    }

    #endregion

    #region 冰冻效果控制

    public override void SlowCharacterSpeed(float slowRatio)
    {
        base.SlowCharacterSpeed(slowRatio);
        EnemyStateData.MoveSpeed *= (1 - slowRatio);
    }

    public override void ReturnCharacterDefaultSpeed()
    {
        base.ReturnCharacterDefaultSpeed();
        EnemyStateData.MoveSpeed = defaultMoveSpeed;
    }

    /// <summary>
    /// 是否冻结自身
    /// </summary>
    /// <returns></returns>
    public virtual void IsFreezeSelf(bool isFreeze)
    {
        if (isFreeze)
        {
            EnemyStateData.MoveSpeed = 0;
            Animator_CT.speed = 0;
        }
        else
        {
            EnemyStateData.MoveSpeed = defaultMoveSpeed;
            Animator_CT.speed = 1;
        }
    }

    /// <summary>
    /// 冻结自身并在一定时间后解冻
    /// </summary>
    /// <param name="duration"></param>
    public virtual void FreezeTimerForSelf(float duration) => StartCoroutine(FreezeSelfCo(duration));

    /// <summary>
    /// 控制冻结自身
    /// </summary>
    /// <param name="freezeTimer"></param>
    /// <returns></returns>

    protected virtual IEnumerator FreezeSelfCo(float freezeTimer)
    {
        IsFreezeSelf(true);

        yield return new WaitForSeconds(freezeTimer);

        IsFreezeSelf(false);
    }

    #endregion

    #region 反击窗口控制

    /// <summary>
    /// 是可以转换反击状态
    /// </summary>
    /// <returns></returns>
    public virtual bool IsSetStunnedState()
    {
        if (canStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// 开启反击窗口
    /// </summary>
    public virtual void OpenCounterAttackWindow()
    {
        canStunned=true;
        CounterImage.SetActive(true);
    }

    /// <summary>
    /// 关闭反击窗口
    /// </summary>
    public virtual void CloseCounterAttackWindow()
    {
        canStunned = false;
        CounterImage.SetActive(false);
    }

    #endregion

    /// <summary>
    /// 掉落物品和货币
    /// </summary>
    /// <param name="coldCoin"></param>
    /// <param name="soul"></param>
    public virtual void DropItemAndCurrency(int coldCoin, int soul)
    {
        if (GlobalReferencesMgr.Instance != null)
        {
            GlobalReferencesMgr.Instance.GamePlayer.CurrencyData.IncreaseGoldCoin(coldCoin);
            GlobalReferencesMgr.Instance.GamePlayer.CurrencyData.IncreaseSoul(soul);
        }

        ItemDrop.DropItem();
    }

    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="xVelocity"></param>
    /// <param name="yVelocity"></param>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnock)
            return;

        if (xVelocity * Direction < 0)
        {
            TurnDirection();
        }
        Rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    /// <summary>
    /// 判断是否可以攻击
    /// </summary>
    /// <returns></returns>
    public bool CanAttack()
    {
        if (Time.time > _attackLastTime + _attackCooldown)
        {
            _attackCooldown = Random.Range(EnemyStateData.AttackCooldownOffset.x,
                EnemyStateData.AttackCooldownOffset.y);

            _attackLastTime = Time.time;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 检测玩家
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(PlayerCheck.position, Vector2.right * Direction, EnemyStateData.CheckPlayerDistance, EnemyStateData.PlayerLayer);

}
