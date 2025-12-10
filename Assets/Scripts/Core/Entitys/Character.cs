using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("攻击检测")]
    public Transform AttackCheck;
    public float AttackCheckRadius;

    [Header("地面检测")]
    [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] protected LayerMask groundLayer;

    [Header("墙面检测")]
    [SerializeField] protected Transform wallCheckPoint;
    [SerializeField] protected float wallCheckSize = 1f;
    [SerializeField] protected LayerMask wallLayer;

    [Header("击退")]
    public Vector2 KnockbackForce;
    public float KnockDuration;
    protected bool isKnock;

    public float Direction = 1f;

    public bool IsFacingRight { get; set; }
    public StateMachine CharacterStateMachine { get; private set; }

    #region 组件

    public Animator Animator_CT { get;private set; }
    public Rigidbody2D Rb { get;private set; }
    public CharacterFX Fx { get;private set; }
    public CharacterAttribute Attribute { get; private set; }

    SpriteRenderer _sr;

    #endregion

    public event Action FlipEvent = delegate { };

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Fx = GetComponentInChildren<CharacterFX>();
        Attribute = GetComponent<CharacterAttribute>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        Animator_CT = GetComponentInChildren<Animator>();

        CharacterStateMachine = new StateMachine();
    }

    #region 攻击特殊效果

    /// <summary>
    /// 造成伤害的击退特性
    /// </summary>
    /// <param name="character"></param>
    public virtual void DamageEffect(Character character)
    {
        StartCoroutine(HitKnockbackCo(character.KnockbackForce, character.KnockDuration, character.Direction));
    }


    /// <summary>
    /// 击退效果
    /// </summary>
    /// <param name="hitForce"></param>
    /// <param name="hitDuration"></param>
    /// <param name="direction"></param>
    /// <param name="mult"></param>
    /// <returns></returns>
    private IEnumerator HitKnockbackCo(Vector2 hitForce, float hitDuration, float direction, float mult = 1)
    {
        isKnock = true;

        Rb.AddForce(new Vector2(hitForce.x * direction * mult, hitForce.y * mult), ForceMode2D.Impulse);

        yield return new WaitForSeconds(hitDuration);

        isKnock = false;
    }

    /// <summary>
    ///当你受到冷冻时减慢角色
    /// </summary>
    public virtual void SlowCharacterSpeed(float slowRatio) 
    {
        Animator_CT.speed *= (1 - slowRatio);
    }
    
    /// <summary>
    /// 恢复到原来的角色的速度
    /// </summary>
    public virtual void ReturnCharacterDefaultSpeed()
    {
        Animator_CT.speed = 1;
    }

    #endregion

    /// <summary>
    /// 完成动画播放检测
    /// </summary>
    public void CurrentAnimationFinish() => CharacterStateMachine.CurrentState.AnimationFinishTrigger();

    /// <summary>
    /// 改变朝向
    /// </summary>
    public virtual void TurnDirection()
    {
        if (isKnock)
            return;

        transform.Rotate(0, 180, 0);

        Direction *= -1;
        IsFacingRight = !IsFacingRight;

        FlipEvent.Invoke();
    }

    #region 通用射线碰撞检测

    /// <summary>
    /// 地面检测默认返回true
    /// </summary>
    /// <returns></returns>
    public bool IsGroundCheck() => Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);

    /// <summary>
    /// 墙检测
    /// </summary>
    /// <returns></returns>
    public bool IsWallCheck() => Physics2D.Raycast(wallCheckPoint.position, (Vector2.right * Direction).normalized, wallCheckSize, wallLayer);

    /// <summary>
    /// 停止角色
    /// </summary>
    public void SetVelocityZero() => Rb.velocity = Vector3.zero;

    /// <summary>
    /// 绘制检测线
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        Gizmos.DrawWireSphere(AttackCheck.position, AttackCheckRadius);
        Gizmos.DrawLine(wallCheckPoint.position, new Vector3(wallCheckPoint.position.x + wallCheckSize * Direction, wallCheckPoint.position.y));
    }

    #endregion
}
