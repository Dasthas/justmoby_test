using Services.Input.Scheme;
using UniRx;
using UnityEngine;
using VContainer;

namespace UI.Input
{
    public class InputHandler : MonoBehaviour
    {
        private IInputSchemeService _inputSchemeService;

        [SerializeField] private MoveJoystickView _moveJoystickView;
        [SerializeField] private DragScreenView _dragScreenView;
        [SerializeField] private ShootButtonView _shootButtonView;

        public void Register(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback((container) =>
            {
                _inputSchemeService = container.Resolve<IInputSchemeService>();

                _inputSchemeService.OnInputSchemeChanged
                    .Subscribe(OnInputSchemeChanged)
                    .AddTo(gameObject);

                OnInputSchemeChanged(_inputSchemeService.CurrentScheme);
            });

            builder.RegisterInstance(_moveJoystickView);
            builder.RegisterInstance(_dragScreenView);
            builder.RegisterInstance(_shootButtonView);
        }

        private void OnInputSchemeChanged(InputSchemeType inputSchemeType)
        {
            var mobile = inputSchemeType == InputSchemeType.Mobile;
            _moveJoystickView.gameObject.SetActive(mobile);
            _dragScreenView.gameObject.SetActive(mobile);
            _shootButtonView.gameObject.SetActive(mobile);
        }
    }
}