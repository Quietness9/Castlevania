using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoSingleton<MenuController>
{
    public CharacterUIController CharacterUI;

    [SerializeField] Transform _allMenu;
    [SerializeField] GameObject _bg;
    [SerializeField] GameObject _menuHeader;

    GameObject _activeMenu;

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
        for (int i = 0; i < _allMenu.childCount; i++)
        {
            _allMenu.GetChild(i).gameObject.SetActive(false);
        }

        if (menu == _activeMenu)
        {
            CloseMenu();
            return;
        }

        if (menu != null)
        {
            menu.SetActive(true);
            _activeMenu = menu;
        }
    }

    /// <summary>
    /// 关闭菜单
    /// </summary>
    public void CloseMenu()
    {
        CharacterUI.UpdateEquipmentSlotData();


        _bg.SetActive(false);
        _menuHeader.SetActive(false);
        _allMenu.gameObject.SetActive(false);

    }

    /// <summary>
    /// 初始化菜单
    /// </summary>
    private void InitMenu()
    {
        for(int i=0;i< _allMenu.childCount; i++)
        {
            _allMenu.GetChild(i).gameObject.SetActive(false);
        }

    }

    #endregion

    

    
}
