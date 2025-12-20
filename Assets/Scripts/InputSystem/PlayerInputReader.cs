using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInputSystem
{
    [CreateAssetMenu(fileName ="New Input Data",menuName ="GameData/InputData")]
    public class PlayerInputReader:ScriptableObject,GameInput.IPlayerActions,GameInput.ISkillActions,GameInput.IGamePropsActions,GameInput.IUIActions
    {
        GameInput _gameInput;

        //playerController
        public event Action<Vector2> OnMoveEvent=delegate { };
        public event Action OnJumpUpEvent=delegate { };
        public event Action OnAttackEvent=delegate { };

        //SkillController
        public event Action OnDashEvent=delegate { };
        public event Action OnCounterAttackEvent=delegate { };
        public event Action OnAimSwordEvent=delegate { };
        public event Action OnCancelSwordEvent = delegate { };
        public event Action OnBlackHoleEvent=delegate { };
        public event Action OnCrystalEvent=delegate { };

        //GamePropController
        public event Action OnUseFlaskEvent=delegate { };

        //UI
        public event Action OnCharacterUIEvent=delegate { };
        public event Action OnSkillTreeUIEvent=delegate { };
        public event Action OnCraftUIEvent=delegate { };
        public event Action OnOptionUIEvent=delegate { };

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Player.SetCallbacks(this);
                _gameInput.Skill.SetCallbacks(this);
                _gameInput.GameProps.SetCallbacks(this);
                _gameInput.UI.SetCallbacks(this);
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
            OnMoveEvent = delegate { };
            OnJumpUpEvent = delegate { };
            OnAttackEvent = delegate { };

            OnDashEvent = delegate { };
            OnCounterAttackEvent = delegate { };
            OnAimSwordEvent = delegate { };
            OnCancelSwordEvent = delegate { };
            OnBlackHoleEvent = delegate { };
            OnCrystalEvent = delegate { };

            OnUseFlaskEvent = delegate { };


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
                _gameInput.GameProps.Disable();
                _gameInput.UI.Disable();
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
            _gameInput.GameProps.Enable();
            _gameInput.UI.Enable();
        }


        #region 玩家控制

        public void OnMove(InputAction.CallbackContext context)
        {
            OnMoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {

            if (context.phase == InputActionPhase.Canceled)
            {
                OnJumpUpEvent.Invoke();
            }
        }


        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                OnAttackEvent.Invoke();
            }
        }

        #endregion

        #region 技能控制

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnDashEvent.Invoke();
            }
        }

        public void OnCounterAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnCounterAttackEvent.Invoke();
            }
        }

        public void OnAimSword(InputAction.CallbackContext context)
        {
            if(context.phase== InputActionPhase.Performed)
            {
                OnAimSwordEvent.Invoke();
            }

            if(context.phase== InputActionPhase.Canceled)
            {
                OnCancelSwordEvent.Invoke();
            }
        }


        public void OnBlackHole(InputAction.CallbackContext context)
        {
            if( context.phase == InputActionPhase.Performed)
            {
                OnBlackHoleEvent.Invoke();
            }
        }

        public void OnCrystal(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnCrystalEvent.Invoke();
            }
        }

        #endregion

        #region 道具控制

        public void OnUseFlask(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnUseFlaskEvent.Invoke();
            }
        }

        #endregion

        #region UI控制

        public void OnCharacterUI(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                OnCharacterUIEvent.Invoke();
            }
        }

        public void OnSkillTreeUI(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnSkillTreeUIEvent.Invoke();
            }
        }

        public void OnCraftUI(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnCraftUIEvent.Invoke();
            }
        }

        public void OnOption(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnOptionUIEvent.Invoke();
            }
        }

        #endregion
    }

}


