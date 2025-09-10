using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="PlayerMoveData")]
public class PlayerMoveData : ScriptableObject
{
    public float MaxSpeed;
    public bool doConserveMomentum;

    [Header("���ٶ�ǿ��ϵ��")]
    public float AccelAmount;
    public float DerceAmount;

    //������
    public float AccelForce { get; set; }
    public float DecreForce { get; set; }

    [Header("�����еļ��ٶ�")]
    [Range(0.01f, 1)] public float AccelInAir;
    [Range(0.01f, 1)] public float DecreInAir;


    private void OnValidate()
    {
        //ʹ�ù�ʽ�������м��ٺͼ�������amount = ��(1 / Time.fixedDeltaTime) *���ٶȣ�/ runMaxSpeed
        AccelForce = (50 * AccelAmount) / MaxSpeed;
        DecreForce = (50 * DerceAmount) / MaxSpeed;

        #region Variable Ranges
        AccelAmount = Mathf.Clamp(AccelAmount, 0.01f, MaxSpeed);
        DerceAmount = Mathf.Clamp(DerceAmount, 0.01f, MaxSpeed);
        #endregion
    }
}
