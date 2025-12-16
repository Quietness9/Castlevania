using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New ThunderStrike Effect",menuName ="GameEffect/EquipmentEffect/ThunderStrike")]
public class ThunderStrikeEffect : EquipmentEffect
{
    public override void ReleaseEffects(Transform transform)
    {
        GameObject thunderStrikePre=GlobalReferencesManager.Instance.GetPrefab("ThunderStrike");

        if(thunderStrikePre==null)
            return;

        GameObject thunderStrikeObj=Instantiate(thunderStrikePre,transform.position,Quaternion.identity);

        Destroy(thunderStrikeObj, destroyTime);
    }
}
