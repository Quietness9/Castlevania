using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tipContent;

    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="content"></param>
    public void ShowTip(string content)
    {
        _tipContent.text = content;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏提示
    /// </summary>
    public void HideTip()=> gameObject.SetActive(false);

}
