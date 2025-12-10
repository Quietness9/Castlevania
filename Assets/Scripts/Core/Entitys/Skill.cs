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
    /// 使用技能
    /// </summary>
    public virtual void UseSkill() { }

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


    /// <summary>
    /// 获得最近距离的敌人位置
    /// </summary>
    public virtual Transform GetClosestEnemy(Transform checkTransform,float checkRadius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, checkRadius);

        Transform resultEnemy = null;

        float enemyDistance=Mathf.Infinity;

        foreach(var collider in colliders)
        {
            if(collider.TryGetComponent(out Enemy enemy))
            {
                float distanceToEnemy=Vector2.Distance(checkTransform.position,enemy.transform.position);

                if(distanceToEnemy < enemyDistance)
                {
                    enemyDistance = distanceToEnemy;
                    resultEnemy = enemy.transform;
                }
            }
        }

        return resultEnemy;
    }


}
