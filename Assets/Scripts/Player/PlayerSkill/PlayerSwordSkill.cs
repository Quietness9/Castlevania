using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkill : Skill
{
    [Header("SwordInfo")]
    [SerializeField] Vector2 _swordForce;
    [SerializeField] float _swordGravity;
    [SerializeField] Vector3 _offset;

    [Header("Aim Dots Info")]
    [SerializeField] int _dotsCount;
    [SerializeField] float _spaceBetweenDots;
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
                _dots[i].transform.position = GetDotsPosition((i+1) * _spaceBetweenDots);
            }
        }

    }

    /// <summary>
    /// 创建剑
    /// </summary>
    public void CreateSword()
    {

        GameObject swordObj = Instantiate(GlobalReferencesManager.Instance.GetPrefab("PlayerSword"), player.transform.position+_offset, Quaternion.identity);

        if(!swordObj)
        {
            Debug.Log("swordObj is Null");
            return;
        }
        SwordController newSword = swordObj.GetComponent<SwordController>();
        newSword.SetSword(_swordForce *_finalDir , _swordGravity);
        SetDotsActive(false);

    }

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
        foreach(var item in _dots)
        {
            item.SetActive(isActive);
        }
    }

    /// <summary>
    /// 创建瞄准点
    /// </summary>
    private void CreateDots()
    {
        GameObject dotPrefab = GlobalReferencesManager.Instance.GetPrefab("SwordDot");

        if (!dotPrefab)
        {
            Debug.Log("dotPrefab is NULL");
            return;
        }

        _dots = new GameObject[_dotsCount];

        for(int i = 0; i < _dotsCount; i++)
        {
            _dots[i]=Instantiate(dotPrefab,player.transform.position, Quaternion.identity,_dotsParent);
            _dots[i].SetActive(false);
        }
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

    /// <summary>
    /// 获得Dots位置
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private Vector2 GetDotsPosition(float time)
    {
        Vector2 position=(Vector2)player.transform.position +new Vector2(
            GetAimDirection().normalized.x*_swordForce.x,
            GetAimDirection().normalized.y*_swordForce.y)*time
            +0.5f*(Physics2D.gravity*_swordGravity)*(time*time);

        return position;
    }
}
