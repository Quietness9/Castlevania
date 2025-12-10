using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAnimationTrigger : MonoBehaviour
{
    Enemy _enemy;

    private void Awake()
    {
        _enemy= GetComponentInParent<Enemy>();
    }

    /// <summary>
    /// 动画完成回调
    /// </summary>
    private void AnimationFinish()
    {
        _enemy.CurrentAnimationFinish();
    }

    /// <summary>
    /// 攻击动画造成伤害
    /// </summary>
    private void AttackAnimationFinish()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemy.AttackCheck.position, _enemy.AttackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            
            if (hit.TryGetComponent(out Player player))
            {
                player.Attribute.TakePhysicalDamage(_enemy);
                player.DamageEffect(_enemy);

                //EnemyStat _target = hit.GetComponent<EnemyStat>();
                //if (_target != null)
                //{
                //    player.stats.TakePhysicalDamage(_target);
                //}

                //ItemDateEquipment weaponData = InventoryController.instance.GetUseEquipment(EquipmentType.Weapon);
                //if (weaponData != null)
                //{
                //    weaponData.Effect(_target.transform);
                //}
            }
        }
    }

    /// <summary>
    /// 死亡时销毁
    /// </summary>
    private void DestroySelf() => Destroy(_enemy.gameObject);

    /// <summary>
    /// 打开反击窗口
    /// </summary>
    private void OpenCounterWindow()=>_enemy.OpenCounterAttackWindow();

    /// <summary>
    /// 关闭反击窗口
    /// </summary>
    private void CloseCounterWindow()=>_enemy.CloseCounterAttackWindow();
}
