using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="PlayerMoveData")]
public class PlayerMoveData : ScriptableObject
{
    public float MaxSpeed;
    public bool doConserveMomentum;

    [Header("加速度强度系数")]
    public float AccelAmount;
    public float DerceAmount;

    //作用力
    public float AccelForce { get; set; }
    public float DecreForce { get; set; }

    [Header("空气中的加速度")]
    [Range(0.01f, 1)] public float AccelInAir;
    [Range(0.01f, 1)] public float DecreInAir;


    private void OnValidate()
    {
        //使用公式计算运行加速和减速力：amount = （(1 / Time.fixedDeltaTime) *加速度）/ runMaxSpeed
        AccelForce = (50 * AccelAmount) / MaxSpeed;
        DecreForce = (50 * DerceAmount) / MaxSpeed;

        #region Variable Ranges
        AccelAmount = Mathf.Clamp(AccelAmount, 0.01f, MaxSpeed);
        DerceAmount = Mathf.Clamp(DerceAmount, 0.01f, MaxSpeed);
        #endregion
    }
}
