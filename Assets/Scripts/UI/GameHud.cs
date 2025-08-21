using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private Image _hpFillImage;
        [SerializeField] private TMP_Text _hpText;

        public void UpdateHealth(float newHealth, float maxHealth)
        {
            var t = newHealth / maxHealth;
            t = Mathf.Clamp01(t);
            _hpFillImage.fillAmount = t;
            _hpText.text = $"{newHealth}/{maxHealth}";
        }
    }
}