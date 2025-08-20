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

namespace Services.Controls.CameraRotation
{
    [Serializable]
    public class CameraRotationService : InputSchemeDependentService, ICameraRotationService
    {
        [SerializeField] private bool _invertYAxis;
        [SerializeField] private float _playerRotationSpeed;
        [SerializeField] private float _pcSensitivity;
        [SerializeField] private float _mobileSensitivity;
        [Inject] private ICameraController _cameraController;
        [Inject] private IPlayerService _playerService;
        [Inject] private DragScreenView _dragScreenView;
        private IDisposable _schemeUpdateSubscription = Disposable.Empty;

        public void Rotate(Vector2 delta)
        {
            var playerPos = _playerService.PlayerProxy.transform.position;
            var camTransform = _cameraController.VirtualCamera.transform;
            camTransform.RotateAround(playerPos, -Vector3.up, delta.x);
            if (Mathf.Abs(camTransform.forward.y) < 0.9f || Mathf.Sign(delta.y) == Mathf.Sign(camTransform.forward.y))
            {
                camTransform.RotateAround(Vector3.zero, camTransform.right, delta.y);
            }

            // camTransform.Rotate(-camTransform.up * delta.x * _sensitivity);
            // camTransform.Rotate(camTransform.right * delta.y * _sensitivity);
        }

        private void RefreshScheme()
        {
            _schemeUpdateSubscription?.Dispose();
            switch (InputSchemeService.CurrentScheme)
            {
                case InputSchemeType.PC:
                    _schemeUpdateSubscription = Observable.EveryUpdate()
                        .Subscribe(EveryUpdatePC);
                    break;
                case InputSchemeType.Mobile:
                    _schemeUpdateSubscription = _dragScreenView.VectorDrag
                        .Subscribe(OnDragScreenMobile);
                    break;
            }
        }

        private void EveryUpdatePC(long _)
        {
            float deltaX = -UnityEngine.Input.GetAxis("Mouse X");
            float deltaY = -UnityEngine.Input.GetAxis("Mouse Y");
            var delta = new Vector2(deltaX, deltaY);

            if (_invertYAxis)
            {
                delta.x *= -1;
            }

            if (delta == Vector2.zero)
            {
                return;
            }

            Rotate(delta * _pcSensitivity);
        }

        private void OnDragScreenMobile(Vector2 drag)
        {
            if (drag == Vector2.zero)
            {
                return;
            }

            drag.x = -drag.x;
            drag.y = -drag.y;

            if (_invertYAxis)
            {
                drag.y *= -1;
            }

            Rotate(drag * _mobileSensitivity);
        }

        protected override void OnChangeScheme(InputSchemeType inputSchemeType)
        {
            base.OnChangeScheme(inputSchemeType);
            RefreshScheme();
        }

        #region Service

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as CameraRotationService;
            builder.RegisterInstance<ICameraRotationService>(instance)
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
            _schemeUpdateSubscription.Dispose();
        }

        protected override void OnTick()
        {
            _playerService.PlayerProxy.LookToDirectionSmooth(_cameraController.Camera.transform.forward,
                Time.deltaTime * _playerRotationSpeed);
        }

        #endregion
    }
}