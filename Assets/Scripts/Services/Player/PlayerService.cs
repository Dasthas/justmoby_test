using System;
using Components;
using Components.Characters;
using Components.Scene;
using Services.Base;
using Services.Characteristics;
using Services.Characteristics.Settings;
using Sirenix.OdinInspector;
using UI.Input;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Services.Player
{
    [Serializable]
    public class PlayerService : Service, IPlayerService
    {
        [SerializeField] private float _defaultHealth = 100f;
        [SerializeField] [AssetsOnly] private PlayerProxy _prefab;

        [Inject] private ISceneEnvironment _sceneEnvironment;
        [Inject] private ICameraController _cameraController;
        [Inject] private ICharacteristicsService _characteristicsService;
        [Inject] private GameHud _gameHud;

        private PlayerProxy _playerProxy;
        private CompositeDisposable _disposables;

        PlayerProxy IPlayerService.PlayerProxy => _playerProxy;

        public void OnPlayerHealthChanged(HealthChangedData healthChangedData)
        {
            _gameHud.UpdateHealth(healthChangedData.CurrentHealth, healthChangedData.MaxHealth);
        }

        private void OnCharacteristicsUpdated(CharacteristicType characteristicType)
        {
            if (characteristicType != CharacteristicType.Health)
            {
                return;
            }

            _playerProxy.HealthController.SetMaxHealth(
                _characteristicsService.CalculateUpgradedValue(_defaultHealth, CharacteristicType.Health));
        }

        #region Service

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as PlayerService;
            builder.RegisterInstance<IPlayerService>(instance)
                .As<IInitializable>()
                .As<IDisposable>();
            return instance;
        }

        protected override void OnInitialize()
        {
            _disposables = new CompositeDisposable();
            _playerProxy = Object.Instantiate(_prefab, _sceneEnvironment.DynamicObjectsParent);
            _playerProxy.OnHealthChanged
                .Subscribe(OnPlayerHealthChanged)
                .AddTo(_disposables);
            _playerProxy.Initialize(_defaultHealth);
            _characteristicsService.OnCharacteristicUpgraded
                .Subscribe(OnCharacteristicsUpdated)
                .AddTo(_disposables);

            _cameraController.SetFollowTarget(_playerProxy.transform);
        }

        protected override void OnDispose()
        {
            _disposables.Dispose();
        }

        #endregion
    }
}