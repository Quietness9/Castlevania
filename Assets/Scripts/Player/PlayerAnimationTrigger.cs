using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    Player _player=>GetComponentInParent<Player>();

    public void AnimationFinish()
    {
        _player.CurrentAnimationFinish();
    }
}
