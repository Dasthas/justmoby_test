using System;
using Components;
using Services.Base;
using Services.Characteristics;
using Services.Characteristics.Settings;
using Services.Input.Scheme;
using Services.Player;
using UI.Input;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.Controls.Move
{
    [Serializable]
    public class PlayerMovingService : InputSchemeDependentService, IPlayerMovingService
    {
        [SerializeField] private float _defaultSpeed = 3.0f;
        
        [Inject] private ICameraController _cameraController;
        [Inject] private IPlayerService _playerService;
        [Inject] private ICharacteristicsService _characteristicsService;
        [Inject] private MoveJoystickView _moveJoystickView;

        private Vector3 _playerInput = Vector3.zero;
        private IDisposable _schemeUpdateSubscription = Disposable.Empty;

        private void Move()
        {
            Quaternion flatten = Quaternion.LookRotation(
                                     -Vector3.up,
                                     _cameraController.Camera.transform.forward)
                                 * Quaternion.Euler(-90f, 0, 0);

            var direction = (flatten * _playerInput).normalized;
            var newForward = new Vector3(direction.x, 0, direction.z);
            var motionVector = newForward * GetPlayerSpeed() * Time.deltaTime;
            _playerService.PlayerProxy.Move(motionVector);
        }

        private void RefreshScheme()
        {
            _schemeUpdateSubscription?.Dispose();
            switch (InputSchemeService.CurrentScheme)
            {
                case InputSchemeType.PC:
                    _schemeUpdateSubscription = Observable.EveryUpdate()
                        .Subscribe(EveryUpdateMovePC);
                    break;
                case InputSchemeType.Mobile:
                    _schemeUpdateSubscription = _moveJoystickView.VectorDrag
                        .Subscribe(OnDragMoveMobile);
                    break;
            }
        }

        private void EveryUpdateMovePC(long _)
        {
            Vector3 inputVector = Vector3.zero;

            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                inputVector += Vector3.forward;
            }

            if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                inputVector += Vector3.back;
            }

            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                inputVector += Vector3.left;
            }

            if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                inputVector += Vector3.right;
            }

            _playerInput = inputVector;
        }

        private float GetPlayerSpeed()
        {
            return _characteristicsService.CalculateUpgradedValue(_defaultSpeed, CharacteristicType.Speed);
        }

        private void OnDragMoveMobile(Vector2 drag)
        {
            _playerInput = new Vector3(drag.x, 0, drag.y);
        }

        protected override void OnChangeScheme(InputSchemeType inputSchemeType)
        {
            RefreshScheme();
        }

        #region Service

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as PlayerMovingService;
            builder.RegisterInstance<IPlayerMovingService>(instance)
                .As<IInitializable>()
                .As<ITickable>()
                .As<IDisposable>();
            return instance;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            RefreshScheme();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _schemeUpdateSubscription?.Dispose();
        }

        protected override void OnTick()
        {
            if (_playerInput == Vector3.zero)
            {
                return;
            }

            Move();
        }

        #endregion
    }
}