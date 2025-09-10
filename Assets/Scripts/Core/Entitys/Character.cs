using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public StateMachine CharacterStateMachine { get; private set; }

    #region ×é¼þ

    public Animator Animator_CT { get;set; }
    public Rigidbody2D Rb { get;set; }

    #endregion

    protected virtual void Awake()
    {
        Animator_CT = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        CharacterStateMachine = new StateMachine();
    }
}
