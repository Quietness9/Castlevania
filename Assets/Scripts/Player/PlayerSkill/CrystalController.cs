using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    Player _player;
    Animator _animator;
    CrystalData _crystalData;
    Transform _closestEnemy;
    CircleCollider2D _circleCollider2D;

    bool _isGrow;
    bool _isCanMove;
    float _crystalDurationTimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _circleCollider2D=GetComponent<CircleCollider2D>();
    }

    public void SetCrystalData(CrystalData crystalData,Player player)
    {
        _player=player;
        _crystalData=crystalData;
        _crystalDurationTimer = crystalData.CrystalDurationTime;
    }

    private void Update()
    {
        _crystalDurationTimer-= Time.deltaTime;
        if( _crystalDurationTimer<=0)
        {
            DestroySelf();
        }

        if (_isGrow)
        {
            transform.localScale=Vector2.Lerp(transform.localScale,_crystalData.GrowScale, _crystalData.GrowSpeed*Time.deltaTime);
        }

        if (_isCanMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _closestEnemy.position, _crystalData.CrystalMoveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _closestEnemy.position) <1.5)
            {
                CrystalExplosion();
            }
        }
    }

    /// <summary>
    /// 水晶爆炸
    /// </summary>
    public void CrystalExplosion()
    {
        _isGrow=true;
        _animator.SetTrigger("Explode");
    }

    /// <summary>
    /// 交换水晶和玩家位置
    /// </summary>
    public void PlayerSwapCrystalPosition()
    {
        Vector3 position = _player.transform.position;
        PlayerReturnCrystalPosition();
        transform.position=position;
    }


    /// <summary>
    /// 玩家返回到水晶位置
    /// </summary>
    public void PlayerReturnCrystalPosition()
    {
        _player.transform.position = transform.position;
    }

    /// <summary>
    /// 水晶向最近的敌人移动
    /// </summary>
    public void CrystalMoveToEnemy(Transform EnemyTransform)
    {
        _closestEnemy=EnemyTransform;
        _isCanMove = true;
    }

    /// <summary>
    /// 销毁自身
    /// </summary>
    public void DestroySelf()=>Destroy(gameObject);

    /// <summary>
    /// 爆炸造成伤害
    /// </summary>
    private void CrystalExplosionDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleCollider2D.radius);

        foreach (Collider2D hit in colliders)
        {
            if(hit.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage(_player);
            }
        }
    }
}
