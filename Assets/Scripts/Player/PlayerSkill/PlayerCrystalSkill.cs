using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCrystalSkill : Skill
{
    public CrystalData PlayerCrystalData;

    [SerializeField] bool _isCanExplode;
    [SerializeField] bool _isSwapPositions;
    [SerializeField] bool _isCanMove;
    [SerializeField] bool _isUseMultCrystal;

    CrystalController _crystalController;
    [SerializeField] List<GameObject> _crystalObj = new();


    protected override void Start()
    {
        base.Start();
        if (player.PlayerInput != null)
        {
            player.PlayerInput.CrystalEvent += UseSkill;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(player.PlayerInput != null)
        {
            player.PlayerInput.CrystalEvent-= UseSkill;
        }
    }

    /// <summary>
    /// 创建水晶
    /// </summary>
    public void CreateCrystal()
    {
        GameObject crystalPre = GlobalReferencesManager.Instance.GetPrefab("Crystal");
        if (crystalPre == null)
            return;

        //创建多个水晶
        if (_isUseMultCrystal)
        {
            FillCrystal(crystalPre);
            return;
        }

        //创建多个水晶 
        GameObject crystalObj = Instantiate(crystalPre,player.transform.position,Quaternion.identity);
        _crystalController = crystalObj.GetComponent<CrystalController>();

        _crystalController.SetCrystalData(PlayerCrystalData,player);

        if (_isCanExplode && _isCanMove)
        {
            _crystalController.CrystalMoveToEnemy(GetClosestEnemy(_crystalController.transform, PlayerCrystalData.CheckRadius));
        }
    }

    public override void UseSkill()
    {

        //单个水晶时触发
        if (_crystalController != null)
        {
            //在此触发按键时执行

            if (_isSwapPositions)
            {
                _crystalController.PlayerReturnCrystalPosition();
            }

            if (_isSwapPositions&&_isCanExplode)
            {
                _crystalController.PlayerSwapCrystalPosition();
                _crystalController.CrystalExplosion();
            }

            if(_isCanExplode&& _isCanMove)
            {
                _crystalController.CrystalExplosion();
            }
        }

        //拥有多个水晶
        if (_crystalObj != null && _crystalObj.Count > 0)
        {

            GameObject crystalObj = GetCanUseCrystal();
            crystalObj.transform.position=player.transform.position;
            Transform enemyTransform = GetClosestEnemy(crystalObj.transform, PlayerCrystalData.CheckRadius);

            if(enemyTransform != null)
            {
                crystalObj.SetActive(true);
                crystalObj.GetComponent<CrystalController>().CrystalMoveToEnemy(enemyTransform);
            }

            
        }

        if (CanUseSkill()&&_crystalController==null)
        {
            CreateCrystal();
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

        return crystalObj;
    }

    /// <summary>
    /// 填充满水晶
    /// </summary>
    private void FillCrystal(GameObject crystalPre)
    {
        for(int i = 0; i < PlayerCrystalData.MaxSpawnCrystalAmount; i++)
        {
            GameObject crystalObj = Instantiate(crystalPre);
            crystalObj.GetComponent<CrystalController>().SetCrystalData(PlayerCrystalData, player);
            crystalObj.SetActive(false);
            _crystalObj.Add(crystalObj);
        }
    }
}
