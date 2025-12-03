using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CloneAnimationTrigger : MonoBehaviour
{
    Transform _attackCheck;
    Transform _closestTarget;
    SpriteRenderer _spriteRenderer;
    PlayerCloneSkill _cloneSkill;

    bool _isDisappear;
    float _cloneTimer;
    float _faceDir=1;

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
            _spriteRenderer.color = new Color(1, 1, 1, _spriteRenderer.color.a - (Time.deltaTime * _cloneSkill.PlayerCloneData.ColorDisappearSpeed));
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
    public void SetPlayerClone(PlayerCloneSkill cloneSkill,Transform closestTarget, bool isDisappear=true)
    {
        _isDisappear = isDisappear;
        _closestTarget = closestTarget;
        _cloneTimer=cloneSkill.PlayerCloneData.CloneDuration;
        _cloneSkill = cloneSkill;

        FaceClosestTarget();
    }

    /// <summary>
    /// 让克隆体面向最近的敌人
    /// </summary>
    private void FaceClosestTarget()
    {
        if( _closestTarget != null )
        {
            if (_closestTarget.position.x < transform.position.x)
            {
                _faceDir *= -1;
                transform.Rotate(0,180,0);
            }
        }
    }

    /// <summary>
    /// 攻击动画完成回调
    /// </summary>
    private void AttackAnimationFinish()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheck.position, _cloneSkill.PlayerCloneData.AttackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            
            if (hit.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage(GlobalReferencesManager.Instance.GamePlayer);
                if (_cloneSkill.IsCreateDuplicateClone&&(Random.Range(0,10)> _cloneSkill.PlayerCloneData.DutCreateCloneProbability))
                {
                    SkillManager.Instance.CloneSkill.CreateClonePlayer(enemy.transform,
                        new Vector2(_cloneSkill.PlayerCloneData.DutCreateCloneOffset.x*_faceDir, _cloneSkill.PlayerCloneData.DutCreateCloneOffset.y));
                }
            }
        }
    }

}
