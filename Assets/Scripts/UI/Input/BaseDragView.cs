using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Input
{
    public abstract class BaseDragView : MonoBehaviour
    {
        [SerializeField] private bool _collectOnlyDeltaDrag;
        [SerializeField] private InputView _inputView;
        private CompositeDisposable _disposables = new();

        private Vector2 _posInput;
        private Vector2 _startDragPosition;

        private readonly Vector2ReactiveProperty _vectorDrag = new();
        private readonly ReactiveCommand<PointerEventData> _onPointerDown = new();
        private readonly ReactiveCommand<PointerEventData> _onPointerUp = new();

        public IObservable<Vector2> VectorDrag => _vectorDrag;
        public IObservable<PointerEventData> OnPointerDown => _onPointerDown;
        public IObservable<PointerEventData> OnPointerUp => _onPointerUp;

        public void OnDisable()
        {
            ExecutePointerUp(default);
            _disposables.Dispose();
            _disposables = new();
        }

        public void OnEnable()
        {
            _inputView.PointerDownTrigger.OnPointerDownAsObservable()
                .Subscribe(ExecutePointerDown)
                .AddTo(_disposables)
                .AddTo(_inputView);

            _inputView.PointerUpTrigger.OnPointerUpAsObservable()
                .Subscribe(ExecutePointerUp)
                .AddTo(_disposables)
                .AddTo(_inputView);

            _inputView.DragTrigger.OnDragAsObservable()
                .Subscribe(ExecuteDragContinued)
                .AddTo(_disposables)
                .AddTo(_inputView);
        }

        private void ExecutePointerDown(PointerEventData eventData)
        {
            if (!_collectOnlyDeltaDrag)
            {
                _startDragPosition = eventData.position;
            }

            _onPointerDown.Execute(eventData);
            OnDragStart(eventData);
        }

        private void ExecuteDragContinued(PointerEventData eventData)
        {
            Vector2 delta;

            if (!_collectOnlyDeltaDrag)
            {
                delta = eventData.position - _startDragPosition;
            }
            else
            {
                delta = eventData.delta;
            }

            _vectorDrag.Value = delta;
            OnDragContinue(delta);
        }

        private void ExecutePointerUp(PointerEventData eventData)
        {
            _startDragPosition = Vector2.zero;
            _vectorDrag.Value = Vector2.zero;
            _onPointerUp.Execute(eventData);
            OnDragFinished(eventData);
        }

        private void OnDragStart(PointerEventData eventData)
        {
            if (_inputView.MoveJoystickView == null)
            {
                return;
            }

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _inputView.MoveJoystickView.RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out _posInput))
            {
                _inputView.MoveJoystickView.ActivateStickInPosition(_posInput);
            }
        }

        private void OnDragContinue(Vector2 drag) => _inputView.MoveJoystickView?.MoveStick(drag);

        private void OnDragFinished(PointerEventData eventData) => _inputView.MoveJoystickView?.ResetStick();
    }
}