using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAnimationTrigger : MonoBehaviour
{
    Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    /// <summary>
    /// 动画完成回调
    /// </summary>
    private void AnimationFinish()
    {
        _player.CurrentAnimationFinish();
    }

    /// <summary>
    /// 攻击动画完成回调
    /// </summary>
    private void AttackAnimationFinish()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.AttackCheck.position, _player.AttackCheckRadius);

        foreach(Collider2D hit in colliders)
        {
            
            if (hit.TryGetComponent(out Enemy enemy))
            {

                enemy.Attribute.TakePhysicalDamage(_player);
                enemy.DamageEffect(_player);

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

        _player.CurrentAnimationFinish();
    }

    private void ThrowSword()
    {
        SkillManager.Instance.SwordSkill.CreateSword();
    }

}
