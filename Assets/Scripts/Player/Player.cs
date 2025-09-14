using GameInputSystem;
using System;
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

    //跳跃
    public bool IsJumpCut { get;set; }
    public bool IsJumpFalling { get;set; }
    public bool IsJumping { get;set; }

    //Timer
    public float LastOnGroundTime { get;set; }
    public float LastPressedJumpTime { get;set; }


    #region 状态

    public PlayerIdle IdleState { get; private set; }

    public PlayerMove MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new PlayerIdle(this, CharacterStateMachine, "Idle");
        MoveState = new PlayerMove(this, CharacterStateMachine, "Move");
        JumpState = new PlayerJumpState(this, CharacterStateMachine, "Jump");
    }

    private void Start()
    {
        InitPlayer();
    }

    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;      
        LastPressedJumpTime -= Time.deltaTime;

        if (!IsJumping)
        {
            if (base.IsGroundCheck())
            {
                LastOnGroundTime = MoveData.coyoteTime;
            }
        } 


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