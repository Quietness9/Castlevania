using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    GameObject _camera;
    [SerializeField]float _parallaxEffect;
    float _xPosition;
    float _length;
    void Start()
    {
        _camera = GameObject.Find("Main Camera");

        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _xPosition = transform.position.x;

    }


    void Update()
    {

        float distanceMove = _camera.transform.position.x * (1 - _parallaxEffect);
        float distanceToMove = _camera.transform.position.x * _parallaxEffect;
        transform.position = new Vector3(_xPosition + distanceToMove, transform.position.y);

        if (distanceMove > _xPosition + _length)
        {

            _xPosition = _xPosition + _length;
        }
        else if (distanceMove < _xPosition - _length)
        {
            _xPosition = _xPosition - _length;
        }
    }
}
