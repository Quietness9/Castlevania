using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//库存物品数据
[CreateAssetMenu(fileName ="New Item Data",menuName ="GameData/ItemData")]
public class ItemData : ScriptableObject
{
    [field:SerializeField] public int Id {  get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Sprite Icon { get; private set; }


}
