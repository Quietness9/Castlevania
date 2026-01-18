using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTipCtr : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tipName;
    [SerializeField] TextMeshProUGUI _itemType;
    [SerializeField] TextMeshProUGUI _tipContent;

    [Header("偏移调整")]
    [SerializeField] float _xLimit = 960;
    [SerializeField] float _yLimit = 540;
    [SerializeField] float _xOffset = 150;
    [SerializeField] float _yOffset = 150;

    /// <summary>
    /// 显示介绍
    /// </summary>
    /// <param name="itemData"></param>
    public void ShowTipContent(ItemData itemData,bool isAdjustPosition=false)
    {
        if(itemData == null) 
            return;

        _tipName.text = itemData.Name;
        _itemType.text = itemData.Type.ToString();
        
        if (itemData.Type == ItemType.Equipment)
        {
            _itemType.text=(itemData as EquipmentItemData).EquipmentType.ToString();
        }

        _tipContent.text=itemData.GetDescription();

        _tipName.fontSize =32;
        if (_tipName.text.Length > 7)
        {
            _tipName.fontSize = _tipName.fontSize * 0.7f;
        }

        if (isAdjustPosition)
        {
            AdjustPosition();
        }
        
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏介绍
    /// </summary>
    public void HideTipContent()=>gameObject.SetActive(false);


    /// <summary>
    /// 调整提示位置
    /// </summary>
    private void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        //以下功能为提示跟随鼠标
        float offsetX = 0;
        float offsetY = 0;

        offsetX = _xOffset;
        if (mousePosition.x > _xLimit)
        {
            offsetX = -_xOffset;
        }

        offsetY = _yOffset;
        if (offsetY > _yLimit)
        {
            offsetY = -_yOffset;
        }

        transform.position = new Vector3(mousePosition.x + offsetX, mousePosition.y + offsetY, transform.position.z);
    }
}
