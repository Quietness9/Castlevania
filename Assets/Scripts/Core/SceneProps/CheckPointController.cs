using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    Animator _animator;
    public bool IsActive;
    public string CheckPointId;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive)
            return;
        
        if (collision.GetComponent<Player>() != null)
        {
            ActiveCheckPoint();
        }
    }

    /// <summary>
    /// 激活检查点
    /// </summary>
    public void ActiveCheckPoint()
    {
        GameManager.Instance.ActiveCheckPoint = this;

        IsActive = true;
        _animator.SetBool("Active", true);
    }

    /// <summary>
    /// 创建检查点Id
    /// </summary>
    [ContextMenu("Generate CheckPoint Id")]
    private void GenerateId()
    {
        CheckPointId=System.Guid.NewGuid().ToString();
    }
}
