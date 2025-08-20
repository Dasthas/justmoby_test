using System;
using Services.Input.Scheme;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using IInitializable = VContainer.Unity.IInitializable;

namespace Services.Base
{
    [Serializable]
    public abstract class Service : IService, IInitializable, ITickable, IDisposable
    {
        public abstract Service RegisterAndGetInstance(IContainerBuilder builder);
        
        protected object Clone() => this.CloneViaSerialization();
        public void Initialize()
        {
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
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnTick()
        {
        }
    }
}