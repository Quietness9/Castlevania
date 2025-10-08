using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>
{
    public ClonePlayer CloneSkill { get;private set; }

    protected override void Awake()
    {
        base.Awake();

        CloneSkill = GetComponent<ClonePlayer>();
    }
}
