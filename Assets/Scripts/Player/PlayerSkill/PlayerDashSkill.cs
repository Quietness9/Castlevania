using UnityEngine;


public class PlayerDashSkill : Skill
{
    public float DashForce;

    protected override void Start()
    {
        base.Start();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.DashEvent += DashSkillHandle;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.DashEvent -= DashSkillHandle;
        }
        
    }

    /// <summary>
    /// 冲刺技能回调
    /// </summary>
    private void DashSkillHandle()
    {
        if (CanUseSkill())
        {
            player.CharacterStateMachine.ChangeState(player.DashState);
            player.Rb.AddForce(Vector2.right * player.Direction * DashForce, ForceMode2D.Impulse);
        }
    }
}




