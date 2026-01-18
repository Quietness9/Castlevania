using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropItem : ItemDropCtr
{
    public override void DropItem()
    {
        InventoryController inventoryController = InventoryController.Instance;

        GameObject dropItemPre = GlobalReferencesMgr.Instance.GetPrefab("Item");

       

        if(inventoryController==null||dropItemPre==null)
            return;

        //物品为null或总物品数量小于5不掉落物品
        if (inventoryController.Items == null || inventoryController.Items.Count < 5)
            return;

        

        for (int i=0; i < _dropAmount; i++)
        {
            
            int index=Random.Range(0, inventoryController.Items.Count);

            GameObject dropItemObj=Instantiate(dropItemPre,transform.position,Quaternion.identity);
            dropItemObj.GetComponent<ItemObject>().SetItemData(inventoryController.Items[index].ItemData);

            inventoryController.DropDirItem(inventoryController.Items[index]);
        }
    }
}
