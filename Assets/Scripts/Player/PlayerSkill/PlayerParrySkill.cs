using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParrySkill : Skill
{
    public ParryData PlayerParryData;

    public bool IsLock;

    public bool IsRecoverHp;
    public bool IsCreateClone;

    protected override void Start()
    {
        base.Start();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.OnCounterAttackEvent += UseSkill;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.OnCounterAttackEvent -= UseSkill;
        }

    }


    public override void UseSkill()
    {
        if (IsLock&&CanUseSkill())
        {
            InGameUIController.Instance.ParryImageCooldown();
            player.CharacterStateMachine.ChangeState(player.CounterAttackState);
        }
    }

    /// <summary>
    /// 恢复生命
    /// </summary>
    public void RecoverHp()
    {
        if (IsRecoverHp)
        {
            int recoverHp = Mathf.RoundToInt(player.Attribute.Hp.GetValue() * PlayerParryData.RecoverHpRatio);
            player.Attribute.RecoverCurrentHealth(recoverHp);
        }
        
    }

    #region 技能解锁

    /// <summary>
    /// 解锁反击技能
    /// </summary>
    public void UnLockParry() => IsLock = true;

    /// <summary>
    /// 解锁反击回血
    /// </summary>
    public void UnLockRecoverHp()=>IsRecoverHp = true;

    /// <summary>
    /// 解锁反击生成克隆体
    /// </summary>
    public void UnLockCreateClone()=>IsCreateClone = true;

    #endregion
}
