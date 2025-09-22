using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float CoolTime;

    protected Player player;

    float _coolTimer;


    protected virtual void Start()
    {

    }


    protected virtual void Update()
    {
        _coolTimer-= Time.deltaTime;
    }

}
