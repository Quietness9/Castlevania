using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeCtr : MonoBehaviour
{
    [SerializeField] float _delay;
    [SerializeField] Vector3 _increaseScale;
    [SerializeField] Vector3 _animationOffset;

    Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 播放雷电特效
    /// </summary>
    /// <param name="target"></param>
    /// <param name="attackTarget"></param>
    public void ShockEffect()
    {
        PlayShock();
        Invoke("ShockDestroy", _delay);
    }

    /// <summary>
    /// 播放电击动画
    /// </summary>
    private void PlayShock()
    {
        if (_animator == null)
        {
            Debug.LogWarning("ShockStrike Animator is null");
            return;
        }
        
        _animator.transform.localPosition += _animationOffset;
        transform.localScale = _increaseScale;

        _animator.SetTrigger("Hit");
    }

    /// <summary>
    /// 销毁雷电
    /// </summary>
    private void ShockDestroy()
    {
        Destroy(gameObject);
    }
}
