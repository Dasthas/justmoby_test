using System;
using Components;
using Components.Characters;
using Services.Base;
using Services.Characteristics;
using Services.Characteristics.Settings;
using Services.Characteristics.Settings.Data;
using Services.Enemy;
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
        [SerializeField] private float _defaultDamage = 10;
        [SerializeField] private LayerMask _enemiesLayerMask;
        [Inject] private ICameraController _cameraController;
        [Inject] private IEnemyService _enemyService;
        [Inject] private ICharacteristicsService _characteristicsService;
        [Inject] private IPlayerService _playerService;
        [Inject] private ShootButtonView _shootButtonView;

        private IDisposable _schemeUpdateSubscription = Disposable.Empty;
        private IDisposable _enemyKilledSubscription = Disposable.Empty;

        private RaycastHit[] _hits = new RaycastHit[15];

        private void Shoot()
        {
            var rayDirection = _cameraController.Camera.transform.forward;
            var rayOrigin = _cameraController.Camera.transform.position;
            var ray = new Ray(rayOrigin, rayDirection);

            var hitsCount = Physics.RaycastNonAlloc(ray, _hits, float.PositiveInfinity,
                _enemiesLayerMask);

            var hitPosition = Vector3.zero;
            for (int i = 0; i < hitsCount; i++)
            {
                var hit = _hits[i];
                hitPosition = hit.point;
                if (hit.transform.TryGetComponent<IHealthController>(out var healthController))
                {
                    healthController.ProcessDamage(GetPlayerDamage());
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

        private float GetPlayerDamage()
        {
            return _characteristicsService.CalculateUpgradedValue(_defaultDamage, CharacteristicType.Damage);
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

        private void OnEnemyKilled(DeathData deathData)
        {
            _characteristicsService.AddAvailablePoints(1);
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
            base.OnInitialize();
            RefreshScheme();
            
            _enemyKilledSubscription = _enemyService.OnAnyEnemyDeath
                .Subscribe(OnEnemyKilled);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _schemeUpdateSubscription.Dispose();
            _enemyKilledSubscription.Dispose();
        }

        #endregion
    }
}