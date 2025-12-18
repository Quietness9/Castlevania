using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : CharacterAttribute
{
    protected override void Start()
    {
        base.Start();
    }

    public override void ReduceCurrentHealth(int amount)
    {
        base.ReduceCurrentHealth(amount);


        int triggerHealth = Mathf.RoundToInt(GetMaxHealth() * 0.2f);
        if (CurrentHealth < triggerHealth)
        {
            EquipmentItemData equipment = InventoryController.Instance.GetEquipment(EquipmentItemType.Armor);
            if (equipment != null&&equipment.Id==116)
            {
                if(InventoryController.Instance.CanUseEquipment(EquipmentItemType.Armor, equipment))
                {
                    equipment.UseEquipmentEffect(transform);
                }
            }
        }
    }
}
