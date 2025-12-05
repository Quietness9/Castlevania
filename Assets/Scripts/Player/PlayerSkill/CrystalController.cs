using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    Player _player;
    Animator _animator;
    Transform _closestEnemy;
    PlayerCrystalSkill _crystalSkill;
    CircleCollider2D _circleCollider2D;

    bool _isGrow;
    float _crystalDurationTimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _circleCollider2D=GetComponent<CircleCollider2D>();
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
            transform.localScale=Vector2.Lerp(transform.localScale,_crystalSkill.PlayerCrystalData.GrowScale
                , _crystalSkill.PlayerCrystalData.GrowSpeed*Time.deltaTime);
        }

        if (_crystalSkill.IsCanMove)
        {
            if( _closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, _closestEnemy.position,
                _crystalSkill.PlayerCrystalData.CrystalMoveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, _closestEnemy.position) < 1.5)
                {
                    CrystalExplosion();
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_player.PlayerInput != null)
        {
            _player.PlayerInput.CrystalEvent -= CrystalLogicHandle;
        }
    }

    /// <summary>
    /// 设置水晶数据
    /// </summary>
    /// <param name="player"></param>
    /// <param name="crystalSkill"></param>
    public void SetCrystalData(Player player, PlayerCrystalSkill crystalSkill)
    {
        _player = player;
        _crystalSkill = crystalSkill;
        _crystalDurationTimer = crystalSkill.PlayerCrystalData.CrystalDurationTime;
        
        player.PlayerInput.CrystalEvent += CrystalLogicHandle;
    }

    

    /// <summary>
    /// 对于不同解锁状态下水晶处理逻辑订阅
    /// </summary>
    private void CrystalLogicHandle()
    {
        if(!_crystalSkill.ControllerOneCrystal)
            return;

        OneCrystalLogic();
    }

    /// <summary>
    /// 单个水晶处理
    /// </summary>
    private void OneCrystalLogic()
    {
        //在此触发按键时执行

        // 此次添加或修改的逻辑：优先处理组合技能
        // 执行完高级组合后返回，避免执行基础逻辑

        _crystalSkill.ControllerOneCrystal = false;

        if (_crystalSkill.IsCanMove && _crystalSkill.IsCanExplode)
        {
            CrystalExplosion();
            return;
        }

        if (_crystalSkill.IsSwapPosition && _crystalSkill.IsCrystalReplaceClone)
        {
            PlayerSwapCrystalPosition();
            ReplaceCrystalToClone();
            return;
        }

        if (_crystalSkill.IsSwapPosition && _crystalSkill.IsCanExplode)
        {
            PlayerSwapCrystalPosition();
            CrystalExplosion();
            return;
        }

        // 如果没有高级组合，则执行基础逻辑
        if (_crystalSkill.IsSwapPosition)
        {
            PlayerReturnCrystalPosition();
        }
        
    }

    #region 水晶功能函数

    /// <summary>
    /// 销毁自身
    /// </summary>
    public void DestroySelf() => Destroy(gameObject);

    /// <summary>
    /// 设置最近的敌人
    /// </summary>
    public void SetClosestEnemy(Transform EnemyTransform)
    {
        _closestEnemy = EnemyTransform;
    }


    /// <summary>
    /// 水晶爆炸
    /// </summary>
    private void CrystalExplosion()
    {
        _isGrow=true;
        _animator.SetTrigger("Explode");
    }

    /// <summary>
    /// 交换水晶和玩家位置
    /// </summary>
    private void PlayerSwapCrystalPosition()
    {
        Vector3 position = _player.transform.position;
        PlayerReturnCrystalPosition();
        transform.position=position;
    }


    /// <summary>
    /// 玩家返回到水晶位置
    /// </summary>
    private void PlayerReturnCrystalPosition()
    {
        _player.transform.position = transform.position;
    }

    /// <summary>
    /// 交换位置时将水晶替换为克隆体
    /// </summary>
    private void ReplaceCrystalToClone()
    {
        SkillManager.Instance.CloneSkill.CreateClonePlayer(transform);
        DestroySelf();
    }

    #endregion

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
                enemy.DamageEffect(_player);
                enemy.TakeDamage(_player,true);
            }
        }
    }
}
