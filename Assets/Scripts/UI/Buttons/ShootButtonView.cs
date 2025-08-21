using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ShootButtonView : MonoBehaviour
    {
        [SerializeField] private Button _shootButton;
        public Button ShootButton => _shootButton;
    }
}