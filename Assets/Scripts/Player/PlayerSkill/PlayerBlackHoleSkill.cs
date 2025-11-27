using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleSkill : Skill
{

    public BlackHoleData PlayerBlackHoleData;
    public bool IsStart { get;private set; }
    public bool IsEnd { get;private set; }

    protected override void Start()
    {
        base.Start();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.BlackHoleEvent += BlackHoleSkillHandle;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(player.PlayerInput != null)
        {
            player.PlayerInput.BlackHoleEvent -= BlackHoleSkillHandle;
        }
    }

    /// <summary>
    /// 创建黑洞
    /// </summary>
    public void CreateBlackHole()
    {
        GameObject blackHoleObj = Instantiate(GlobalReferencesManager.Instance.GetPrefab("BlackHole"),
            player.transform.position,Quaternion.identity);

        blackHoleObj.GetComponent<BlackHoleController>().SetBlackHoleData(PlayerBlackHoleData);
    }

    /// <summary>
    /// 设置黑洞状态
    /// </summary>
    /// <param name="isStart"></param>
    /// <param name="isEnd"></param>
    public void SetBlackHoleState(bool isStart,bool isEnd)
    {
        IsStart=isStart;
        IsEnd=isEnd;
    }

    /// <summary>
    /// 黑洞技能回调
    /// </summary>
    private void BlackHoleSkillHandle()
    {
        if (CanUseSkill())
        {
            SetBlackHoleState(true,false);
            player.CharacterStateMachine.ChangeState(player.BlackHoleState);
        }
    }
}
