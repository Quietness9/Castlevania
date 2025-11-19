using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] BlackHoleData _blackHoleData;

    [SerializeField] bool isGrow;
    List<Enemy> _enemyTarget = new();

    private void Update()
    {
        if (isGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_blackHoleData.MaxSize, _blackHoleData.MaxSize)
                , _blackHoleData.GrowSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.StartCoroutine("IsFreezeSelfCo", _blackHoleData.BlackHoleFreezeTime);
        }
    }
}
