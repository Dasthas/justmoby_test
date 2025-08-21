using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Characteristics
{
    public class CharacteristicElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _currentLevelText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _upgradeButton;

        public Button UpgradeButton => _upgradeButton;

        public void SetIcon(Sprite icon)
        {
            _iconImage.sprite = icon;
        }

        public void UpdateName(string elementName)
        {
            _nameText.text = elementName;
        }

        public void UpdateLevel(int currentLevel)
        {
            _currentLevelText.text = currentLevel.ToString();
        }

        public void UpdateCanUpgrade(bool canUpgrade)
        {
            UpgradeButton.interactable = canUpgrade;
        }
    }
}