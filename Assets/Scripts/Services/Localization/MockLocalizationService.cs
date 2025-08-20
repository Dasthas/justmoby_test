using System;
using Services.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.Localization
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

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as MockLocalizationService;
            builder.RegisterInstance<ILocalizationService>(instance)
                .As<IInitializable>()
                .As<IDisposable>();
            return instance;
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