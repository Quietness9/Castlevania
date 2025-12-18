using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FreezeEnemy Effect", menuName = "GameEffect/EquipmentEffect/FreezeEnemy")]
public class FreezeEnemyEffect : EquipmentEffect
{

    [SerializeField] float _duration;
    [SerializeField] float _frozenRange;


    public override void ReleaseEffects(Transform transform)
    {
        Collider2D[] colliders=Physics2D.OverlapCircleAll(transform.position, _frozenRange);

        foreach (Collider2D collider in colliders)
        {
            if(collider.TryGetComponent(out Enemy enemy))
            {
                enemy.FreezeTimerForSelf(_duration);
            }
        }
    }
}
