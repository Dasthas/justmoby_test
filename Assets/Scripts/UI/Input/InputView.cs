using JetBrains.Annotations;
using UI.Input.Joysticks;
using UniRx.Triggers;
using UnityEngine;

namespace UI.Input
{
    public class InputView : MonoBehaviour
    {
        [SerializeField] private ObservableDragTrigger _dragTrigger;
        [SerializeField] private ObservablePointerDownTrigger _pointerDownTrigger;
        [SerializeField] private ObservablePointerUpTrigger _pointerUpTrigger;
        [SerializeField] private JoystickView _moveJoystickView;

        public ObservableDragTrigger DragTrigger => _dragTrigger;
        public ObservablePointerDownTrigger PointerDownTrigger => _pointerDownTrigger;
        public ObservablePointerUpTrigger PointerUpTrigger => _pointerUpTrigger;

        public JoystickView MoveJoystickView => _moveJoystickView;
    }
}