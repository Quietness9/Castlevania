using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttributeSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string _attributeName;
    [SerializeField] AttributeType _type;
    [SerializeField] TextMeshProUGUI _attributeValueText;
    [SerializeField] TextMeshProUGUI _attributeNameText;

    [TextArea]
    [SerializeField] string _attributeDescription;

    PlayerAttribute _playerAttribute;


    private void OnValidate()
    {
        gameObject.name = _attributeName;

        if (_attributeNameText != null)
        {
            _attributeNameText.text = _attributeName;
        }
    }

    private void Start()
    {
        _playerAttribute = GlobalReferencesManager.Instance.GamePlayer.Attribute as PlayerAttribute;
        if (_playerAttribute != null)
        {
            _playerAttribute.OnAttributeSlotEvent += UpdateSlotValue;
        }

        InitAttributeSlot();
    }

    private void OnDestroy()
    {
        if (_playerAttribute != null)
        {
            _playerAttribute.OnAttributeSlotEvent -= UpdateSlotValue;
        }
    }

    /// <summary>
    /// 初始化属性值
    /// </summary>
    private void InitAttributeSlot()
    {

        if (_playerAttribute != null)
        {
            UpdateSlotValue();
        }
    }

    /// <summary>
    /// 更新属性值
    /// </summary>
    private void UpdateSlotValue()
    {

        if (_playerAttribute != null)
        {

            int allValue = -1;
            //特殊属性处理
            switch (_type)
            {
                case AttributeType.Hp: allValue = _playerAttribute.GetMaxHealth(); break;
                case AttributeType.Evasion: allValue = _playerAttribute.GetTotalEvasion(); break;
                case AttributeType.Atk: allValue = _playerAttribute.GetTotalAtk(); break;
                case AttributeType.CriticalChance: allValue = _playerAttribute.GetTotalCriticalChance(); break;
                case AttributeType.CriticalDamage: allValue = _playerAttribute.GetTotalCriticalDamage(); break;
                case AttributeType.MagicResistance: allValue = _playerAttribute.GetTotalMagicResistance(); break;
            }

            _attributeValueText.text = "" + _playerAttribute.GetAttribute(_type).GetValue();
            if (allValue != -1)
            {
                _attributeValueText.text = "" + allValue;
            }


        }
    }

    /// <summary>
    /// 鼠标划入属性值
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuController.Instance.Tip.ShowTip(_attributeDescription);
    }

    /// <summary>
    /// 鼠标划出属性值
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        MenuController.Instance.Tip.HideTip();
    }
}
