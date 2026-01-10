using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EntityState
{
    protected Slime slime;

    public SlimeGroundState(Character character,Slime slime, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        this.slime = slime;
    }

    public override void Update()
    {
        base.Update();
        if (slime.IsPlayerDetected() && Vector2.Distance(slime.transform.position, GlobalReferencesManager.Instance.GamePlayer.transform.position) < slime.EnemyStateData.IgnoreDistance)
        {
            baseStateMachine.ChangeState(slime.BattleState);
        }
    }
}
