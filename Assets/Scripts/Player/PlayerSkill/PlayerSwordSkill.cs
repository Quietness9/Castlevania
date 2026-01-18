using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkill : Skill
{
    
    public SwordData PlayerSwordData;

    public bool IsLock;
    public bool IsLockTimeStop;
    public bool IsLockSwordPower;
    public SwordType SwordType=SwordType.Ordinary;


    [SerializeField] Transform _dotsParent;
    GameObject[] _dots;
    Vector2 _finalDir;

    protected override void Start()
    {
        base.Start();
        CreateDots();
    }


    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _finalDir = GetAimDirection().normalized;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < _dots.Length; ++i)
            {
                _dots[i].transform.position = GetDotsPosition((i+1) * PlayerSwordData.SpaceBetweenDots);
            }
        }

    }

    /// <summary>
    /// 创建剑
    /// </summary>
    public void CreateSword()
    {
        GameObject swordPre = GlobalReferencesMgr.Instance.GetPrefab("PlayerSword");
        if (swordPre == null)
            return;

        GameObject swordObj = Instantiate(swordPre, player.transform.position, Quaternion.identity);

        if(swordObj==null)
        {
            Debug.Log("swordObj is Null");
            return;
        }

        if(swordObj.TryGetComponent(out SwordCtr newSword))
        {
            newSword.SetSwordData(PlayerSwordData.SwordForce * _finalDir,player,this);
            player.GetNewSword(swordObj);
        }

        SetDotsActive(false);
    }

    /// <summary>
    /// 获得瞄准方向
    /// </summary>
    /// <returns></returns>
    private Vector2 GetAimDirection()
    {
        Vector2 playerPosition=player.transform.position;
        Vector2 mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction=mousePosition - playerPosition;

        return direction;
    }

    #region 瞄准点功能

    /// <summary>
    /// 激活瞄准点
    /// </summary>
    public void ActiveDots()
    {
        SetDotsActive(true);
    }

    /// <summary>
    /// 隐藏瞄准点
    /// </summary>
    public void HideDots()
    {
        SetDotsActive(false);
    }

    /// <summary>
    /// 控制瞄准点显示
    /// </summary>
    /// <param name="isActive"></param>
    private void SetDotsActive(bool isActive)
    {
        foreach (var item in _dots)
        {
            item.SetActive(isActive);
        }
    }

    /// <summary>
    /// 创建瞄准点
    /// </summary>
    private void CreateDots()
    {
        GameObject dotPrefab = GlobalReferencesMgr.Instance.GetPrefab("SwordDot");

        if (dotPrefab==null)
            return;

        _dots = new GameObject[PlayerSwordData.DotsCount];

        for (int i = 0; i < PlayerSwordData.DotsCount; i++)
        {
            _dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, _dotsParent);
            _dots[i].SetActive(false);
        }
    }

    /// <summary>
    /// 获得Dots位置
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private Vector2 GetDotsPosition(float time)
    {
        Vector2 position=(Vector2)player.transform.position +new Vector2(
            GetAimDirection().normalized.x*PlayerSwordData.SwordForce.x,
            GetAimDirection().normalized.y*PlayerSwordData.SwordForce.y)*time
            +0.5f*(Physics2D.gravity* PlayerSwordData.getSwordGravity(SwordType)) *(time*time);

        return position;
    }

    #endregion

    #region 技能解锁

    /// <summary>
    /// 解锁技能
    /// </summary>
    public void UnLockSword() => IsLock = true;

    /// <summary>
    /// 解锁定身
    /// </summary>
    public void UnLockTimeStop()=>IsLockTimeStop = true;

    /// <summary>
    /// 解锁减少护甲
    /// </summary>
    public void UnLockVulnerability()=>IsLockSwordPower = true;

    /// <summary>
    /// 解锁剑为弹跳模式
    /// </summary>
    public void UnLockBounceMode()=>SwordType=SwordType.Bounce;

    /// <summary>
    /// 解锁剑为穿透模式
    /// </summary>
    public void UnLockPierceMode()=>SwordType=SwordType.Pierce;

    /// <summary>
    /// 解锁剑为旋转模式
    /// </summary>
    public void UnLockSpinMode() => SwordType = SwordType.Spin;

    #endregion
}
