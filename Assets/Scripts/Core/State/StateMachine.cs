using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EntityState CurrentState { get; private set; }


    /// <summary>
    /// 初始化状态
    /// </summary>
    /// <param name="state"></param>
    public void InitState(EntityState state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }


    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(EntityState newState)
    {
        CurrentState.Exit();
        CurrentState=newState;
        CurrentState.Enter();
    }


}
