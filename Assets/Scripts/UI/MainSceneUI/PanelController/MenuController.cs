using GameInputSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuController : MonoSingleton<MenuController>
{
    [Header("提示UI")]
    public ItemTipController ItemTip;
    public TipController AttributeTip;
    public TipController SkillTip;

    [Header("主要角色面板UI")]
    public CharacterUIController CharacterUI;
    public CraftUlController CraftUI;


    [SerializeField] Transform _allMenuParent;
    [SerializeField] GameObject _bg;
    [SerializeField] GameObject _menuHeader;

    [SerializeField] List<GameObject> _allMenus=new();

    PlayerInputReader playerInput;

    private void Start()
    {
        InitMenu();
    }

    #region 菜单控制

    /// <summary>
    /// 切换菜单界面
    /// </summary>
    /// <param name="menu"></param>
    public void SwitchMenu(GameObject menu)
    {
        if (menu == null)
            return;

        if (!_allMenuParent.gameObject.activeSelf && !_bg.activeSelf && !_menuHeader.activeSelf)
        {
            SetMenuActive(true);
        }

        if (menu.activeSelf)
        {
            CloseAllWindow();
            CloseMenu();
        }
        else
        {
            CloseAllWindow();
            menu.SetActive(true);
        }
        
    }

    /// <summary>
    /// 关闭菜单
    /// </summary>
    public void CloseMenu()
    {
        CharacterUI.UpdateEquipmentSlotData();
        SetMenuActive(false);
    }

    /// <summary>
    /// 设置菜单激活状态
    /// </summary>
    /// <param name="active"></param>
    private void SetMenuActive(bool active)
    {
        _bg.SetActive(active);
        _menuHeader.SetActive(active);
        _allMenuParent.gameObject.SetActive(active);
    }

    /// <summary>
    /// 关闭所有窗口
    /// </summary>
    private void CloseAllWindow()
    {
        for (int i = 0; i < _allMenus.Count; i++)
        {
            _allMenus[i].SetActive(false);
        }
    }

    /// <summary>
    /// 初始化菜单
    /// </summary>
    private void InitMenu()
    {

        for (int i = 0; i < _allMenuParent.childCount; i++)
        {
            GameObject menuObj = _allMenuParent.GetChild(i).gameObject;
            if (menuObj != null)
            {
                _allMenus.Add(menuObj);
                menuObj.SetActive(true);
                menuObj.SetActive(false);
            }
        }

        SetMenuActive(false);

        playerInput =GlobalReferencesManager.Instance.GamePlayer.PlayerInput;

        if(playerInput != null)
        {
            playerInput.OnCharacterUIEvent += ActiveCharacterUI;
            playerInput.OnSkillTreeUIEvent += ActiveSkillTreeUI;
            playerInput.OnCraftUIEvent += ActiveCraftUI;
            playerInput.OnOptionUIEvent+=ActiveOptionUI;
        }
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (playerInput != null)
        {
            playerInput.OnCharacterUIEvent -= ActiveCharacterUI;
            playerInput.OnSkillTreeUIEvent -= ActiveSkillTreeUI;
            playerInput.OnCraftUIEvent -= ActiveCraftUI;
            playerInput.OnOptionUIEvent -= ActiveOptionUI;
        }
    }

    #region 激活UI 

    private void ActiveCharacterUI() => SwitchMenu(_allMenus[0]);

    private void ActiveSkillTreeUI()=>SwitchMenu(_allMenus[1]);

    private void ActiveCraftUI()=>SwitchMenu(_allMenus[2]);

    private void ActiveOptionUI() => SwitchMenu(_allMenus[3]);

    #endregion


    #endregion




}
