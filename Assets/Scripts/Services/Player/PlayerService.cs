using System;
using Components;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services
{
    [Serializable]
    public class PlayerService : Service, IPlayerService
    {
        [SerializeField] 
        [AssetsOnly] 
        private PlayerProxy _prefab;

        [Inject] private ISceneEnvironment _sceneEnvironment;
        
        public override void RegisterSelf(IContainerBuilder builder)
        {
            builder.RegisterInstance<IPlayerService>(this)
                .As<IInitializable>()
                .As<IDisposable>();
        }

        protected override void OnInitialize()
        {
            Debug.Log("OnInitialize PlayerService");
        }

        protected override void OnDispose()
        {
            Debug.Log("OnDispose PlayerService");
        }
    }
}