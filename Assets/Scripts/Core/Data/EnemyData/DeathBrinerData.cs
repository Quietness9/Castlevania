using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DeathBriner Data", menuName ="CharacterData/EnemyData/DeathBriner Data")]
public class DeathBrinerData : ScriptableObject
{
    //对周围的检查大小
    [field:SerializeField] public Vector2 SurroundCheck { get;private set; }
    //每攻击一次增加下一次传送的概率大小
    [field:SerializeField] public Vector2 IncreaseTeleportRatio { get;private set; }
    [field:SerializeField] public float DefaultChanceTeleport { get;private set; }
    //转为传送状态的界限
    [field:SerializeField] public float TeleportLimit { get;private set; }
    [field:SerializeField] public float SpellStateCooldown { get;private set; }
    [field:SerializeField] public float SpellAttackCooldown { get; private set; }
    [field:SerializeField] public int SpellAttackCount { get; private set; }
    [field:SerializeField] public Vector3 CreateSpellOffset { get; private set; }
}
