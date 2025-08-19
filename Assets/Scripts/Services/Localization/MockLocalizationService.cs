using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services
{
    [Serializable]
    [InfoBox("Return key instead of localized string")]
    public class MockLocalizationService : Service, ILocalizationService
    {
        string ILocalizationService.GetLocalizedString(string key)
        {
            return key;
        }

        #region Service

        public override void RegisterSelf(IContainerBuilder builder)
        {
            builder.RegisterInstance<ILocalizationService>(this)
                .As<IInitializable>()
                .As<IDisposable>();
        }

        protected override void OnInitialize()
        {
            Debug.Log("OnInitialize MockLocalizationService");
        }

        protected override void OnDispose()
        {
            Debug.Log("OnDispose MockLocalizationService");
        }

        #endregion
    }
}