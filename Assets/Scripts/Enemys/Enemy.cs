using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : Character
{
    [Header("玩家检测")]
    [SerializeField] float _checkPlayerDistance=50f;
    [SerializeField] LayerMask _playerLayer;

    [Header("闲置状态")]
    public float IdleTime;

    [Header("移动状态")]
    public float MoveSpeed;
    public float MoveTime;
    float _defaultMoveSpeed;

    [Header("危险状态")]
    public float BattleTime;

    [Header("攻击状态")]
    public float IgnoreDistance=2f;
    public float AttackLastTime;
    public Vector2 AttackCooldownOffset;
    public float AttackCooldown { get; set; }

    [Header("眩晕状态")]
    public float StunnedMult;
    [SerializeField] protected GameObject counterImage;
    protected bool canStunned;


    protected override void Awake()
    {
        base.Awake();

        _defaultMoveSpeed=MoveSpeed;
    }

    /// <summary>
    /// 是否冻结自身
    /// </summary>
    /// <returns></returns>
    public virtual void IsFreezeSelf(bool isFreeze)
    {
        if (isFreeze)
        {
            MoveSpeed = 0;
            Animator_CT.speed = 0;
        }
        else
        {
            MoveSpeed = _defaultMoveSpeed;
            Animator_CT.speed = 1;
        }
    }

    /// <summary>
    /// 控制冻结自身
    /// </summary>
    /// <param name="freezeTimer"></param>
    /// <returns></returns>

    protected virtual IEnumerator IsFreezeSelfCo(float freezeTimer)
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
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(AttackCheck.position, Vector2.right * Direction, _checkPlayerDistance, _playerLayer);
}
