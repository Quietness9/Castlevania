using GameInputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerInputReader PlayerInput;

    #region ״状态

    public PlayerIdle IdleState { get;private set; }

    public PlayerMove MoveState { get;private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new PlayerIdle(this, CharacterStateMachine, "Idle");
        MoveState = new PlayerMove(this, CharacterStateMachine, "Move");
    }

    private void Start()
    {
        CharacterStateMachine.InitState(IdleState);
    }

    private void Update()
    {
        CharacterStateMachine.CurrentState.Update();
    }
}
