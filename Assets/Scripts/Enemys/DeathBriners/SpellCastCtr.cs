using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpellCastCtr : MonoBehaviour
{
    [SerializeField] Transform _checkTransform;
    [SerializeField] Vector2 _spellCastCheckSize;
    [SerializeField] LayerMask _playerLayer;

    DeathBriner _deathBriner;

    /// <summary>
    /// 设置鬼手数据
    /// </summary>
    /// <param name="deathBriner"></param>
    public void SetSpellCastData(DeathBriner deathBriner)
    {
        _deathBriner = deathBriner;
    }

    /// <summary>
    /// 鬼手伤害
    /// </summary>
    private void SpellCastAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_checkTransform.position,
            _spellCastCheckSize, _playerLayer);

        foreach (Collider2D hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                player.Attribute.TakePhysicalDamage(_deathBriner);
                player.DamageEffect(_deathBriner);
            }
        }
    }

    /// <summary>
    /// 销毁自身
    /// </summary>
    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_checkTransform.position, _spellCastCheckSize);
    }
}
