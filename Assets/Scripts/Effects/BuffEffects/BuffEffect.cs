using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Buff Effect", menuName = "GameEffect/BuffEffect")]
public class BuffEffect : EquipmentEffect
{
    [SerializeField] int _buffAmount;
    [SerializeField] float _duration;
    [SerializeField] AttributeType _buffType;

    public override void ReleaseEffects(Transform transform)
    {
        CharacterAttribute characterAttribute=transform.GetComponent<CharacterAttribute>();

        if (characterAttribute==null)
        {
            Debug.Log("使用buff" + _buffType + "的角色属性值为null");
        }

        characterAttribute.AddBuffModifier(_buffType, _buffAmount, _duration);
    }
}
