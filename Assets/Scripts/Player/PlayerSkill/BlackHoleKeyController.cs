using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleKeyController : MonoBehaviour
{
    Enemy _enemy;
    TextMeshProUGUI _keyText;
    KeyCode _blackHoleKey;
    float _blackHoleKeyDurationTimer;

    BlackHoleController _blackHole;


    /// <summary>
    /// 设置敌人头上按键
    /// </summary>
    /// <param name="key"></param>
    public void SetBlackHoleKey(KeyCode key,Enemy enemy,float blackHoleKeyDurationTimer, BlackHoleController blackHoleController)
    {
        _keyText=GetComponentInChildren<TextMeshProUGUI>();

        _blackHoleKey = key;
        _enemy = enemy;
        _keyText.text=key.ToString();
        _blackHole = blackHoleController;
        _blackHoleKeyDurationTimer=blackHoleKeyDurationTimer;

    }

    private void Update()
    {
        if (Input.GetKeyDown(_blackHoleKey))
        {
            _blackHole.AddEnemyToList(_enemy);
            Destroy(gameObject);
        }

        _blackHoleKeyDurationTimer-=Time.deltaTime;
        if (_blackHoleKeyDurationTimer < 0)
        {
            Destroy(gameObject);
        }
    }

}
