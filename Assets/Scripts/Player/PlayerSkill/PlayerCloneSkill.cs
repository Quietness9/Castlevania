using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCloneSkill : Skill
{
    

    [SerializeField] float _colorDisappearSpeed;
    [SerializeField] float _cloneDuration;
    float _cloneTimer;
    bool _isDisappear;
    GameObject _clonePlayer;
    SpriteRenderer _spriteRenderer;



    /// <summary>
    /// 创建并攻击克隆体
    /// </summary>
    public void CreateClonePlayer()
    {


        _cloneTimer =_cloneDuration;
        _clonePlayer = Instantiate(GlobalReferencesManager.Instance.GetPrefab("PlayerClone"), player.transform.position,Quaternion.identity);

        Vector3 scale=_clonePlayer.transform.localScale;
        scale.x *= player.Direction;

        _clonePlayer.transform.localScale = scale;
        _spriteRenderer = _clonePlayer.GetComponentInChildren<SpriteRenderer>();

        int index = Random.Range(1, 4);
        _clonePlayer.GetComponentInChildren<Animator>().SetInteger("AttackNumber",index);
        
        _isDisappear =true;
        
    }

    protected override void Update()
    {
        base.Update();

        if (!_isDisappear)
            return;

        _cloneTimer-= Time.deltaTime;

        if (_cloneTimer > 0.01f)
        {
            _spriteRenderer.color=new Color(1,1,1,_spriteRenderer.color.a-(Time.deltaTime*_colorDisappearSpeed));
        }

        if (_cloneTimer < 0)
        {
            _isDisappear=false;
            Destroy(_clonePlayer);
        }
    }
}
