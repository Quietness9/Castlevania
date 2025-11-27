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
    /// 创建并攻击克隆体
    /// </summary>
    public void CreateClonePlayer(Transform transform=null,Vector3 offset= default)
    {
        if (!CanUseSkill())
            return;

        Transform createTransform=transform;

        if(createTransform == null)
        {
            createTransform=player.transform;
        }

        _clonePlayer = Instantiate(GlobalReferencesManager.Instance.GetPrefab("PlayerClone"), createTransform.transform.position+offset,Quaternion.identity);
        _clonePlayer.GetComponentInChildren<CloneAnimationTrigger>().SetPlayerClone(_cloneDuration,_colorDisappearSpeed);

        Vector3 scale=_clonePlayer.transform.localScale;
        scale.x *= player.Direction;

        _clonePlayer.transform.localScale = scale;

        int index = Random.Range(1, 4);
        _clonePlayer.GetComponentInChildren<Animator>().SetInteger("AttackNumber",index);

        
    }

    protected override void Update()
    {
        base.Update();
    }
}
