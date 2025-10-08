using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : Character
{
    [Header("玩家检测")]
    [SerializeField] float _checkPlayerDistance=50f;
    [SerializeField] LayerMask _playerLayer;

    [Header("IdleState")]
    public float IdleTime;

    [Header("MoveState")]
    public float MoveSpeed;
    public float MoveTime;

    [Header("BattleState")]
    public float BattleTime;

    [Header("AttackState")]
    public float IgnoreDistance=2f;
    public float AttackLastTime;
    public Vector2 AttackCooldownOffset;
    public float AttackCooldown { get; set; }

    [Header("StunnedState")]
    public float StunnedMult;
    [SerializeField] protected GameObject counterImage;
    protected bool canStunned;

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
