using UnityEngine;
using UnityEngine.UI;

namespace UI.Input
{
    public class ShootButtonView : MonoBehaviour
    {
        [SerializeField] private Button _shootButton;
        public Button ShootButton => _shootButton;
    }
}