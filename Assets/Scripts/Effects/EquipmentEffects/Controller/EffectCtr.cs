using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtr : MonoBehaviour
{
    protected Player player => GlobalReferencesMgr.Instance.GamePlayer;


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null || collision == null)
            return;

        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.Attribute.TakeMagicDamage(player);
        }
    }
}
