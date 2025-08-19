using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services
{
    [Serializable]
    public abstract class Service : IService, IInitializable, ITickable, IDisposable
    {
        public abstract void RegisterSelf(IContainerBuilder builder);
        
        public void Initialize()
        {
            Debug.Log("Initialize");
            OnInitialize();
        }

        public void Dispose()
        {
            Debug.Log("Dispose");
            OnDispose();
        }

        public void Tick()
        {
            OnTick();
        }

        protected virtual void OnInitialize()
        {
            Debug.Log("OnInitialize");
        }

        protected virtual void OnDispose()
        {
            Debug.Log("OnDispose");
        }

        protected virtual void OnTick()
        {
            Debug.Log("OnTick");
        }
    }
}