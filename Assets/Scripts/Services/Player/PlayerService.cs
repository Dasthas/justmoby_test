using System;
using Components;
using Components.Characters;
using Components.Scene;
using Services.Base;
using Sirenix.OdinInspector;
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

        private PlayerProxy _playerProxy;
        public PlayerProxy PlayerProxy => _playerProxy;

        [Inject] private ISceneEnvironment _sceneEnvironment;
        [Inject] private ICameraController _cameraController;

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
            Debug.Assert(_sceneEnvironment != null);
            _playerProxy = Object.Instantiate(_prefab, _sceneEnvironment.DynamicObjectsParent);
            _playerProxy.Initialize(_defaultHealth);
            _cameraController.SetFollowTarget(_playerProxy.transform);
            Debug.Log("OnInitialize PlayerService");
        }

        protected override void OnDispose()
        {
            Debug.Log("OnDispose PlayerService");
        }
    }
}