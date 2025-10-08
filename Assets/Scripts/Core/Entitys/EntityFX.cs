using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Material _originMat;

    [Header("FlashFX")]
    [SerializeField] Material _hitMat;
    [SerializeField] float _flashDuration;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _originMat=_spriteRenderer.material;    
    }

    /// <summary>
    /// 白色闪光特效
    /// </summary>
    private IEnumerator FlashFX()
    {
        _spriteRenderer.material= _hitMat;

        yield return new WaitForSeconds(_flashDuration);

        _spriteRenderer.material=_originMat;
    }

    /// <summary>
    /// 反击后的红色闪光
    /// </summary>
    private void RedColorBlink()
    {
        if (_spriteRenderer.color != Color.white)
        {
            _spriteRenderer.color= Color.white;
        }
        else
        {
            _spriteRenderer.color=Color.red;
        }
    }

    /// <summary>
    /// 取消红闪
    /// </summary>
    public void CancelRedBlink()
    {
        CancelInvoke("RedColorBlink");
        _spriteRenderer.color = Color.white;
    }
}
