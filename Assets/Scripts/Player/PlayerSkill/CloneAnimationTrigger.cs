using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CloneAnimationTrigger : MonoBehaviour
{

    
    [SerializeField] float _attackCheckRadius;

    Transform _attackCheck;
    SpriteRenderer _spriteRenderer;
    bool _isDisappear;
    float _cloneTimer;
    float _colorDisappearSpeed;

    private void Start()
    {
        _attackCheck=GetComponentInParent<Transform>();
         _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {

        if (!_isDisappear)
            return;

        _cloneTimer -= Time.deltaTime;

        if (_cloneTimer > 0.01f)
        {
            _spriteRenderer.color = new Color(1, 1, 1, _spriteRenderer.color.a - (Time.deltaTime * _colorDisappearSpeed));
        }

        if (_cloneTimer < 0)
        {
            _isDisappear = false;
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// 设置克隆体参数
    /// </summary>
    /// <param name="cloneDuration"></param>
    /// <param name="isDisappear"></param>
    public void SetPlayerClone(float cloneDuration,float colorDisappearSpeed, bool isDisappear=true)
    {
        _cloneTimer = cloneDuration;
        _colorDisappearSpeed = colorDisappearSpeed;
        _isDisappear = isDisappear;
    }

    /// <summary>
    /// 攻击动画完成回调
    /// </summary>
    private void AttackAnimationFinish()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheck.position, _attackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            
            if (hit.TryGetComponent(out Enemy enemy))
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
