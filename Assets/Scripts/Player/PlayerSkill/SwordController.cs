using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{

    Player _player;
    Animator _animator;
    Rigidbody2D _rb;
    CircleCollider2D _circleCollider;
    PlayerSwordSkill _swordSkill;


    [Header("弹跳剑")]
    int _bounceAmount;
    int _enemyIndex;
    List<Transform> _enemyTarget = new(); //设为私有后unity不在自动创建其空间

    [Header("穿透剑")]
    int _pierceAmount;

    [Header("旋转剑")]
    float _spinTimer;
    float _hitTimer;
    float _spinDirection;
    bool _isStopSpin;

    float _swordMoveTimer;
    bool _canRotation = true;
    bool _isSwordReturning;
    bool _isAdvanceReturn;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _swordMoveTimer -= Time.deltaTime;

        if (_canRotation)
        {
            transform.right = _rb.velocity;
        }

        if (_isSwordReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                _swordSkill.PlayerSwordData.ReturnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _player.transform.position) < 1)
            {
                _player.CatchSword();
            }
        }

        if (_swordMoveTimer <= 0)
        {
            Destroy(this.gameObject);
        }

        BounceLogic();
        SpinLogic();

    }

    /// <summary>
    /// 设置剑的重力和作用力
    /// </summary>
    /// <param name="force"></param>
    /// <param name="gravity"></param>
    public void SetSwordData(Vector2 force,Player player,PlayerSwordSkill swordSkill)
    {
        _player= player;
        _swordSkill= swordSkill;

        _bounceAmount = swordSkill.PlayerSwordData.BounceAmount;
        _pierceAmount = swordSkill.PlayerSwordData.PierceAmount;

        _rb.gravityScale = swordSkill.PlayerSwordData.getSwordGravity(swordSkill.SwordType);
        _rb.AddForce(force, ForceMode2D.Impulse);

        _swordMoveTimer = swordSkill.PlayerSwordData.MaxMoveTime;
        _spinDirection = Mathf.Clamp(_rb.velocity.x, -1, 1);


        if (swordSkill.SwordType != SwordType.Pierce)
        {
            _animator.SetBool("Flip", true);
        }

        _enemyTarget.Clear();
        _enemyIndex = 0;
        
    }

    /// <summary>
    /// 返回剑
    /// </summary>
    public void ReturnSword()
    {
        //_rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        _isSwordReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isSwordReturning)
            return;

        SwordTypeManager(collision);

    }

    /// <summary>
    /// 对不同类型剑的处理
    /// </summary>
    private void SwordTypeManager(Collider2D collision)
    {

        _isAdvanceReturn = false;

        switch (_swordSkill.SwordType)
        {
            case SwordType.Bounce: SwordBounceTriggerEnter(collision); break;
            case SwordType.Pierce: SwordPierceTriggerEnter(collision); break;
            case SwordType.Spin: SwordSpinTriggerEnter(collision); break;
            case SwordType.Ordinary: break;
            default: Debug.Log("没有此类型的剑"); break;

        }

        if (collision.TryGetComponent(out Enemy enemy))
        {
            SwordSkillDamage(enemy);
        }

        if (_isAdvanceReturn)
            return;

        HitObject(collision);

        transform.parent = collision.transform;
        _animator.SetBool("Flip", false);
    }

    /// <summary>
    /// 剑造成的伤害和效果
    /// </summary>
    /// <param name="collision"></param>
    private void SwordSkillDamage(Enemy enemy)
    {
        if(enemy == null)
        {
            Debug.LogWarning("enemy is null");
            return;
        }

        //enemy.StartCoroutine("FreezeSelfCo", _swordSkill.PlayerSwordData.FreezeTime);

        if (_swordSkill.IsLockTimeStop)
        {
            enemy.FreezeTimerForSelf(_swordSkill.PlayerSwordData.FreezeTime);
        }

        float swordRation = _swordSkill.PlayerSwordData.SwordAtkRation;

        if (_swordSkill.IsLockSwordPower)
        {
            swordRation += _swordSkill.PlayerSwordData.SwordAtkEnhancedRation;
        }

        enemy.Attribute.TakePhysicalDamage(_player,swordRation);

        EquipmentItemData equipment=InventoryController.Instance.GetEquipment(EquipmentItemType.Amulet);
        if (equipment != null)
        {
            if(InventoryController.Instance.CanUseEquipment(EquipmentItemType.Amulet, equipment))
            {
                equipment.UseEquipmentEffect(enemy.transform);
            }
        }

        enemy.DamageEffect(_player);
    }

    /// <summary>
    /// 剑击中对象处理
    /// </summary>
    /// <param name="collision"></param>
    private void HitObject(Collider2D collision)
    {
        _circleCollider.enabled = false;
        _canRotation = false;
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    /// <summary>
    /// 穿透剑处理逻辑
    /// </summary>
    private void SwordPierceTriggerEnter(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.Attribute.TakePhysicalDamage(_player);

            EquipmentItemData equipment = InventoryController.Instance.GetEquipment(EquipmentItemType.Amulet);
            if (equipment != null)
            {
                if(InventoryController.Instance.CanUseEquipment(EquipmentItemType.Amulet, equipment))
                {
                    equipment.UseEquipmentEffect(enemy.transform);
                }
            }

            enemy.DamageEffect(_player);
            if (_pierceAmount > 0)
            {
                _pierceAmount--;
                _isAdvanceReturn = true;
            }
        }

    }

    #region Spin

    /// <summary>
    /// 旋转剑触碰处理逻辑
    /// </summary>
    private void SwordSpinTriggerEnter(Collider2D collision)
    {
        StopSpinSword();
        _isAdvanceReturn = true;
    }

    /// <summary>
    /// 旋转剑处理逻辑
    /// </summary>
    private void SpinLogic()
    {
        if (_swordSkill.SwordType == SwordType.Spin)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) >= _swordSkill.PlayerSwordData.MaxTravelDistance && !_isStopSpin)
            {
                StopSpinSword();
            }

            if (_isStopSpin)
            {
                _spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + _spinDirection, transform.position.y), _swordSkill.PlayerSwordData.SpinMoveSpeed * Time.deltaTime);

                if (_spinTimer < 0)
                {
                    _isSwordReturning = true;
                    _isStopSpin = false;
                }
            }

            _hitTimer -= Time.deltaTime;
            if (_hitTimer < 0)
            {
                _hitTimer = _swordSkill.PlayerSwordData.SpinHitCooldown;
                Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, _swordSkill.PlayerSwordData.SpinDetectionRadius);

                foreach (var collider in collider2D)
                {
                    if (collider.TryGetComponent(out Enemy enemy))
                    {
                        SwordSkillDamage(enemy);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 停止旋转剑的位置
    /// </summary>
    private void StopSpinSword()
    {
        _isStopSpin = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _spinTimer = _swordSkill.PlayerSwordData.SpinDuration;
    }

    #endregion

    #region Bounce
    /// <summary>
    /// 弹跳剑碰撞处理
    /// </summary>
    private void SwordBounceTriggerEnter(Collider2D collision)
    {

        if (collision.TryGetComponent(out Enemy collisionEnemy))
        {

            if (_enemyTarget.Count <= 0)
            {
                Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, _swordSkill.PlayerSwordData.BounceDetectionRadius);

                foreach (var collider in collider2D)
                {
                    if (collider.TryGetComponent(out Enemy enemy))
                    {
                        _enemyTarget.Add(enemy.transform);
                    }
                }

                SortEnemyPosition(collisionEnemy.transform);
            }

            if (_enemyTarget.Count > 1)
            {
                _isAdvanceReturn = true;
                HitObject(collision);
            }
        }
    }

    /// <summary>
    /// 弹跳剑效果逻辑
    /// </summary>
    private void BounceLogic()
    {
        if (_swordSkill.SwordType == SwordType.Bounce && _enemyTarget.Count > 0&&_bounceAmount>0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTarget[_enemyIndex].position, _swordSkill.PlayerSwordData.BounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _enemyTarget[_enemyIndex].position) < 0.5)
            {

                SwordSkillDamage(_enemyTarget[_enemyIndex].GetComponent<Enemy>());

                _bounceAmount--;
                _enemyIndex++;

                if (_enemyIndex >= _enemyTarget.Count)
                {
                    _enemyIndex = 0;
                }

                if (_bounceAmount <= 0)
                {
                    _isSwordReturning = true;

                }
            }
        }
    }

    /// <summary>
    /// 排序检测到的敌人距离从小到大
    /// </summary>
    private void SortEnemyPosition(Transform firstTransform)
    {
        if (_enemyTarget == null || _enemyTarget.Count == 0 || firstTransform == null)
            return;

        _enemyTarget.Sort((enemy1, enemy2) =>
        {
            if (enemy1 == null && enemy2 == null) return 0;
            if (enemy1 == null) return 1;
            if (enemy2 == null) return -1;

            float distance1 = Vector2.Distance(firstTransform.position, enemy1.position);
            float distance2 = Vector2.Distance(firstTransform.position, enemy2.position);
            return distance1.CompareTo(distance2);
        });
    }

    #endregion
}
