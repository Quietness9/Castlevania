using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : EntityState
{
    protected Player player;

    public PlayerState(Character character, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        player = character as Player;
    }

    
}
