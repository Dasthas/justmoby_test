using System;
using Services.Base;
using UI.Input;
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

        private IDisposable _onInputSchemeButtonOnClick;

        [Inject] private ChangeInputSchemeButtonView _changeInputSchemeButtonView;

        IObservable<InputSchemeType> IInputSchemeService.OnInputSchemeChanged => _onInputSchemeCommand;

        InputSchemeType IInputSchemeService.CurrentScheme => _currentScheme;

        public void ChangeInputScheme(InputSchemeType newInputScheme)
        {
            Debug.Log($"OnChangeScheme: {newInputScheme}");
            _currentScheme = newInputScheme;
            _onInputSchemeCommand.Execute(newInputScheme);
        }

        #region Service

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _currentScheme = _defaultScheme;
            _onInputSchemeCommand.Execute(_currentScheme);
            
            _onInputSchemeButtonOnClick = _changeInputSchemeButtonView.Button
                .OnClickAsObservable()
                .Subscribe((_) =>
                    ChangeInputScheme(_currentScheme == InputSchemeType.Mobile
                        ? InputSchemeType.PC
                        : InputSchemeType.Mobile));
        }

        protected override void OnDispose()
        {
            _onInputSchemeButtonOnClick?.Dispose();
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