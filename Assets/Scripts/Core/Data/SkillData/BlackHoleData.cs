using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BlackHole Data", menuName = "GameData/Skill/BlackHole")]
public class BlackHoleData : ScriptableObject
{

    [field: SerializeField] public Vector3 KeyOffset { get; private set; }
    [field: SerializeField] public Vector3 CloneOffset { get; private set; }
    [field:SerializeField] public float BlackHoleDuration {  get; private set; }
    [field: SerializeField] public float FlyTime { get; private set; }
    [field: SerializeField] public float FlySpeed { get; private set; }
    [field: SerializeField] public float LandSpeed { get; private set; }
    [field: SerializeField] public float MaxSize { get; private set; }
    [field: SerializeField] public float GrowSpeed { get; private set; }
    [field: SerializeField] public float ShrinkSpeed { get; private set; }
    [field: SerializeField] public float BlackHoleFreezeTime { get; private set; }
    [field: SerializeField] public float CloneAttackCooldown { get; private set; }
    [field:SerializeField] public float BlackHoleEndDelay {  get; private set; }
    [field:SerializeField] public float CreateCrystalDelay {  get; private set; }
    [field: SerializeField] public int CloneAttackAmount { get; private set; }
    [field:SerializeField] public int CreateCrystalAmount {  get; private set; }

    [field: SerializeField] public List<KeyCode> KeyCodeList { get; private set; } = new();

}
