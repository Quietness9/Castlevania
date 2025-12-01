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

    private void AttackAnimationFinish()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemy.AttackCheck.position, _enemy.AttackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            
            if (hit.TryGetComponent(out Player player))
            {
                player.Damage(_enemy);

                //EnemyStat _target = hit.GetComponent<EnemyStat>();
                //if (_target != null)
                //{
                //    player.stats.DoDamage(_target);
                //}

                //ItemDateEquipment weaponData = Inventory.instance.GetUseEquipment(EquipmentType.Weapon);
                //if (weaponData != null)
                //{
                //    weaponData.Effect(_target.transform);
                //}
            }
        }
    }

    private void OpenCounterWindow()=>_enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow()=>_enemy.CloseCounterAttackWindow();
}
