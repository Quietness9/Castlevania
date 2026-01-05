using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Fx Data",menuName ="GameData/FxData")]
public class CharacterFxData : ScriptableObject
{
    [field:Header("基础信息")]
    [field:SerializeField] public float RepeatTime {  get;private set; }

    [field:Header("物理攻击特效")]
    [field: SerializeField] public Material HitMat { get; private set; }
    [field: SerializeField] public float FlashDurationTime {  get; private set; }

    [field: Header("魔法颜色特效")]
    [field: SerializeField] public Color[] ChillColors { get; private set; }
    [field: SerializeField] public Color[] IgniteColors { get; private set; }
    [field: SerializeField] public Color[] ShockColors {  get; private set; }

    [field:Header("打击特效")]
    [field: SerializeField] public Vector2 HitFXOffsetX {  get; private set; }
    [field: SerializeField] public Vector2 HitFXOffsetY { get; private set; }
    [field: SerializeField] public Vector2 HitFXRotation { get; private set; }


}
