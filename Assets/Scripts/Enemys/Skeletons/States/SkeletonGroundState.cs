using UnityEngine;

public class SkeletonGroundState : EntityState
{
    protected Skeleton skeleton;

    public SkeletonGroundState(Character character, Skeleton skeleton, StateMachine stateMachine, string animationName) : base(character, stateMachine, animationName)
    {
        this.skeleton = skeleton;
    }

    public override void Update()
    {
        base.Update();

        if (skeleton.IsPlayerDetected() && Vector2.Distance(skeleton.transform.position, GlobalReferencesManager.Instance.GamePlayer.transform.position) < skeleton.EnemyStateData.IgnoreDistance)
        {
            baseStateMachine.ChangeState(skeleton.BattleState);
        }
    }
}
