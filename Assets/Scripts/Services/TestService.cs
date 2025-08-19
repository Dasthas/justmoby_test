using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services
{
    [Serializable]
    public class TestService : Service
    {
        [SerializeField] private float _test;

        public override void RegisterSelf(IContainerBuilder builder)
        {
            builder.Register<TestService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<TestService>();
        }

        protected override void OnInitialize()
        {
            Debug.Log("OnInitialize TestService");
        }

        protected override void OnDispose()
        {
            Debug.Log("OnDispose TestService");
        }
    }
}