using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTipController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tipName;
    [SerializeField] TextMeshProUGUI _itemType;
    [SerializeField] TextMeshProUGUI _tipContent;

    /// <summary>
    /// œ‘ æΩÈ…‹
    /// </summary>
    /// <param name="itemData"></param>
    public void ShowTipContent(ItemData itemData)
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
        
        gameObject.SetActive(true);
    }

    /// <summary>
    /// “˛≤ÿΩÈ…‹
    /// </summary>
    public void HideTipContent()=>gameObject.SetActive(false);
}
