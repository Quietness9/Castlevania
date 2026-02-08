using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    Slider _slider;
    Character _character;
    RectTransform _rectTransform;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _rectTransform = GetComponent<RectTransform>();
        _slider=GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        if (_character != null)
        {
            _character.OnFlipEvent += AvoidHealthUIFlipHandle;
            _character.Attribute.OnChangeHealthEvent += UpdateHealthUIHandle;
        }
    }

    private void Start()
    {
        UpdateHealthUIHandle();
    }

    private void OnDisable()
    {
        if (_character != null)
        {
            _character.OnFlipEvent -= AvoidHealthUIFlipHandle;
            _character.Attribute.OnChangeHealthEvent -= UpdateHealthUIHandle;
        }
    }

    /// <summary>
    /// 设置血量是否显示
    /// </summary>
    public void SetSliderActive()
    {
        if (_slider.IsActive())
        {
            _slider.gameObject.SetActive(false);
        }
        else
        {
            _slider.gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// 更新生命值UI
    /// </summary>
    private void UpdateHealthUIHandle()
    {
        _slider.maxValue=_character.Attribute.GetMaxHealth();
        _slider.value = _character.Attribute.CurrentHealth;
    }

    /// <summary>
    /// 避免生命条UI跟随角色反转
    /// </summary>
    private void AvoidHealthUIFlipHandle()
    {
        _rectTransform.Rotate(0, 180, 0);
    }


}
