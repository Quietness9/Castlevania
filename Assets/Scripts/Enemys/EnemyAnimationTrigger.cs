using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAnimationTrigger : MonoBehaviour
{
    protected Enemy enemy;

    private void Awake()
    {
        enemy= GetComponentInParent<Enemy>();
    }

    /// <summary>
    /// 动画完成回调
    /// </summary>
    private void AnimationFinish()
    {
        enemy.CurrentAnimationFinish();
    }

    /// <summary>
    /// 攻击动画造成伤害
    /// </summary>
    private void AttackAnimationFinish()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.AttackCheck.position, enemy.AttackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            
            if (hit.TryGetComponent(out Player player))
            {
                player.Attribute.TakePhysicalDamage(enemy);
                player.DamageEffect(enemy);
            }
        }
    }

    /// <summary>
    /// 死亡时销毁
    /// </summary>
    private void DestroySelf() => Destroy(enemy.gameObject);

    /// <summary>
    /// 打开反击窗口
    /// </summary>
    private void OpenCounterWindow()=>enemy.OpenCounterAttackWindow();

    /// <summary>
    /// 关闭反击窗口
    /// </summary>
    private void CloseCounterWindow()=>enemy.CloseCounterAttackWindow();
}
