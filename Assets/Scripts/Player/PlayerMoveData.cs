using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Move Data",menuName = "PlayerData/MoveData")]
public class PlayerMoveData : ScriptableObject
{

    [Header("�������")]
    //��������
    public float FallGravityMult;
    public float MaxFallSpeed;
    //��ס�¼�
    public float FastFallGravityMult;
    public float MaxFastFallSpeed;


    [Space(4)]
    [Header("�ƶ�����")]
    public float MaxMoveSpeed;
    public bool doConserveMomentum;

    [Header("���ٶ�ǿ��ϵ��")]
    [SerializeField] float _accelAmount;
    [SerializeField] float _derceAmount;
    [SerializeField] float _frictionAmount;

    //������
    public float AccelForce { get; set; }
    public float DecreForce { get; set; }
    public float FrictionForce { get; set; }

    [Header("�����еļ��ٶȱ���")]
    [Range(0.01f, 1)] public float AccelInAir;
    [Range(0.01f, 1)] public float DecreInAir;

    [Header("��Ծ����")]
    public float JumpForce;

    [Header("��ߵ�����")]
    //�ڽӽ���Ծ���㣨���������߶ȣ�ʱ��������
    [Range(0f, 1)] public float JumpHangGravityMult;
    //�ٶȣ��ӽ�0������ҽ����鵽����ġ���Ծ���ҡ�����ҵ��ٶȡ�Y����Ծ������ӽ�0
    public float JumpHangTimeThreshold;
    [Space(0.5f)]
    public float JumpHangAccelerationMult;
    public float JumpHangMaxSpeedMult;

    [Header("Assists")]
    //���µ������Ȼ������Ծ��ʱ��
    [Range(0.01f, 0.5f)] public float coyoteTime;


    private void OnValidate()
    {
        //��Ծ
        //ʹ�ù�ʽ��������ǿ�ȣ�gravity = 2 * jumpHeight / timeToJumpApex^2��
        //GravityStrength = -(2 * JumpHeight) / (JumpTimeToApex * JumpTimeToApex);
        ////�������������߶ȣ���������ǿ������ڵ�λ������ֵ���μ�project settings/Physics2D��
        //GravityScale = GravityStrength / Physics2D.gravity.y;
        ////ʹ�ù�ʽ��initialJumpVelocity = gravity * timeToJumpApex������jumpForce
        //JumpForce = Mathf.Abs(GravityScale) * JumpTimeToApex;
        


        //�ƶ�
        //ʹ�ù�ʽ�������м��ٺͼ�������amount = ��(1 / Time.fixedDeltaTime) *���ٶȣ�/ runMaxSpeed
        AccelForce = (1/Time.fixedDeltaTime * _accelAmount) / MaxMoveSpeed;
        DecreForce = (1 / Time.fixedDeltaTime * _derceAmount) / MaxMoveSpeed;
        FrictionForce=(1/Time.fixedDeltaTime*_frictionAmount) / MaxMoveSpeed;

        #region Variable Ranges
        _accelAmount = Mathf.Clamp(_accelAmount, 0.01f, MaxMoveSpeed);
        _derceAmount = Mathf.Clamp(_derceAmount, 0.01f, MaxMoveSpeed);
        _frictionAmount= Mathf.Clamp(_frictionAmount,0.01f, MaxMoveSpeed);
        #endregion
    }
}
