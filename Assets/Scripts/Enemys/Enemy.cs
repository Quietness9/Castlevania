using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : Character
{
    public EnemyStateData EnemyStateData;

    //攻击
    public float AttackLastTime { get; set; }
    public float AttackCooldown { get; set; }

    [SerializeField] protected GameObject counterImage;
    protected bool canStunned;

    protected float defaultMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        defaultMoveSpeed = EnemyStateData.MoveSpeed;
    }

    /// <summary>
    /// 掉落货币
    /// </summary>
    protected virtual void DropCurrency(int coldCoin,int soul)
    {
        if(GlobalReferencesManager.Instance != null)
        {
            GlobalReferencesManager.Instance.GamePlayer.CurrencyData.IncreaseGoldCoin(coldCoin);
            GlobalReferencesManager.Instance.GamePlayer.CurrencyData.IncreaseSoul(soul);
        }
    }

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

    #region 反击窗口

    /// <summary>
    /// 开启反击窗口
    /// </summary>
    public virtual void OpenCounterAttackWindow()
    {
        canStunned=true;
        counterImage.SetActive(true);
    }

    /// <summary>
    /// 关闭反击窗口
    /// </summary>
    public virtual void CloseCounterAttackWindow()
    {
        canStunned = false;
        counterImage.SetActive(false);
    }

    #endregion

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
    /// 检测玩家
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(AttackCheck.position, Vector2.right * Direction, EnemyStateData.CheckPlayerDistance, EnemyStateData.PlayerLayer);
}
