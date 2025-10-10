using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    Rigidbody2D _rb;
    Animator _animator;
    CircleCollider2D _circleCollider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _animator = GetComponentInChildren<Animator>();
    }


    public void SetSword(Vector2 force,float gravity)
    {
        _rb.gravityScale = gravity;
        _rb.AddForce(force,ForceMode2D.Impulse);
    }
}
