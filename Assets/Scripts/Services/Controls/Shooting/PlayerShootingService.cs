using System;
using Components;
using Components.Characters;
using Services.Base;
using Services.Input.Scheme;
using Services.Player;
using UI.Input;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.Controls.Shooting
{
    [Serializable]
    public class PlayerShootingService : InputSchemeDependentService, IPlayerShootingService
    {
        [SerializeField] private LayerMask _enemiesLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;
        [Inject] private ICameraController _cameraController;
        [Inject] private IPlayerService _playerService;
        [Inject] private ShootButtonView _shootButtonView;

        private IDisposable _schemeUpdateSubscription = Disposable.Empty;

        private RaycastHit[] _hits = new RaycastHit[15];

        private void Shoot()
        {
            var rayDirection = _cameraController.Camera.transform.forward;
            var rayOrigin = _cameraController.Camera.transform.position;
            var ray = new Ray(rayOrigin, rayDirection);

            var hitsCount = Physics.RaycastNonAlloc(ray, _hits, float.PositiveInfinity,
                _enemiesLayerMask);

            var damage = 15;
            var hitPosition = Vector3.zero;
            for (int i = 0; i < hitsCount; i++)
            {
                var hit = _hits[i];
                hitPosition = hit.point;
                if (hit.transform.TryGetComponent<IHealthController>(out var healthController))
                {
                    Debug.Log("hit enemy");
                    healthController.ProcessDamage(damage);
                    break;
                }
            }

            Vector3 directionToTarget;
            if (hitPosition != Vector3.zero)
            {
                directionToTarget = (hitPosition - _playerService.PlayerProxy.transform.position).normalized;
            }
            else
            {
                directionToTarget = rayDirection;
            }

            _playerService.PlayerProxy.LookToDirectionSmooth(directionToTarget, 1);
            _playerService.PlayerProxy.ShowShootVfx();
        }

        private void RefreshScheme()
        {
            _schemeUpdateSubscription?.Dispose();
            switch (InputSchemeService.CurrentScheme)
            {
                case InputSchemeType.PC:
                    _schemeUpdateSubscription = Observable.EveryUpdate()
                        .Subscribe(EveryUpdateShootPC);
                    break;
                case InputSchemeType.Mobile:
                    _schemeUpdateSubscription = _shootButtonView.ShootButton
                        .OnClickAsObservable()
                        .Subscribe((_) => Shoot());
                    break;
            }
        }

        private void EveryUpdateShootPC(long _)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        protected override void OnChangeScheme(InputSchemeType inputSchemeType)
        {
            RefreshScheme();
        }

        #region Service

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as PlayerShootingService;
            builder.RegisterInstance<IPlayerShootingService>(instance)
                .As<IInitializable>()
                .As<IDisposable>();
            return instance;
        }

        protected override void OnInitialize()
        {
            RefreshScheme();
            Debug.Log("OnInitialize PlayerService");
        }

        protected override void OnDispose()
        {
            Debug.Log("OnDispose PlayerService");
        }

        #endregion
    }
}