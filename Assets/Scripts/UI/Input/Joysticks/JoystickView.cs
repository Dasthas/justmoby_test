using UnityEngine;

namespace UI.Input.Joysticks
{
    public class JoystickView : MonoBehaviour
    {
        [SerializeField] private RectTransform _stickContainer;
        [SerializeField] private RectTransform _stickTransform;
        [SerializeField] private RectTransform _rectTransform;

        private float _stickZoneSize;
        public RectTransform RectTransform => _rectTransform;

        protected void Awake() => _stickZoneSize = _stickContainer.rect.width;

        public void ActivateStickInPosition(Vector2 position)
        {
            _stickContainer.anchoredPosition = position;
            _stickContainer.gameObject.SetActive(true);
        }

        public void MoveStick(Vector2 delta)
        {
            var stickPosition = delta.magnitude > _stickZoneSize / 2
                ? delta.normalized * _stickZoneSize / 2
                : delta;

            _stickTransform.anchoredPosition = stickPosition;
        }

        public void DisableStick()
        {
            _stickContainer.gameObject.SetActive(false);
            _stickTransform.anchoredPosition = Vector2.zero;
        }
    }
}