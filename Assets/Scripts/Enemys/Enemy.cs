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

    [Header("AttackInfo")]
    public float IgnoreDistance=2f;
    public float AttackLastTime;
    public Vector2 AttackCooldownOffset;
    public float AttackCooldown { get; set; }

    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="xVelocity"></param>
    /// <param name="yVelocity"></param>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
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
