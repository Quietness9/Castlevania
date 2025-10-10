using UnityEngine;


public class PlayerDashSkill : Skill
{
    public float DashForce;


    protected override void Start()
    {
        base.Start();
        player.PlayerInput.DashEvent += DashHandle;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        player.PlayerInput.DashEvent -= DashHandle;
    }


    private void DashHandle()
    {
        if (CanUseSkill())
        {
            player.Rb.AddForce(Vector2.right * player.Direction * DashForce, ForceMode2D.Impulse);
            player.CharacterStateMachine.ChangeState(player.DashState);
        }
    }
}




