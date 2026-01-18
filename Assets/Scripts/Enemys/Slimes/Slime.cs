using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [Header("史莱姆分裂")]
    [SerializeField] SlimeType _slimeType;
    [SerializeField] int _slimeCreateAmount;
    [SerializeField] Vector2 _xCreateVelocity;
    [SerializeField] Vector2 _yCreateVelocity;


    public BoxCollider2D Bd2d { get; private set; }

    #region 状态
    public SlimeStunnedState StunnedState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        StunnedState = new SlimeStunnedState(this, this, CharacterStateMachine, "Stunned");

        Bd2d = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        InitSlime();
    }

    private void Update()
    {
        CharacterStateMachine.CurrentState.Update();
    }

    #region 创建史莱姆状态

    protected override EnemyIdleState CreateIdleState() => new SlimeIdleState(this, this, CharacterStateMachine, "Idle");
    protected override EnemyMoveState CreateMoveState() => new SlimeMoveState(this, this, CharacterStateMachine, "Move");
    protected override EnemyAttackState CreateAttackState() => new SlimeAttackState(this, this, CharacterStateMachine, "Attack");
    protected override EnemyBattleState CreateBattleState() => new SlimeBattleState(this, this, CharacterStateMachine, "Move");
    protected override EnemyDeathState CreateDeathState() => new SlimeDeathState(this, this, CharacterStateMachine, "Death");


    #endregion

    /// <summary>
    /// 判断是否可以切换Stunned状态
    /// </summary>
    /// <returns></returns>
    public override bool IsSetStunnedState()
    {
        if (base.IsSetStunnedState())
        {
            CharacterStateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 设置史莱姆
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="slimeType"></param>
    /// <param name="scale"></param>
    public void SetSlime(float direction,SlimeType slimeType,Vector3 scale)
    {
        _slimeType = slimeType;
        transform.localScale = scale;

        if (direction != Direction)
            TurnDirection();

        float xVelocity = Random.Range(_xCreateVelocity.x, _xCreateVelocity.y);
        float yVelocity=Random.Range(_yCreateVelocity.x,_yCreateVelocity.y);

        SetVelocity(-Direction*xVelocity, yVelocity);
    }

    /// <summary>
    /// 初始化史莱姆
    /// </summary>
    private void InitSlime()
    {
        IsFacingRight = false;
        CharacterStateMachine.InitState(IdleState);
    }


    protected override void ChangeDieStateHandle()
    {
        base.ChangeDieStateHandle();

        if (_slimeType!=SlimeType.Small)
        {
            CreateSlime(_slimeCreateAmount);
        }
    }

    /// <summary>
    /// 创建史莱姆
    /// </summary>
    /// <param name="amount"></param>
    private void CreateSlime(int amount)
    {
        SlimeType slimeType = _slimeType;
        Vector3 scale=Vector3.one;

        GameObject slimePre = GlobalReferencesMgr.Instance.GetPrefab("Slime");

        if (slimePre == null)
            return;

        for (int i = 0; i < amount; i++)
        {
            GameObject slimeObj=Instantiate(slimePre,transform.position,Quaternion.identity);

            if (_slimeType == SlimeType.Big)
            {
                slimeType = SlimeType.Medium;                
            }

            if (_slimeType == SlimeType.Medium)
            {
                slimeType= SlimeType.Small;
                scale = new Vector3(0.8f, 0.8f, 1);
            }

            slimeObj.GetComponent<Slime>().SetSlime(Direction, slimeType,scale);

        }
    }

}
