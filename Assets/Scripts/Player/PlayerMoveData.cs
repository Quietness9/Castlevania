using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Move Data",menuName = "PlayerData/MoveData")]
public class PlayerMoveData : ScriptableObject
{
    //重力
    //向下的力（重力）需要期望的jumpHeight和jumpTimeToApex。
    public float GravityStrength { get; set; }
    public float GravityScale { get; set; }

    [Header("下落参数")]
    //自由下落
    public float FallGravityMult;
    public float MaxFallSpeed;
    //按住下键
    public float FastFallGravityMult;
    public float MaxFastFallSpeed;


    [Space(4)]
    [Header("移动参数")]
    public float MaxMoveSpeed;
    public bool doConserveMomentum;

    [Header("加速度强度系数")]
    [SerializeField] float _accelAmount;
    [SerializeField] float _derceAmount;
    [SerializeField] float _frictionAmount;

    //作用力
    public float AccelForce { get; set; }
    public float DecreForce { get; set; }
    public float FrictionForce { get; set; }

    [Header("空气中的加速度比例")]
    [Range(0.01f, 1)] public float AccelInAir;
    [Range(0.01f, 1)] public float DecreInAir;

    [Header("跳跃参数")]
    public float JumpHeight;
    //跳跃到指定高度需要的时间
    public float JumpTimeToApex;
    public float JumpForce { get; set; }

    [Header("蓄力跳跃")]
    public float JumpCutGravityMult;
    //在接近跳跃顶点（期望的最大高度）时减少重力
    [Range(0f, 1)] public float JumpHangGravityMult;
    //速度（接近0），玩家将体验到额外的“跳跃悬挂”。玩家的速度。Y在跳跃顶点最接近0
    public float JumpHangTimeThreshold;
    [Space(0.5f)]
    public float JumpHangAccelerationMult;
    public float JumpHangMaxSpeedMult;

    //[Header("跳墙")]
    //public Vector2 WallJumpForce;

    [Header("Assists")]
    //落下地面后依然可以跳跃的时间
    [Range(0.01f, 0.5f)] public float coyoteTime;
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime;


    private void OnValidate()
    {
        //跳跃
        //使用公式计算重力强度（gravity = 2 * jumpHeight / timeToJumpApex^2）
        GravityStrength = -(2 * JumpHeight) / (JumpTimeToApex * JumpTimeToApex);

        //计算刚体的重力尺度（即：重力强度相对于单位的重力值，参见project settings/Physics2D）
        GravityScale = GravityStrength / Physics2D.gravity.y;

        //使用公式（initialJumpVelocity = gravity * timeToJumpApex）计算jumpForce
        JumpForce = Mathf.Abs(GravityStrength) * JumpTimeToApex;


        //移动
        //使用公式计算运行加速和减速力：amount = （(1 / Time.fixedDeltaTime) *加速度）/ runMaxSpeed
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
