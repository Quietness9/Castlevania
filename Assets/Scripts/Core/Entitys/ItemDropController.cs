using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    [SerializeField] int _dropAmount;
    [SerializeField] List<ItemData> _dropItemData=new();

    /// <summary>
    /// 掉落物品
    /// </summary>
    public void DropItem()
    {
        GameObject dropItemPre = GlobalReferencesManager.Instance.GetPrefab("Item");

        if (dropItemPre == null||_dropItemData.Count<=0)
            return;

        List<ItemData> dropList = new ();

        //获得可能掉落列表
        for(int i = 0; i < _dropItemData.Count; i++)
        {
            if (Random.value < _dropItemData[i].DropChance)
            {
                dropList.Add(_dropItemData[i]);
            }
        }

        //特殊情况，如果掉落列表为0，默认掉落一个0号物品
        if (dropList.Count <= 0)
        {
            GameObject dropItem = Instantiate(dropItemPre, transform.position, Quaternion.identity);
            dropItem.GetComponent<ItemObject>().SetItemData(_dropItemData[0]);
            return;
        }

        //根据掉落次数掉落物品
        for(int i = 0; i < _dropAmount; i++)
        {
            GameObject dropItem = Instantiate(dropItemPre, transform.position, Quaternion.identity);
            dropItem.GetComponent<ItemObject>().SetItemData(dropList[Random.Range(0,dropList.Count)]);
        }        
    }
}
