using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoSingleton<MenuController>
{

    [SerializeField] Transform _allMenu;
    [SerializeField] GameObject _bg;
    [SerializeField] GameObject _menuHeader;

    [Header("武器栏UI")]
    [SerializeField] Transform _equipmentMenuSlotParent;
    [SerializeField] Transform _equipmentItemSlotParent;
    List<GameObject> _equipmentItemSlots = new();
    EquipmentSlotController[] _equipmentSlotControllers;



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
        _bg.SetActive(false);
        _menuHeader.SetActive(false);
        _allMenu.gameObject.SetActive(false);

        UpdateEquipmentSlotData();
    }

    /// <summary>
    /// 初始化菜单
    /// </summary>
    private void InitMenu()
    {
        for (int i = 0; i < _equipmentItemSlotParent.childCount; i++)
        {
            _equipmentItemSlots.Add(_equipmentItemSlotParent.GetChild(i).gameObject);
        }

        _equipmentSlotControllers = _equipmentMenuSlotParent.GetComponentsInChildren<EquipmentSlotController>();
    }

    #endregion

    /// <summary>
    /// 装备各种武器
    /// </summary>
    /// <param name="type">装备类型</param>
    public void EquipWeapons(InventoryItem inventoryItem)
    {

        EquipmentItemData equipmentItemData = (EquipmentItemData)inventoryItem.ItemData;

        switch (equipmentItemData.EquipmentType)
        {
            case EquipmentItemType.Weapon:EquipWeaponHelp(0,inventoryItem); break;
            case EquipmentItemType.Armor: EquipWeaponHelp(1, inventoryItem); break;
            case EquipmentItemType.Amulet: EquipWeaponHelp(2, inventoryItem); break;
            case EquipmentItemType.Flask: EquipWeaponHelp(3, inventoryItem); break;
        }
        
    }

    /// <summary>
    /// 装备函数的辅助函数
    /// </summary>
    /// <param name="index"></param>
    /// <param name="inventoryItem"></param>
    private void EquipWeaponHelp(int index, InventoryItem inventoryItem)
    {
        if (_equipmentSlotControllers[index].EqInventoryItem != null)
            return;

        _equipmentSlotControllers[index].SetEquipmentSlotData(inventoryItem);
        InventoryController.Instance.RemoveItem(inventoryItem.ItemData);
    }

    /// <summary>
    /// 刷新展示武器UI数据
    /// </summary>
    private void UpdateEquipmentSlotData()
    {
        for (int i = 0; i < _equipmentMenuSlotParent.childCount; i++)
        {

            if (_equipmentSlotControllers[i].EqInventoryItem != null)
            {
                _equipmentItemSlots[i].GetComponent<Image>().sprite = _equipmentSlotControllers[i].EqInventoryItem.ItemData.ShowIcon;
            }

        }
    }
}
