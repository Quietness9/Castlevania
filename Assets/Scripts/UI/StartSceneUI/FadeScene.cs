using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScene : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// µ­³öÆÁÄ»
    /// </summary>
    public void FadeOutScene() => _animator.SetTrigger("FadeOut");

    /// <summary>
    /// µ­ÈëÆÁÄ»
    /// </summary>
    public void FadeInScene() => _animator.SetTrigger("FadeIn");
}
