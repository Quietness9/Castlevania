using UnityEngine;


public class PlayerDashSkill : Skill
{

    protected override void Start()
    {
        base.Start();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.OnDashEvent += UseSkill;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.OnDashEvent -= UseSkill;
        }
        
    }

    public override void UseSkill()
    {
        if (CanUseSkill())
        {
            player.CharacterStateMachine.ChangeState(player.DashState);
            player.Rb.AddForce(Vector2.right * player.Direction * player.MoveData.DashForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("ººƒ‹¿‰»¥÷–");
        }
    }
}




