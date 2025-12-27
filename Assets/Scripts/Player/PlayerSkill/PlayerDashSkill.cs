using UnityEngine;


public class PlayerDashSkill : Skill
{
    public bool IsLock;

    public bool IsCreateCloneDashEnd;
    public bool IsCreateCloneDashStart;

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
        if (IsLock&&CanUseSkill())
        {
            InGameUIController.Instance.DashImageCooldown();
            player.CharacterStateMachine.ChangeState(player.DashState);
            player.Rb.AddForce(Vector2.right * player.Direction * player.MoveData.DashForce, ForceMode2D.Impulse);
        }
    }

    #region 技能解锁

    /// <summary>
    /// 解锁冲刺
    /// </summary>
    public void UnLockDash()=>IsLock = true;

    /// <summary>
    /// 解锁冲刺开始时创造克隆体
    /// </summary>
    public void UnLockDashStartCreateClone()=> IsCreateCloneDashStart = true;

    /// <summary>
    /// 解锁冲刺结束后创造克隆体
    /// </summary>
    public void UnLockDashEndCreateClone()=> IsCreateCloneDashEnd = true;

    #endregion
}




