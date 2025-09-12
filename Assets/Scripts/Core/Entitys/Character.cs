using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Character : MonoBehaviour
{


    [SerializeField] protected Transform _groundCheckPoint;
    [SerializeField] Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] LayerMask _groundLayer;

    public bool IsFacingRight { get;set; }
    public StateMachine CharacterStateMachine { get; private set; }

    #region ���
    public Animator Animator_CT { get;set; }
    public Rigidbody2D Rb { get;set; }

    #endregion

    protected virtual void Awake()
    {
        Animator_CT = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        CharacterStateMachine = new StateMachine();
    }

    /// <summary>
    /// �ı䳯��
    /// </summary>
    public virtual void TurnDirection()
    {
        Vector3 scale=transform.localScale;

        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight=!IsFacingRight;
    }

    /// <summary>
    /// ������Ĭ�Ϸ���true
    /// </summary>
    /// <returns></returns>
    public virtual bool IsGroundCheck() => Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);

    /// <summary>
    /// ���Ƽ����
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawCube(_groundCheckPoint.position, _groundCheckSize);
    }
}
