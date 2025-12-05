using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCloneSkill : Skill
{
    public CloneData PlayerCloneData;
    
    //通过技能树解锁
    public bool IsCreateDuplicateClone;
    public bool IsCreateCloneOnDashEnd;
    public bool IsCreateCloneOnDashStart;
    public bool IsCreateCloneOnCounterAttack;

    GameObject _clonePlayer;

    /// <summary>
    /// 创建克隆体
    /// </summary>
    public void CreateClonePlayer(Transform transform,Vector3 offset= default)
    {
        GameObject clonePre = GlobalReferencesManager.Instance.GetPrefab("PlayerClone");
        if (clonePre == null)
            return;

        _clonePlayer = Instantiate(clonePre, transform.position + offset, Quaternion.identity);
        Transform closestTarget = GetClosestEnemy(_clonePlayer.transform, PlayerCloneData.CheckClosestEnemyRadius);
        _clonePlayer.GetComponentInChildren<CloneAnimationTrigger>().SetPlayerClone(player,this,closestTarget);

        CloneAttack();

    }

    /// <summary>
    /// 在冲刺开始时创建克隆体
    /// </summary>
    public void CreateCloneOnDashStart(Transform transform, Vector3 offset = default)
    {
        if (IsCreateCloneOnDashStart)
        {
            CreateClonePlayer(transform,offset);
        }
    }

    /// <summary>
    /// 在冲刺结束后创建克隆体
    /// </summary>
    public void CreateCloneOnDashEnd(Transform transform, Vector3 offset = default)
    {
        if (IsCreateCloneOnDashEnd)
        {
            CreateClonePlayer(transform, offset);
        }
    }

    /// <summary>
    /// 反击成功创造克隆体
    /// </summary>
    public void CreateCloneOnCounterAttack(Transform transform, Vector3 offset = default)
    {
        if(IsCreateCloneOnCounterAttack)
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

}
