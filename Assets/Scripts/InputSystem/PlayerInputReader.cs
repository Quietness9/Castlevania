using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInputSystem
{
    [CreateAssetMenu(fileName ="New Input Data",menuName ="PlayerData/InputData")]
    public class PlayerInputReader:ScriptableObject,GameInput.IPlayerActions
    {
        GameInput _gameInput;

        //playerController
        public event Action<Vector2> MoveEvent=delegate { };

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Player.SetCallbacks(this);
            }

            SetInitInput();

        }

        private void OnDisable()
        {
            CloseAllInput();
        }

        private void OnDestroy()
        {
            DisposeAllRes();
        }

        /// <summary>
        /// ��������¼�����
        /// </summary>
        private void ClearAllEvent()
        {

        }


        /// <summary>
        /// �ر���������
        /// </summary>
        private void CloseAllInput()
        {
            if(_gameInput != null)
            {
                _gameInput.Player.Disable();
            }
            
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        public void DisposeAllRes()
        {
            CloseAllInput();
            ClearAllEvent();

            if (_gameInput != null)
            {
                _gameInput.Dispose();
                _gameInput = null;
            }
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void SetInitInput()
        {
            _gameInput.Player.Enable();
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

}


