using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBriner : Enemy
{
    public BoxCollider2D Bd2d { get; private set; }
    public DeathBrinerData Data;
    [SerializeField] BoxCollider2D _moveArea;

    #region 状态

    public DeathBrinerSpellCastState SpellCastState { get; private set; }

    public DeathBrinerTeleportState TeleportState  { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        Bd2d = GetComponent<BoxCollider2D>();

        SpellCastState=new DeathBrinerSpellCastState(this, this, CharacterStateMachine, "SpellCast");
        TeleportState = new DeathBrinerTeleportState(this, this, CharacterStateMachine, "Teleport");
    }


    protected override void Start()
    {
        base.Start();
        InitDeathBriner();
    }

    private void Update()
    {
        CharacterStateMachine.CurrentState.Update();
    }

    /// <summary>
    /// 初始化死亡布林
    /// </summary>
    private void InitDeathBriner()
    {
        IsFacingRight = false;
        CharacterStateMachine.InitState(IdleState);
    }

    #region 死亡布林创建状态

    protected override EnemyAttackState CreateAttackState() => new DeathBrinerAttackState(this, this, CharacterStateMachine, "Attack");
    protected override EnemyBattleState CreateBattleState() => new DeathBrinerBattleState(this, this, CharacterStateMachine, "Move");
    protected override EnemyDeathState CreateDeathState() => new DeathBrinerDeathState(this, this, CharacterStateMachine, "Death");
    protected override EnemyIdleState CreateIdleState() => new DeathBrinerIdleState(this, this, CharacterStateMachine, "Idle");
    protected override EnemyMoveState CreateMoveState() => new DeathBrinerMoveState(this, this, CharacterStateMachine, "Move");


    #endregion


    /// <summary>
    /// 是否能够使用鬼手
    /// </summary>
    /// <returns></returns>
    public bool IsCanSpellCast()
    {
        if(Time.time > Data.LastTimeCast + Data.SpellCastCooldown)
        {
            Data.LastTimeCast = Time.time;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 寻找可以闪现的位置
    /// </summary>
    public void FindSwapPosition()
    {
        float x = Random.Range(_moveArea.bounds.min.x + 3, _moveArea.bounds.max.x - 3);
        float y = Random.Range(_moveArea.bounds.min.y + 3, _moveArea.bounds.max.y - 3);

        transform.position= new Vector3(x, y, transform.position.z);
        transform.position=new Vector3(transform.position.x, transform.position.y-GroundBelow().distance+(Bd2d.size.y/2), transform.position.z);

        if (!GroundBelow() || SomethingIsAround())
        {
            FindSwapPosition();
        }
    }

    /// <summary>
    /// 向下检测地面
    /// </summary>
    /// <returns></returns>
    private RaycastHit2D GroundBelow()=> Physics2D.Raycast(transform.position, Vector2.down, 100f, groundLayer);

    /// <summary>
    /// 检查周围是否有物体
    /// </summary>
    /// <returns></returns>
    private bool SomethingIsAround()=>Physics2D.BoxCast(transform.position, Data.SurroundCheck, 0f, Vector2.zero, 0f, groundLayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y-GroundBelow().distance, transform.position.z));
        Gizmos.DrawWireCube(transform.position,Data.SurroundCheck);
    }
}
