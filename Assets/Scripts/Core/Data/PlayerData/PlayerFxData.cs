using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fx Data", menuName = "GameData/FxData/PlayerFxData")]
public class PlayerFxData : CharacterFxData
{

    [field: Header("冲刺后残影特效")]
    [field: SerializeField] public float ColorLooseRate {  get; private set; }
    [field: SerializeField] public float ShadowCooldown { get; private set; }

    [field: Header("屏幕晃动")]
    [field: SerializeField] public float ShadowMultiplier { get; private set; }
    [field: SerializeField] public Vector3 ShakeSwordImpact {  get; private set; }
    [field:SerializeField] public Vector3 ShakeHightDamage {  get; private set; }

    
}
