using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityState
{
    protected float timer;
    protected bool triggerFinish;


    protected Character baseCharacter;
    protected StateMachine baseStateMachine;
    protected string baseAnimationName;

    public EntityState(Character character,StateMachine stateMachine,string animationName)
    {
        baseCharacter = character;
        baseStateMachine = stateMachine;
        baseAnimationName = animationName;
    }

    
    public virtual void Enter()
    {
        baseCharacter.Animator_CT.SetBool(baseAnimationName, true);
    }

    public virtual void Update()
    {
        timer += Time.deltaTime;
    }

    public virtual void Exit()
    {
        baseCharacter.Animator_CT.SetBool(baseAnimationName, false);
    }

}
