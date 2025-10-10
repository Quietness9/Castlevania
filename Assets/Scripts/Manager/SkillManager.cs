using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>
{
    public PlayerCloneSkill CloneSkill { get;private set; }
    public PlayerSwordSkill SwordSkill { get;private set; }
    protected override void Awake()
    {
        base.Awake();

        CloneSkill = GetComponent<PlayerCloneSkill>();
        SwordSkill = GetComponent<PlayerSwordSkill>();
    }
}
