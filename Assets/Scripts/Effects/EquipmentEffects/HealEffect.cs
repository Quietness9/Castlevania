using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "GameEffect/EquipmentEffect/Heal")]
public class HealEffect : EquipmentEffect
{
    [Range(0f, 1f)]
    [SerializeField] float _healRatio;

    public override void ReleaseEffects(Transform transform)
    {
        CharacterAttribute attribute = GlobalReferencesManager.Instance.GamePlayer.Attribute;

        if (attribute == null)
        {
            Debug.Log("attribute is null");
            return;
        }

        int healValue = Mathf.RoundToInt(attribute.GetMaxHealth() * _healRatio);

        attribute.RecoverCurrentHealth(healValue);
    }
}
