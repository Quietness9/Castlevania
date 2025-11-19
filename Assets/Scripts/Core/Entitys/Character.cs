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
    [SerializeField] public Vector2 KnockbackForce;
    [SerializeField] public float KnockDuration;
    protected bool isKnock;

    public float Direction = 1f;

    public bool IsFacingRight { get; set; }
    public StateMachine CharacterStateMachine { get; private set; }

    #region 组件
    public Animator Animator_CT { get;private set; }
    public Rigidbody2D Rb { get;private set; }
    public EntityFX Fx { get;private set; }

    #endregion

    protected virtual void Awake()
    {
        Fx = GetComponentInChildren<EntityFX>();
        Animator_CT = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        CharacterStateMachine = new StateMachine();
    }


    /// <summary>
    /// 造成伤害
    /// </summary>
    public virtual void Damage(Character character)
    {
        Fx.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockback(character.KnockbackForce, character.KnockDuration, character.Direction));
    }

    /// <summary>
    /// 击退效果
    /// </summary>
    /// <param name="xForce"></param>
    /// <param name="yForce"></param>
    /// <returns></returns>
    private IEnumerator HitKnockback(Vector2 hitForce, float hitDuration, float direction, float mult = 1)
    {
        isKnock = true;

        Rb.AddForce(new Vector2(hitForce.x * direction * mult, hitForce.y * mult), ForceMode2D.Impulse);

        yield return new WaitForSeconds(hitDuration);

        isKnock = false;
    }

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
    }

    #region 射线碰撞检测

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
