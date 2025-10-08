using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInputSystem
{
    [CreateAssetMenu(fileName ="New Input Data",menuName ="GameData/InputData")]
    public class PlayerInputReader:ScriptableObject,GameInput.IPlayerActions,GameInput.ISkillActions
    {
        GameInput _gameInput;

        //playerController
        public event Action<Vector2> MoveEvent=delegate { };
        public event Action JumpUpEvent=delegate { };
        public event Action AttackEvent=delegate { };

        //SkillController
        public event Action DashEvent=delegate { };
        public event Action CounterAttackEvent=delegate { };


        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Player.SetCallbacks(this);
                _gameInput.Skill.SetCallbacks(this);
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
        /// 清除所有事件订阅
        /// </summary>
        private void ClearAllEvent()
        {
            MoveEvent = delegate { };
            JumpUpEvent = delegate { };


        }


        /// <summary>
        /// 关闭所有输入
        /// </summary>
        private void CloseAllInput()
        {
            if(_gameInput != null)
            {
                _gameInput.Player.Disable();
                _gameInput.Skill.Disable();
            }
            
        }

        /// <summary>
        /// 清理资源
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
        /// 初始化输入
        /// </summary>
        private void SetInitInput()
        {
            _gameInput.Player.Enable();
            _gameInput.Skill.Enable();
        }


        #region 玩家控制

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {

            if (context.phase == InputActionPhase.Canceled)
            {
                JumpUpEvent.Invoke();
            }
        }


        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                AttackEvent.Invoke();
            }
        }

        #endregion

        #region 技能控制

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                DashEvent.Invoke();
            }
        }

        public void OnCounterAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CounterAttackEvent.Invoke();
            }
        }

        #endregion
    }

}


