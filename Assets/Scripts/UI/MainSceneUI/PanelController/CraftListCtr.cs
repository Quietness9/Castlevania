using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListCtr : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] Transform _craftSlotParent;
    [SerializeField] List<EquipmentItemData> _equipmentData=new();

    static List<GameObject> _craftSlotControllers = new();


    /// <summary>
    /// 设置制作槽中的数据
    /// </summary>
    private void SetCraftSlotData()
    {

        for(int i=0;i< _craftSlotControllers.Count; i++)
        {
            ObjectPoolMgr.ReturnObjectToPool( _craftSlotControllers[i],PoolType.CraftListObject);
        }

        _craftSlotControllers.Clear();
        GameObject craftSlotPre = GlobalReferencesMgr.Instance.GetPrefab("CraftSlot");

        if (craftSlotPre == null)
            return;


        for(int i = 0; i < _equipmentData.Count; i++)
        {
            GameObject craftSlotObj = ObjectPoolMgr.SpawnObject(craftSlotPre, _craftSlotParent.position, Quaternion.identity, PoolType.CraftListObject);
            craftSlotObj.GetComponent<CraftSlotCtr>().SetSlotData(_equipmentData[i]);

            _craftSlotControllers.Add(craftSlotObj);
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetCraftSlotData();
    }
}
