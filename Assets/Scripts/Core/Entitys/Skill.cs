using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float CoolTime;

    protected Player player;
    protected float coolTimer;

    protected virtual void Awake() 
    {
        player = GetComponent<Player>();
    }


    protected virtual void Start() { }


    protected virtual void Update()
    {
        coolTimer-= Time.deltaTime;
    }

    protected virtual void OnDestroy() { }
    

    public virtual bool CanUseSkill()
    {
        if (coolTimer < 0.01f)
        {
            coolTimer = CoolTime;
            return true;
        }

        return false;
    }

}
