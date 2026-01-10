using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashShadowController : MonoBehaviour
{
    SpriteRenderer _sr;
    float _colorLooseRate;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float alpha=_sr.color.a-_colorLooseRate*Time.deltaTime;
        _sr.color=new Color(_sr.color.r,_sr.color.g,_sr.color.b,alpha);

        if (_sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// …Ë÷√≥Â¥Ã≤–”∞
    /// </summary>
    /// <param name="colorLooseRate"></param>
    public void SetDashShadow(float colorLooseRate)
    {
        _colorLooseRate = colorLooseRate;
    }
    

}
