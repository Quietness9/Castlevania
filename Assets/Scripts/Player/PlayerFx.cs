using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFx : CharacterFX
{
    public PlayerFxData PlayerFxData {  get; private set; }

    [SerializeField] ParticleSystem _dustFx;

    Player _player;
    CinemachineImpulseSource _screenShake;

    float _shadowCooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        _player = GetComponentInParent<Player>();
        _screenShake = GetComponentInParent<CinemachineImpulseSource>();


        PlayerFxData = FxData as PlayerFxData;
    }

    private void Update()
    {
        _shadowCooldownTimer-= Time.deltaTime;
    }

    /// <summary>
    /// 播放收回剑特效
    /// </summary>
    public void PlayDustFx()
    {
        if(_dustFx != null)
        {
            _dustFx.Play();
        }
    }

    /// <summary>
    /// 创建冲刺残影
    /// </summary>
    public void CreateDashShadow()
    {
        if(_shadowCooldownTimer>0)
            return;

        GameObject dashShadowPre=GlobalReferencesMgr.Instance.GetPrefab("DashShadow");

        if (dashShadowPre == null)
            return;

        GameObject dashShadowObj=Instantiate(dashShadowPre,transform.position,transform.rotation);
        dashShadowObj.GetComponent<DashShadowCtr>().SetDashShadow(PlayerFxData.ColorLooseRate);
        _shadowCooldownTimer = PlayerFxData.ShadowCooldown;

    }

    /// <summary>
    /// 让屏幕晃动
    /// </summary>
    public void ScreenShakeFx(Vector3 shakePower)
    {
        _screenShake.m_DefaultVelocity = new Vector3(shakePower.x * _player.Direction,
            shakePower.y) * PlayerFxData.ShadowMultiplier;

        _screenShake.GenerateImpulse();
    }
}
