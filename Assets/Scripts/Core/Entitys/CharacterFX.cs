using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    
    public CharacterFxData FxData;
    public bool IsStartColorChange { get; private set; }


    Material _originMat;
    SpriteRenderer _spriteRenderer;

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
        _spriteRenderer.material= FxData.HitMat;

        yield return new WaitForSeconds(FxData.FlashDurationTime);

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
    /// 颜色交替改变
    /// </summary>
    private void ColorAlternateChange(Color[] colors)
    {
        if (_spriteRenderer.color != colors[0])
        {
            _spriteRenderer.color = colors[0];
        }
        else
        {
            _spriteRenderer.color = colors[1];
        }
    }

    /// <summary>
    /// 上锁颜色变化
    /// </summary>
    public void LockColorChange()=>IsStartColorChange=true;
    

    /// <summary>
    /// 解锁颜色变化
    /// </summary>
    public void UnLockColorChange()=>IsStartColorChange=false;
    

    /// <summary>
    /// 点燃颜色改变
    /// </summary>
    public void IgniteColorChange()
    {
        ColorAlternateChange(FxData.IgniteColors);
    }

    /// <summary>
    /// 雷电颜色改变
    /// </summary>
    public void ShockColorChange()
    {
        ColorAlternateChange(FxData.ShockColors);
    }

    /// <summary>
    /// 冰冻颜色改变
    /// </summary>
    public void ChillColorChange()
    {
        ColorAlternateChange(FxData.ChillColors);
    }

    /// <summary>
    /// 取消颜色改变
    /// </summary>
    public void CancelColorChange()
    {
        CancelInvoke();
        _spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// 使角色透明
    /// </summary>
    /// <param name="isTransparent"></param>
    public void CharacterTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            _spriteRenderer.color = Color.clear;
        }
        else
        {
            _spriteRenderer.color = Color.white;
        }
    }
}
