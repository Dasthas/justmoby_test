using Services.Input.Scheme;
using UI.Buttons;
using UI.Characteristics;
using UniRx;
using UnityEngine;
using VContainer;

namespace UI
{
    public class InputHandler : MonoBehaviour
    {
        private IInputSchemeService _inputSchemeService;

        [SerializeField] private MoveJoystickView _moveJoystickView;
        [SerializeField] private DragScreenView _dragScreenView;
        [SerializeField] private ShootButtonView _shootButtonView;
        [SerializeField] private ShowCharacteristicsButtonView _showCharacteristicsButtonView;
        [SerializeField] private ChangeInputSchemeButtonView _changeInputSchemeButtonView;
        [SerializeField] private GameHud _gameHud;
        [SerializeField] private CharacteristicsScreen _characteristicsScreen;

        private void Start()
        {
            _showCharacteristicsButtonView.Button.OnClickAsObservable()
                .Subscribe(_ => _characteristicsScreen.Open())
                .AddTo(this);
        }

        public void Register(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback((container) =>
            {
                _inputSchemeService = container.Resolve<IInputSchemeService>();

                _inputSchemeService.OnInputSchemeChanged
                    .Subscribe(OnInputSchemeChanged)
                    .AddTo(gameObject);

                OnInputSchemeChanged(_inputSchemeService.CurrentScheme);
                container.Inject(_characteristicsScreen);
            });

            builder.RegisterInstance(_gameHud);
            builder.RegisterInstance(_changeInputSchemeButtonView);
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