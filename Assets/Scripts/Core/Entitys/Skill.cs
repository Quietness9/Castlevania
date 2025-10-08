using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float Cooldown;


    protected Player player;
    protected float cooldownTimer;

    protected virtual void Awake() { }


    protected virtual void Start() 
    {
        player = GlobalReferencesManager.Instance.GamePlayer;
    }


    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void OnDestroy() { }
    
    /// <summary>
    /// 判断是否可以使用技能
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0.01f)
        {
            cooldownTimer = Cooldown;
            return true;
        }

        return false;
    }

}
