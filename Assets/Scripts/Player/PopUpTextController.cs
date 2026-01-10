using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextController : MonoBehaviour
{
    TextMeshPro _popUpText;
    CharacterFxData _fxData;

    float _speed;

    private void Awake()
    {
        _popUpText = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (_fxData == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 1),
            _speed * Time.deltaTime);
        float alpha=_popUpText.color.a- _fxData.ColorDisappearSpeed* Time.deltaTime;
        _popUpText.color=new Color(_popUpText.color.r, _popUpText.color.g,_popUpText.color.b,alpha);

        if (_popUpText.color.a < 50)
        {
            _speed= _fxData.DisappearSpeed;
        }

        if (_popUpText.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 设置上升文本
    /// </summary>
    /// <param name="fxData"></param>
    /// <param name="text"></param>
    public void SetPopUpText(CharacterFxData fxData,string text)
    {
        _fxData = fxData;

        _speed=fxData.PopUpSpeed;
        _popUpText.text=text;
    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)=>_popUpText.color=color;
}
