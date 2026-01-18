using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFireCtr : EffectCtr
{
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb=GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// …Ë÷√“∆∂Ø¡¶
    /// </summary>
    /// <param name="force"></param>
    public void SetForce(Vector2 force)
    {
        if (_rb == null)
            return;

        _rb.velocity = force;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
