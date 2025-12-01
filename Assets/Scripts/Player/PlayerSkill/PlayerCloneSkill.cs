using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCloneSkill : Skill
{
    
    [SerializeField] float _colorDisappearSpeed;
    [SerializeField] float _cloneDuration;
    GameObject _clonePlayer;

    /// <summary>
    /// 创建克隆体
    /// </summary>
    public void CreateClonePlayer(Transform transform=null,Vector3 offset= default)
    {
        if (!CanUseSkill())
            return;

        GameObject clonePre = GlobalReferencesManager.Instance.GetPrefab("PlayerClone");
        if (clonePre == null)
            return;

        Transform createTransform = transform;

        if (createTransform == null)
        {
            createTransform = player.transform;
        }

        _clonePlayer = Instantiate(clonePre, createTransform.transform.position + offset, Quaternion.identity);
        _clonePlayer.GetComponentInChildren<CloneAnimationTrigger>().SetPlayerClone(_cloneDuration, _colorDisappearSpeed);

        CloneAttack();

    }


    protected override void Update()
    {
        base.Update();
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
