using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleSkill : Skill
{

    public BlackHoleData PlayerBlackHoleData;
    public bool IsStart { get;private set; }
    public bool IsEnd { get;private set; }

    public  bool IsCreateCrystal;

    protected override void Start()
    {
        base.Start();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.BlackHoleEvent += UseSkill;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(player.PlayerInput != null)
        {
            player.PlayerInput.BlackHoleEvent -= UseSkill;
        }
    }

    /// <summary>
    /// 创建黑洞
    /// </summary>
    public void CreateBlackHole()
    {

        GameObject blackHolePre = GlobalReferencesManager.Instance.GetPrefab("BlackHole");
        if (blackHolePre == null)
            return;

        GameObject blackHoleObj = Instantiate(blackHolePre,player.transform.position,Quaternion.identity);

        blackHoleObj.GetComponent<BlackHoleController>().SetBlackHoleData(player,this);
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

    public override void UseSkill()
    {
        if (CanUseSkill())
        {
            SetBlackHoleState(true, false);
            player.CharacterStateMachine.ChangeState(player.BlackHoleState);
        }
        else
        {
            Debug.Log("技能在冷却");
        }
    }
}
