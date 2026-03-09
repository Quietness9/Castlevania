using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{

    public CharacterFxData FxData;
    public CharacterHealthUI HealthUI;
    
    public bool IsStartColorChange { get; private set; }

    Material _originMat;
    GameObject _particleObj;
    SpriteRenderer _spriteRenderer;
    Vector3 _particleOffset=new Vector3(0,0.5f,0);

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originMat = _spriteRenderer.material;
    }

    /// <summary>
    /// 白色闪光特效
    /// </summary>
    protected IEnumerator FlashFX()
    {
        _spriteRenderer.material= FxData.HitMat;

        yield return new WaitForSeconds(FxData.FlashDurationTime);

        _spriteRenderer.material=_originMat;
    }

    /// <summary>
    /// 反击后的红色闪光
    /// </summary>
    protected void RedColorBlink()
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
    /// 生成魔法特效粒子
    /// </summary>
    /// <param name="preName"></param>
    public void SpawnMagicParticle(string preName)
    {
        GameObject particlePre = GlobalReferencesMgr.Instance.GetPrefab(preName);

        if (particlePre != null && _particleObj == null)
        {
            _particleObj = ObjectPoolMgr.SpawnObject(particlePre, transform.position + _particleOffset, Quaternion.identity, PoolType.ParticleObject);
            _particleObj.transform.parent = transform;

            _particleObj.GetComponent<ParticleSystem>()?.Play();
        }
    }


    /// <summary>
    /// 取消颜色改变
    /// </summary>
    public void CancelColorChange()
    {
        CancelInvoke();

        if (_particleObj != null)
        {
            _particleObj.GetComponent<ParticleSystem>()?.Stop();
            ObjectPoolMgr.ReturnObjectToPool(_particleObj, PoolType.ParticleObject);
            _particleObj = null;
        }
        

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
            HealthUI.gameObject.SetActive(false);
            _spriteRenderer.color = Color.clear;
        }
        else
        {
            HealthUI.gameObject.SetActive(true);
            _spriteRenderer.color = Color.white;
        }
        
    }

    /// <summary>
    /// 创造击打特效
    /// </summary>
    /// <param name="hitFXType"></param>
    /// <param name="target"></param>
    public void CreateHitFX(HitFXType hitFXType,Transform target)
    {
        GameObject hitFXPre = GlobalReferencesMgr.Instance.GetPrefab(GetHitFXPrefabName(hitFXType));

        if (hitFXPre == null)
            return;

        Vector3 offset = new Vector3(Random.Range(FxData.HitFXOffsetX.x, FxData.HitFXOffsetX.y),
            Random.Range(FxData.HitFXOffsetY.x, FxData.HitFXOffsetY.y));

        float rotation=Random.Range(FxData.HitFXRotation.x, FxData.HitFXRotation.y);

        GameObject hitObj=Instantiate(hitFXPre,target.position+offset,Quaternion.identity);

        hitObj.transform.Rotate(new Vector3(0,0, rotation));

        Destroy(hitObj,1f);

    }

    /// <summary>
    /// 获得攻击特效的预制体名
    /// </summary>
    /// <param name="hitFXType"></param>
    /// <returns></returns>
    private string GetHitFXPrefabName(HitFXType hitFXType)
    {
        string hitFXName = "";

        switch (hitFXType)
        {
            case HitFXType.HitFX00: hitFXName = "Hit00FX"; break;
            case HitFXType.HitFX01: hitFXName = "Hit01FX"; break;
        }

        return hitFXName;
    }

    /// <summary>
    /// 创建提示文本特效
    /// </summary>
    /// <param name="text"></param>
    public void CreatePopUpTextFx(string text)
    {
        GameObject popUpTextPre = GlobalReferencesMgr.Instance.GetPrefab("PopUpText");

        if (popUpTextPre == null)
            return;

        GameObject popUpTextObj = Instantiate(popUpTextPre, transform.position + FxData.TextOffset, Quaternion.identity);

        popUpTextObj.GetComponent<PopUpTextCtr>().SetPopUpText(FxData, text);
    }

}
