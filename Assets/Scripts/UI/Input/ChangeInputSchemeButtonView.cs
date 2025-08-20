using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Input
{
    public class ChangeInputSchemeButtonView : MonoBehaviour
    {
        [FormerlySerializedAs("_shootButton")] [SerializeField] private Button _button;
        public Button Button => _button;
    }
}