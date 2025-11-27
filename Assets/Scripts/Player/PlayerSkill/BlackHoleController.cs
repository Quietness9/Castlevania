using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{

    BlackHoleData _blackHoleData;
    bool _isCloneAttack;
    bool _isGrow;
    bool _isShrink;
    bool _isCreateKey;

    int _cloneAttackAmount=5;
    float _cloneAttackTimer;
    float _blackHoleDurationTimer;

    List<Enemy> _enemyTarget = new();
    List<KeyCode> _keyCodeList;


    private void Start()
    {
        if(GlobalReferencesManager.Instance != null)
        {
           GlobalReferencesManager.Instance.GamePlayer.PlayerInput.BlackHoleEvent+= CloneStartAttackHandle;
        }
    }

    private void Update()
    {
        if (_isGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_blackHoleData.MaxSize, _blackHoleData.MaxSize)
                , _blackHoleData.GrowSpeed * Time.deltaTime);
        }

        BlackHoleShrink();

        CloneAttackLogic();

        SpecialBlackHoleFinish();
    }

    private void OnDestroy()
    {
        if(GlobalReferencesManager.Instance!= null&& GlobalReferencesManager.Instance.GamePlayer.PlayerInput != null)
        {
            GlobalReferencesManager.Instance.GamePlayer.PlayerInput.BlackHoleEvent -= CloneStartAttackHandle;
        }
        
    }

    /// <summary>
    /// 设置黑洞技能数据
    /// </summary>
    public void SetBlackHoleData(BlackHoleData data)
    {
        _blackHoleData = data;
        _keyCodeList = new List<KeyCode>(data.KeyCodeList);
        _cloneAttackAmount = data.CloneAttackAmount;
        _isGrow = true;
        _isCreateKey = true;
        _blackHoleDurationTimer=data.BlackHoleDuration;
    }
    
    /// <summary>
    /// 添加敌人到列表
    /// </summary>
    /// <param name="target"></param>
    public void AddEnemyToList(Enemy target) => _enemyTarget.Add(target);


    /// <summary>
    /// 特殊情况下结束黑洞技能
    /// </summary>
    private void SpecialBlackHoleFinish()
    {
        _blackHoleDurationTimer-= Time.deltaTime;
        
        //超过技能持续时间
        if( _blackHoleDurationTimer < 0)
        {
            _blackHoleDurationTimer=Mathf.Infinity;
            BlackHoleFinish();
            return;
        }

        //空放技能
        if (_isCloneAttack&&_enemyTarget.Count <= 0)
        {
            BlackHoleFinish();
            return;
        }
    }

    /// <summary>
    /// 缩小并销毁黑洞
    /// </summary>

    private void BlackHoleShrink()
    {
        if (!_isCloneAttack && _isShrink)
        {
            GlobalReferencesManager.Instance.GamePlayer.CharacterTransparent(false);
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1)
                , _blackHoleData.ShrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 克隆体攻击逻辑
    /// </summary>
    private void CloneAttackLogic()
    {
        if (_enemyTarget.Count <= 0)
            return;

        _cloneAttackTimer -= Time.deltaTime;
        if (_isCloneAttack && _cloneAttackTimer <= 0)
        {
            _cloneAttackTimer = _blackHoleData.CloneAttackCooldown;
            int randomIndex = Random.Range(0, _enemyTarget.Count);

            float xOffset = _blackHoleData.CloneOffset.x;
            if (Random.Range(0, 10) > 5)
            {
                xOffset = -_blackHoleData.CloneOffset.x;
            }

            SkillManager.Instance.CloneSkill.CreateClonePlayer(_enemyTarget[randomIndex].transform, new Vector3(xOffset, 0, 0));

            _cloneAttackAmount--;

            if (_cloneAttackAmount <= 0)
            {
                BlackHoleFinish();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.IsFreezeSelf(true);

            CreateBlackHoleKey(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.IsFreezeSelf(false);
        }
    }

    /// <summary>
    /// 黑洞技能结束
    /// </summary>
    private void BlackHoleFinish()
    {
        _isGrow = false;
        _isShrink = true;
        _isCloneAttack = false;
        SkillManager.Instance.BlackSkill.SetBlackHoleState(false, true);
    }

    /// <summary>
    /// 创建敌人头上的攻击键
    /// </summary>
    /// <param name="enemy"></param>
    private void CreateBlackHoleKey(Enemy enemy)
    {

        if (_keyCodeList.Count <= 0)
        {
            Debug.LogWarning("攻击键为0");
            return;
        }

        if (!_isCreateKey)
            return;

        GameObject newKey = Instantiate(GlobalReferencesManager.Instance.GetPrefab("BlackHoleKey"),
                        enemy.transform.position + _blackHoleData.KeyOffset, Quaternion.identity);

        if (newKey != null)
        {
            KeyCode key = _keyCodeList[Random.Range(0, _keyCodeList.Count)];
            _keyCodeList.Remove(key);
            newKey.GetComponent<BlackHoleKeyController>().SetBlackHoleKey(key,enemy,_blackHoleDurationTimer,this);
        }
    }

    /// <summary>
    /// 克隆体开始攻击
    /// </summary>
    private void CloneStartAttackHandle()
    {
        if (SkillManager.Instance.BlackSkill.IsStart)
        {
            _isCloneAttack = true;
            _isCreateKey = false;
        }
    }
}
