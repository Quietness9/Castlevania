using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    public bool UnLock;


    [SerializeField] int _skillPrice;
    [SerializeField] string _slotName;
    [SerializeField] Image _slotImage;
    [TextArea]
    [SerializeField] string _skillDescription;
    [SerializeField] Color _lockColor;

    [Space]
    [SerializeField] UnityEvent _onSkillUnLockEvent = new();
    [SerializeField] SkillSlotController[] _unLockSlots;
    [SerializeField] SkillSlotController[] _lockSlots;

    Button _slotBt;

    private void OnValidate()
    {
        gameObject.name ="Skill-"+ _slotName;
    }

    private void Awake()
    {
        _slotBt = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _slotBt.onClick.AddListener(IsUnLockSlot);
    }

    private void Start()
    {

        

        if (!UnLock)
        {
            _slotImage.color = _lockColor;
        }
        
    }

    private void OnDisable()
    {
        _slotBt.onClick.RemoveListener(IsUnLockSlot);
    }

    private void OnDestroy()
    {
        if (_onSkillUnLockEvent != null)
        {
            _onSkillUnLockEvent.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 是否解锁技能
    /// </summary>
    private void IsUnLockSlot()
    {
        if(UnLock)
           return;

        if (GlobalReferencesManager.Instance.GamePlayer.HaveEnoughMoney(_skillPrice) == false)
            return;

        foreach (var slot in _unLockSlots)
        {
            if (slot.UnLock == false)
            {
                Debug.Log("请先解锁前置技能"+slot.gameObject.name);
                return;
            }
        }

        foreach (var slot in _lockSlots)
        {
            if (slot.UnLock == true)
            {
                Debug.Log("此技能必须在"+slot.gameObject.name+"锁定下才能使用");
                return;
            }
        }

        UnLock = true;
        _slotImage.color = Color.white;
        _onSkillUnLockEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuController.Instance.SkillTip.ShowTip(_slotName+": "+_skillDescription);

        //以下功能为提示跟随鼠标
        //float offsetX = 0;
        //float offsetY = 0;

        //Vector2 mousePosition=Input.mousePosition;

        //offsetX = 150;
        //if (mousePosition.x > 600)
        //{
        //    offsetX = -150;
        //}

        //offsetY = 150;
        //if (offsetY > 600)
        //{
        //    offsetY = -150;
        //}

        //MenuController.Instance.SkillTip.transform.position=new Vector3(mousePosition.x+offsetX,mousePosition.y+offsetY, 0);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuController.Instance.SkillTip.HideTip();
    }

    public void LoadGameData(GameData data)
    {
        if(data.SkillUnlock.TryGetValue(_slotName, out bool value))
        {
            UnLock = value;
            if (UnLock)
            {
                _slotImage.color = Color.white;
                _onSkillUnLockEvent?.Invoke();
            }
        }
    }

    public void SaveGameData(GameData data)
    {
        if(data.SkillUnlock.TryGetValue(_slotName, out bool value))
        {
            data.SkillUnlock.Remove(_slotName);
            data.SkillUnlock.Add(_slotName, UnLock);
        }
        else
        {
            data.SkillUnlock.Add(_slotName, UnLock);
        }
    }
}
