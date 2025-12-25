using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCloneSkill : Skill
{
    public CloneData PlayerCloneData;

    //通过技能树解锁
    public bool IsLock;
    public bool IsCloneAttack;
    public bool IsCloneEnhancedAtk;
    public bool IsCreateDuplicateClone;

    GameObject _clonePlayer;
    SkillManager _skillManager;

    protected override void Start()
    {
        base.Start();
        _skillManager = SkillManager.Instance;
    }

    /// <summary>
    /// 创建克隆体
    /// </summary>
    public void CreateClonePlayer(Transform transform,Vector3 offset= default)
    {
        if(!IsLock)
            return;

        GameObject clonePre = GlobalReferencesManager.Instance.GetPrefab("PlayerClone");
        if (clonePre == null)
            return;

        _clonePlayer = Instantiate(clonePre, transform.position + offset, Quaternion.identity);
        Transform closestTarget = GetClosestEnemy(_clonePlayer.transform, PlayerCloneData.CheckClosestEnemyRadius);
        _clonePlayer.GetComponentInChildren<CloneAnimationTrigger>().SetPlayerClone(player,this,closestTarget);

        if (IsCloneAttack)
        {
            CloneAttack();
        }
        

    }

    /// <summary>
    /// 在冲刺开始时创建克隆体
    /// </summary>
    public void CreateCloneOnDashStart(Transform transform, Vector3 offset = default)
    {
        if (_skillManager.dashSkill.IsCreateCloneDashStart)
        {
            CreateClonePlayer(transform,offset);
        }
    }

    /// <summary>
    /// 在冲刺结束后创建克隆体
    /// </summary>
    public void CreateCloneOnDashEnd(Transform transform, Vector3 offset = default)
    {
        if (_skillManager.dashSkill.IsCreateCloneDashEnd)
        {
            CreateClonePlayer(transform, offset);
        }
    }

    /// <summary>
    /// 反击成功创造克隆体
    /// </summary>
    public void CreateCloneOnCounterAttack(Transform transform, Vector3 offset = default)
    {
        if(_skillManager.ParrySkill.IsCreateClone)
        {
            StartCoroutine(CreateCloneDelayCo(transform,offset));
        }
    }

   /// <summary>
   /// 延迟调用创建克隆体
   /// </summary>
   /// <param name="transform"></param>
   /// <param name="offset"></param>
   /// <returns></returns>
    private IEnumerator CreateCloneDelayCo(Transform transform, Vector3 offset = default)
    {
        yield return new WaitForSeconds(PlayerCloneData.CreateCloneDelayTime);
        CreateClonePlayer(transform,offset);
    }

    /// <summary>
    /// 克隆体攻击
    /// </summary>
    private void CloneAttack()
    {
        Vector3 scale = _clonePlayer.transform.localScale;
        scale.x *= player.Direction;

        _clonePlayer.transform.localScale = scale;

        int index = Random.Range(1, 4);
        _clonePlayer.GetComponentInChildren<Animator>().SetInteger("AttackNumber", index);
    }

    #region 技能解锁

    /// <summary>
    /// 解锁技能
    /// </summary>
    public void UnLockClone() => IsLock = true;

    /// <summary>
    /// 解锁克隆体攻击
    /// </summary>
    public void UnLockCloneAttack()=>IsCloneAttack = true;

    /// <summary>
    /// 解锁增强克隆体攻击力
    /// </summary>
    public void UnLockCloneEnhancedAtk()=>IsCloneEnhancedAtk = true;

    /// <summary>
    /// 解锁在次生成克隆体
    /// </summary>
    public void UnLockCreateDuplicateClone()=>IsCreateDuplicateClone = true;

    #endregion

}
