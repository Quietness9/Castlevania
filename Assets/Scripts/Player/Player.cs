using GameInputSystem;
using UnityEngine;

public class Player : Character
{
    [Header("可配置数据")]
    public PlayerInputReader PlayerInput;
    public PlayerMoveData MoveData;

    [Header("移动")]
    [SerializeField] float _direction;
    public float Hor { get;private set; }
    public float Vert { get;private set; }

    public float LastOnGroundTime { get; private set; }

    #region ״状态

    public PlayerIdle IdleState { get; private set; }

    public PlayerMove MoveState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new PlayerIdle(this, CharacterStateMachine, "Idle");
        MoveState = new PlayerMove(this, CharacterStateMachine, "Move");
    }

    private void Start()
    {
        InitPlayer();
    }

    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        CharacterStateMachine.CurrentState.Update();
    }

    private void OnDestroy()
    {
        PlayerInput.MoveEvent -= GetDirectionHandle;
    }

    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        IsFacingRight = true;
        _direction = 1;

        CharacterStateMachine.InitState(IdleState);
        PlayerInput.MoveEvent += GetDirectionHandle;
    }

    /// <summary>
    /// 重写玩家地面检测
    /// </summary>
    /// <returns></returns>
    public override bool IsGroundCheck()
    {
        if (base.IsGroundCheck())
        {
            LastOnGroundTime = 0.1f;
            return true;
        }

        return false;
    }

    #region EventHandle

    /// <summary>
    /// 改变方向订阅
    /// </summary>
    /// <param name="direction"></param>
    public void GetDirectionHandle(Vector2 moveDire)
    {
        

        Hor = moveDire.x;
        Vert = moveDire.y;

        if (Hor*_direction<0)
        {
            _direction *= -1;
            TurnDirection();
        }
    }

    #endregion
}