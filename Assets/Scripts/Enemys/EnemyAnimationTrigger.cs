using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAnimationTrigger : MonoBehaviour
{
    Enemy _enemy=>GetComponentInParent<Enemy>();

    /// <summary>
    /// 动画完成回调
    /// </summary>
   private void AnimationFinish()
    {
        _enemy.CurrentAnimationFinish();
    }

    private void AttackAnimationFinish()
    {

    }

    private void OpenCounterWindow() { }
    private void CloseCounterWindow() { }
}
