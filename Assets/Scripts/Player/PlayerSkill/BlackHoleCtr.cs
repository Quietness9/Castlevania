using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCtr : MonoBehaviour
{

    Player _player;
    PlayerBlackHoleSkill _blackHoleSkill;

    bool _isGrow;
    bool _isShrink;
    bool _isCreateKey;
    bool _isCloneAttack;

    int _cloneAttackAmount;
    float _cloneAttackTimer;
    float _blackHoleDurationTimer;

    List<Enemy> _enemyTarget = new();
    List<KeyCode> _keyCodeList;

    private void Update()
    {
        if (_isGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_blackHoleSkill.PlayerBlackHoleData.MaxSize,
                _blackHoleSkill.PlayerBlackHoleData.MaxSize), _blackHoleSkill.PlayerBlackHoleData.GrowSpeed * Time.deltaTime);
        }

        BlackHoleShrink();

        CloneAttackLogic();

        SpecialBlackHoleFinish();
    }

    private void OnDestroy()
    {
        if (_player != null && _player.PlayerInput != null)
        {
            _player.PlayerInput.OnBlackHoleEvent -= CloneCrystalAttackHandle;
        }
    }

    /// <summary>
    /// 设置黑洞技能数据
    /// </summary>
    public void SetBlackHoleData(Player player,PlayerBlackHoleSkill blackSkill)
    {
        _player=player;
        _blackHoleSkill=blackSkill;

        _keyCodeList = new List<KeyCode>(blackSkill.PlayerBlackHoleData.KeyCodeList);
        _cloneAttackAmount = blackSkill.PlayerBlackHoleData.CloneAttackAmount;
        _isGrow = true;
        _isCreateKey = true;
        _blackHoleDurationTimer= blackSkill.PlayerBlackHoleData.BlackHoleDuration;

        player.PlayerInput.OnBlackHoleEvent += CloneCrystalAttackHandle;
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
            if (!_blackHoleSkill.IsCreateCrystal)
            {
                //_player.CharacterTransparent(false);
                _player.Fx.CharacterTransparent(false);
            }
            
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1)
                , _blackHoleSkill.PlayerBlackHoleData.ShrinkSpeed * Time.deltaTime);

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
            _cloneAttackTimer = _blackHoleSkill.PlayerBlackHoleData.CloneAttackCooldown;
            int randomIndex = Random.Range(0, _enemyTarget.Count);

            float xOffset = _blackHoleSkill.PlayerBlackHoleData.CloneOffset.x;
            if (Random.Range(0, 10) > 5)
            {
                xOffset = -_blackHoleSkill.PlayerBlackHoleData.CloneOffset.x;
            }

            SkillMgr.Instance.CloneSkill.CreateClonePlayer(_enemyTarget[randomIndex].transform, new Vector3(xOffset, 0, 0));

            _cloneAttackAmount--;

            if (_cloneAttackAmount <= 0)
            {
                Invoke("BlackHoleFinish", _blackHoleSkill.PlayerBlackHoleData.BlackHoleEndDelay);
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
        _blackHoleSkill.SetBlackHoleState(false, true);
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

        GameObject blackHoleKeyPre = GlobalReferencesMgr.Instance.GetPrefab("BlackHoleKey");

        if(blackHoleKeyPre != null)
        {
            GameObject newKey = Instantiate(blackHoleKeyPre, enemy.transform.position + _blackHoleSkill.PlayerBlackHoleData.KeyOffset, Quaternion.identity);
            if (newKey != null)
            {
                KeyCode key = _keyCodeList[Random.Range(0, _keyCodeList.Count)];
                _keyCodeList.Remove(key);
                newKey.GetComponent<BlackHoleKeyCtr>().SetBlackHoleKey(key, enemy, _blackHoleDurationTimer, this);
            }
        }
        
    }

    /// <summary>
    /// 克隆体和水晶开始攻击
    /// </summary>
    private void CloneCrystalAttackHandle()
    {
        if (_blackHoleSkill.IsStart)
        {
            _isCloneAttack = true;
            _isCreateKey = false;

            if (_blackHoleSkill.IsCreateCrystal&&_enemyTarget.Count>0)
            {
                StartCoroutine(CreateCrystalDelay());
            }
        }
    }

    /// <summary>
    /// 延迟生产水晶
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateCrystalDelay()
    {
        int createCrystalAmount = 0;

        while (createCrystalAmount < _blackHoleSkill.PlayerBlackHoleData.CreateCrystalAmount)
        {
            GameObject crystalObj = SkillMgr.Instance.CrystalSkill.CreateCrystal(_player.transform);

            Transform enemyTransform = _enemyTarget[Random.Range(0, _enemyTarget.Count)].transform;

            crystalObj.GetComponent<CrystalCtr>().SetClosestEnemy(enemyTransform);

            createCrystalAmount++;

            yield return new WaitForSeconds(_blackHoleSkill.PlayerBlackHoleData.CreateCrystalDelay);
        }
    }
}
