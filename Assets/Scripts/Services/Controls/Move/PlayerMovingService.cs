using System;
using Components;
using Services.Base;
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
        [SerializeField] private float _playerSpeed = 3.0f;
        [Inject] private ICameraController _cameraController;
        [Inject] private IPlayerService _playerService;
        [Inject] private MoveJoystickView _moveJoystickView;

        private Vector3 _playerVelocity = Vector3.zero;
        private IDisposable _schemeUpdateSubscription = Disposable.Empty;

        private void Move()
        {
            Quaternion flatten = Quaternion.LookRotation(
                                     -Vector3.up,
                                     _cameraController.Camera.transform.forward)
                                 * Quaternion.Euler(-90f, 0, 0);

            var camDir = (flatten * _playerVelocity).normalized;
            var newForward = new Vector3(camDir.x, 0, camDir.z);
            var motionVector = newForward * _playerSpeed * Time.deltaTime;
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

            _playerVelocity = inputVector * _playerSpeed * Time.deltaTime;
        }

        private void OnDragMoveMobile(Vector2 drag)
        {
            _playerVelocity = new Vector3(drag.x, 0, drag.y) * _playerSpeed * Time.deltaTime;
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
            RefreshScheme();
        }

        protected override void OnDispose()
        {
            _schemeUpdateSubscription?.Dispose();
        }

        protected override void OnTick()
        {
            if (_playerVelocity == Vector3.zero)
            {
                return;
            }

            Move();
        }

        #endregion
    }
}