using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCrystalSkill : Skill
{
    public CrystalData PlayerCrystalData;

    //通过技能树解锁
    public bool IsLock;

    public bool IsCanMove;
    public bool IsCanExplode;
    public bool IsSwapPosition;
    public bool IsUseMulCrystal;
    public bool IsCrystalReplaceClone;

    public bool ControllerOneCrystal { get; set; }//控制单个水晶

    bool _isOpenWindow;

    List<GameObject> _crystalObj = new();


    protected override void Start()
    {
        base.Start();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.OnCrystalEvent += UseSkill;
            player.PlayerInput.OnCrystalEvent += MulCrystalLogicHandle;
        }

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(player.PlayerInput != null)
        {
            player.PlayerInput.OnCrystalEvent-= UseSkill;
            player.PlayerInput.OnCrystalEvent -= MulCrystalLogicHandle;
        }
    }

    /// <summary>
    /// 创建水晶
    /// </summary>
    public GameObject CreateCrystal(Transform transform,Quaternion quaternion=default)
    {
        GameObject crystalPre = GlobalReferencesManager.Instance.GetPrefab("Crystal");
        if (crystalPre == null)
        {
            Debug.LogWarning("预制体为null");
            return null;
        }

        GameObject crystalObj = Instantiate(crystalPre,transform.position, quaternion);
        CrystalController crystalController = crystalObj.GetComponent<CrystalController>();

        crystalController.SetCrystalData(player,this);

        //if (IsCanExplode && IsCanMove)
        //{
        //    crystalController.SetClosestEnemy(GetClosestEnemy(player.transform, PlayerCrystalData.CheckRadius));
        //}

        return crystalObj;
    }

    /// <summary>
    /// 创建单个控制水晶
    /// </summary>
    public void CreateOneCrystal()
    {
        GameObject crystalObj = CreateCrystal(player.transform);
        if (crystalObj == null)
            return;

        if(IsCanExplode && IsCanMove)
        {
            Transform closestEnemy = GetClosestEnemy(player.transform, PlayerCrystalData.CheckRadius);
            crystalObj.GetComponent<CrystalController>().SetClosestEnemy(closestEnemy);
        }
    }

    public override void UseSkill()
    {

        if (IsLock&&CanUseSkill())
        {
            
            //CreateCrystal();
            if (IsUseMulCrystal)
            {
                CreateMulCrystal();
            }
            else
            {
                ControllerOneCrystal = true;
                CreateOneCrystal();
            }
        }
    }

    /// <summary>
    /// 多个水晶控制
    /// </summary>
    private void MulCrystalLogicHandle()
    {
        
        if (!_isOpenWindow)
            return;

        if (_crystalObj != null && _crystalObj.Count > 0)
        {
            if (_crystalObj.Count == PlayerCrystalData.MaxSpawnCrystalAmount)
            {
                Invoke("CloseCrystalUse", PlayerCrystalData.MulCrystalWindowTime);
            }
            
            Transform enemyTransform = GetClosestEnemy(player.transform, PlayerCrystalData.CheckRadius);

            if (enemyTransform != null)
            {
                GameObject crystalObj = GetCanUseCrystal();
                crystalObj.transform.position = player.transform.position;
                crystalObj.SetActive(true);
                crystalObj.GetComponent<CrystalController>().SetClosestEnemy(enemyTransform);
            }

        }
    }

    /// <summary>
    /// 减少并获得可以使用的水晶对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetCanUseCrystal()
    {
        GameObject crystalObj=_crystalObj[_crystalObj.Count-1];
        _crystalObj.Remove(crystalObj);

        if (_crystalObj.Count == 0)
        {
            ControllerOneCrystal=false;
        }

        return crystalObj;
    }

    /// <summary>
    /// 创建或填满水晶
    /// </summary>
    private void CreateMulCrystal()
    {
        int amount= PlayerCrystalData.MaxSpawnCrystalAmount-_crystalObj.Count;
        _isOpenWindow = true;

        for (int i = 0; i < amount; i++)
        {
            GameObject crystalObj = CreateCrystal(player.transform);
            crystalObj.SetActive(false);
            _crystalObj.Add(crystalObj);
        }
    }

    /// <summary>
    /// 关闭水晶使用
    /// </summary>
    private void CloseCrystalUse()
    {
        _isOpenWindow = false;
    }

    #region 技能解锁

    /// <summary>
    /// 解锁技能
    /// </summary>
    public void UnLockCrystal() => IsLock = true;

    /// <summary>
    /// 解锁水晶移动
    /// </summary>
    public void UnLockCrystalMove() => IsCanMove = true;

    /// <summary>
    /// 解锁水晶爆炸
    /// </summary>
    public void UnLockCrystalExplode()=>IsCanExplode = true;

    /// <summary>
    /// 解锁水晶交换位置
    /// </summary>
    public void UnLockCrystalSwapPosition()=>IsSwapPosition = true;

    /// <summary>
    /// 解锁可以使用多个水晶
    /// </summary>
    public void UnLockUseMulCrystal()=>IsUseMulCrystal = true;

    /// <summary>
    /// 解锁增加闪避率
    /// </summary>
    public void UnLockIncreaseEvasion()
    {
        int value=Mathf.RoundToInt(player.Attribute.Evasion.GetValue()*PlayerCrystalData.EvasionRatio);
        player.Attribute.Evasion.AddModifier(value);
    }

    /// <summary>
    /// 解锁使用克隆体代替水晶
    /// </summary>
    public void UnLockCrystalReplaceClone()=>IsCrystalReplaceClone=true;


    #endregion
}
