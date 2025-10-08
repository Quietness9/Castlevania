using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CloneAnimationTrigger : MonoBehaviour
{

    [SerializeField] Transform _attackCheck;
    [SerializeField] float _attackCheckRadius;

    /// <summary>
    /// 攻击动画完成回调
    /// </summary>
    private void AttackAnimationFinish()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheck.position, _attackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {

                enemy.Damage(GlobalReferencesManager.Instance.GamePlayer);
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
}
