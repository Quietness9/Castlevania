using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerAnimationTrigger : EnemyAnimationTrigger
{
    /// <summary>
    /// 设置无敌和透明
    /// </summary>
    private void MakeTransparent()
    {
        enemy.Fx.CharacterTransparent(true);
        enemy.Attribute.SetInvincible(true);
    }

    /// <summary>
    /// 取消无敌和透明
    /// </summary>
    private void MakeUnTransparent()
    {
        enemy.Fx.CharacterTransparent(false);
        enemy.Attribute.SetInvincible(false);
    }
}
