using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sword Data", menuName = "GameData/Skill/SwordData")]
public class SwordData : ScriptableObject
{
    [field: Header("基础剑")]
    [field: SerializeField] public Vector2 SwordForce { get; private set; }
    [field: SerializeField] public float SwordGravity { get; private set; }
    [field: SerializeField] public float FreezeTime { get; private set; }
    [field: SerializeField] public float ReturnSpeed { get; private set; }
    [field: SerializeField] public float MaxMoveTime { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float SwordAtkRation { get; private set; }
    [field:Range(0,1f)]
    [field:SerializeField] public float SwordAtkEnhancedRation {  get; private set; }

    [field: Header("弹跳剑")]
    [field: SerializeField] public float BounceGravity { get; private set; }
    [field: SerializeField] public float BounceSpeed { get; private set; }
    [field: SerializeField] public int BounceAmount { get; private set; }
    [field: SerializeField] public float BounceDetectionRadius { get; private set; }

    [field: Header("穿透剑")]
    [field: SerializeField] public float PierceGravity { get; private set; }
    [field: SerializeField] public int PierceAmount { get; private set; }

    [field: Header("旋转剑")]
    [field: SerializeField] public float SpinGravity { get; private set; }
    [field: SerializeField] public float MaxTravelDistance { get; private set; }
    [field: SerializeField] public float SpinDuration { get; private set; }
    [field: SerializeField] public float SpinHitCooldown { get; private set; }
    [field: SerializeField] public float SpinDetectionRadius { get; private set; }
    [field: SerializeField] public float SpinMoveSpeed { get; private set; }

    [field: Header("瞄准点设置")]
    [field: SerializeField] public int DotsCount { get; private set; }
    [field: SerializeField] public float SpaceBetweenDots { get; private set; }




    /// <summary>
    /// 获得不同类型的剑的重力
    /// </summary>
    public float getSwordGravity(SwordType swordType) 
    {
        switch (swordType)
        {
            case SwordType.Bounce: return BounceGravity;
            case SwordType.Pierce: return PierceGravity;
            case SwordType.Spin: return SpinGravity;
            case SwordType.Ordinary: return SwordGravity;
        }

        return -1;
    }
}
