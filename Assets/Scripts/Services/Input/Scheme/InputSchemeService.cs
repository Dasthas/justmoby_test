using System;
using Services.Base;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.Input.Scheme
{
    [Serializable]
    public class InputSchemeService : Service, IInputSchemeService
    {
        [SerializeField] private InputSchemeType _defaultScheme = InputSchemeType.PC;
        private InputSchemeType _currentScheme;
        private ReactiveCommand<InputSchemeType> _onInputSchemeCommand = new ReactiveCommand<InputSchemeType>();

        IObservable<InputSchemeType> IInputSchemeService.OnInputSchemeChanged => _onInputSchemeCommand;

        InputSchemeType IInputSchemeService.CurrentScheme => _currentScheme;

        void IInputSchemeService.ChangeInputScheme(InputSchemeType newInputScheme)
        {
            _currentScheme = newInputScheme;
            _onInputSchemeCommand.Execute(newInputScheme);
        }

        #region Service

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _currentScheme = _defaultScheme;
            _onInputSchemeCommand.Execute(_currentScheme);
        }

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as InputSchemeService;
            builder.RegisterInstance<IInputSchemeService>(instance)
                .As<IInitializable>()
                .As<IDisposable>();
            return instance;
        }

        #endregion
    }
}