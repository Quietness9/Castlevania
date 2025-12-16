using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IceAndFire Effect", menuName = "GameEffect/EquipmentEffect/IceAndFire")]
public class IceAndFireEffect : EquipmentEffect
{

    [SerializeField] Vector2 _force;
    [Range(0,1f)]
    [SerializeField] float _triggerProbability;
    float _direction=>GlobalReferencesManager.Instance.GamePlayer.Direction;

    public override void ReleaseEffects(Transform transform)
    {
        if (Random.value > _triggerProbability)
            return;

        GameObject iceAndFirePre=GlobalReferencesManager.Instance.GetPrefab("IceAndFire");

        if(iceAndFirePre==null)
            return;

        GameObject iceAndFireObj=Instantiate(iceAndFirePre,transform.position, Quaternion.identity);
        iceAndFireObj.GetComponent<IceAndFireController>().SetForce(new Vector2(_force.x * _direction, _force.y));

        Destroy(iceAndFireObj, destroyTime);
    }
}
