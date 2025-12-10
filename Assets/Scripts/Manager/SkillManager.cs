using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>
{
    public PlayerDashSkill dashSkill {  get; private set; }
    public PlayerCloneSkill CloneSkill { get;private set; }
    public PlayerSwordSkill SwordSkill { get;private set; }
    public PlayerBlackHoleSkill BlackSkill { get; private set; }
    public PlayerCrystalSkill CrystalSkill { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        dashSkill = GetComponent<PlayerDashSkill>();
        CloneSkill = GetComponent<PlayerCloneSkill>();
        SwordSkill = GetComponent<PlayerSwordSkill>();
        BlackSkill = GetComponent<PlayerBlackHoleSkill>();
        CrystalSkill = GetComponent<PlayerCrystalSkill>();
    }
}
