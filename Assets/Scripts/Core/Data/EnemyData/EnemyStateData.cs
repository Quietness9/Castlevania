using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New EnemyState Data",menuName ="GameData/EnemyStateData")]
public class EnemyStateData : ScriptableObject
{
    [Header("Íæ¼Ò¼ì²â")]
    public float CheckPlayerDistance;
    public LayerMask PlayerLayer;

    [Header("ÏÐÖÃ×´Ì¬")]
    public float IdleTime;

    [Header("ÒÆ¶¯×´Ì¬")]
    public float MoveSpeed;
    public float MoveTime;

    [Header("Î£ÏÕ×´Ì¬")]
    public float BattleTime;

    [Header("¹¥»÷×´Ì¬")]
    public float IgnoreDistance;
    public Vector2 AttackCooldownOffset;

    [Header("Ñ£ÔÎ×´Ì¬")]
    public float StunnedMul;
}
