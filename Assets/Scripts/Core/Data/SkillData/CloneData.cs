using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Clone Data",menuName ="GameData/Skill/CloneData")]
public class CloneData : ScriptableObject
{
    [field: Range(0,1f)]
    [field:SerializeField] public float CloneAtkRatio {  get;private set; }
    [field: Range(0, 1f)]
    [field:SerializeField] public float CloneAtkEnhancedRation {  get; private set; }
    [field: SerializeField] public float ColorDisappearSpeed { get; private set; }
    [field: SerializeField] public float CloneDuration { get; private set; }
    [field: SerializeField] public float CreateCloneDelayTime { get; private set; }
    [field: SerializeField] public float AttackCheckRadius {  get; private set; }
    [field:SerializeField] public float CheckClosestEnemyRadius {  get; private set; }

    //重复创造克隆体
    [field:SerializeField] public float DutCreateCloneProbability {  get; private set; }
    [field: SerializeField]public Vector2 DutCreateCloneOffset { get; private set; }
}
