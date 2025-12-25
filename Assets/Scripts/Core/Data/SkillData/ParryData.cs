using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Paryy Data", menuName = "GameData/Skill/ParryData")]
public class ParryData:ScriptableObject
{
    [field:Range(0,1f)]
    [field: SerializeField] public float RecoverHpRatio { get; private set; }
}
