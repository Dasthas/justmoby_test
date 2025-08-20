using Services.Base;
using UniRx;
using UnityEngine;
using VContainer;

namespace Services.Input.Scheme
{
    public abstract class InputSchemeDependentService : Service
    {
        [Inject] protected IInputSchemeService InputSchemeService;
        
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        protected override void OnInitialize()
        {
            InputSchemeService.OnInputSchemeChanged
                .Subscribe(OnChangeScheme)
                .AddTo(_disposable);
        }

        protected override void OnDispose()
        {
            _disposable.Dispose();
        }

        protected virtual void OnChangeScheme(InputSchemeType inputSchemeType)
        {
        }
    }
}