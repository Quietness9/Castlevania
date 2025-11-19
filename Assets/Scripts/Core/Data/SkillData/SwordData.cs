using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sword Data", menuName = "GameData/Skill/SwordData")]
public class SwordData : ScriptableObject
{
    [Header("基础剑")]
    public Vector2 SwordForce;
    public Vector3 Offset;
    public float SwordGravity;
    public float FreezeTime;
    public float ReturnSpeed;
    public float MaxMoveDistance;

    [Header("弹跳剑")]
    public float BounceGravity;
    public float BounceSpeed;
    public int BounceAmount;
    public float BounceDetectionRadius;

    [Header("穿透剑")]
    public float PierceGravity;
    public int PierceAmount;

    [Header("旋转剑")]
    public float SpinGravity;
    public float MaxTravelDistance;
    public float SpinDuration;
    public float SpinHitCooldown;
    public float SpinDetectionRadius;
    public float SpinMoveSpeed;

    [Header("瞄准点设置")]
    public int DotsCount;
    public float SpaceBetweenDots;

    
    

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
