using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInputSystem
{
    [CreateAssetMenu(menuName ="PlayerInputData")]
    public class PlayerInputReader:ScriptableObject,GameInput.IPlayerActions
    {
        GameInput _gameInput;

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Player.SetCallbacks(this);
            }

            SetInitInput();

        }

        private void OnDestroy()
        {
            CloseAllInput();
            ClearAllEvent();
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
            _gameInput.Player.Disable();
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
            throw new System.NotImplementedException();
        }
    }

}


