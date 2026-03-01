using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrinerAnimationTrigger : EnemyAnimationTrigger
{
    /// <summary>
    /// 设置无敌
    /// </summary>
    private void MakeTransparent()
    {
        enemy.Attribute.SetInvincible(true);
    }

    /// <summary>
    /// 取消无敌
    /// </summary>
    private void MakeUnTransparent()
    {
        enemy.Attribute.SetInvincible(false);
    }


}
