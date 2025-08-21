using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ShowCharacteristicsButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public Button Button => _button;
    }
}