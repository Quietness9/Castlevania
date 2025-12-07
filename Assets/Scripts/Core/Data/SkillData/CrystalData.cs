using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Crystal Data",menuName ="GameData/Skill/CrystalData")]
public class CrystalData : ScriptableObject
{
    [field:SerializeField] public int MaxSpawnCrystalAmount { get; private set; }
    [field:SerializeField] public float MultCrystalWindowTime {  get; private set; }
    [field:SerializeField] public float CheckRadius {  get;private set; } 
    [field:SerializeField] public float CrystalMoveSpeed {  get;private set; }
    [field: SerializeField] public float CrystalDurationTime { get; private set; }
    [field: SerializeField] public float GrowSpeed { get; private set; }
    [field: SerializeField] public Vector2 GrowScale { get; private set; }


}
