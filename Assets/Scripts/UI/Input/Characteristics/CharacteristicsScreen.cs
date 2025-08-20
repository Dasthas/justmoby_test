using System.Collections.Generic;
using Services.Characteristics;
using Services.Characteristics.Settings;
using Services.Characteristics.Settings.Data;
using Services.Localization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Input.Characteristics
{
    public class CharacteristicsScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _availablePointsText;
        [SerializeField] private CharacteristicElement _characteristicElementPrefab;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _closeButton;

        [Inject] private ICharacteristicsService _characteristicsService;
        [Inject] private ILocalizationService _localizationService;

        private Dictionary<CharacteristicType, CharacteristicElement> _spawnedCharacteristics =
            new Dictionary<CharacteristicType, CharacteristicElement>();

        private Dictionary<CharacteristicType, int> _tempPoints = new Dictionary<CharacteristicType, int>();

        private CompositeDisposable _disposables;

        private int _tempSpendedPoints = 0;

        public void Open()
        {
            _disposables = new CompositeDisposable();
            _tempPoints.Clear();
            _tempSpendedPoints = 0;
            Time.timeScale = 0;
            _canvas.enabled = true;
            var allCharacteristics = _characteristicsService.Table.Characteristics;
            _availablePointsText.text = _characteristicsService.AvailablePoints.ToString();
            for (var i = 0; i < allCharacteristics.Count; i++)
            {
                var characteristicData = allCharacteristics[i];
                if (!_spawnedCharacteristics.TryGetValue(characteristicData.Type, out CharacteristicElement element))
                {
                    element = Instantiate(_characteristicElementPrefab, _content);
                    _spawnedCharacteristics.Add(characteristicData.Type, element);
                }
                else
                {
                    element = _spawnedCharacteristics[characteristicData.Type];
                }

                element.SetIcon(characteristicData.Icon);
                element.UpdateName(_localizationService.GetLocalizedString(characteristicData.NameLocalizationKey));
                element.UpdateLevel(_characteristicsService.GetUpgradeLevel(characteristicData.Type));
                element.UpdateCanUpgrade(_characteristicsService.CanUpgrade(characteristicData.Type));
                element.UpgradeButton
                    .OnClickAsObservable()
                    .Subscribe(_ => OnUpgradeClicked(characteristicData.Type, element))
                    .AddTo(_disposables);
                _tempPoints.Add(characteristicData.Type, 0);
            }

            _applyButton
                .OnClickAsObservable()
                .Subscribe(_ => ApplyAndClose())
                .AddTo(_disposables);
            _closeButton
                .OnClickAsObservable()
                .Subscribe(_ => Close())
                .AddTo(_disposables);
        }

        private void OnUpgradeClicked(CharacteristicType type, CharacteristicElement element)
        {
            _tempPoints[type]++;
            _tempSpendedPoints++;
            element.UpdateLevel(_characteristicsService.GetUpgradeLevel(type) + _tempPoints[type]);

            _availablePointsText.text = (_characteristicsService.AvailablePoints - _tempSpendedPoints).ToString();
            UpdateAllButtons();
        }

        private void UpdateAllButtons()
        {
            foreach (var (type, element) in _spawnedCharacteristics)
            {
                element.UpdateCanUpgrade(
                    _characteristicsService.CanUpgrade(type, _tempPoints[type] + 1, _tempSpendedPoints));
            }
        }

        private void ApplyAndClose()
        {
            foreach (var (type, level) in _tempPoints)
            {
                _characteristicsService.AddPlayerUpgrade(type, level);
            }

            _characteristicsService.RemoveAvailablePoints((uint)_tempSpendedPoints);
            Close();
        }

        private void Close()
        {
            Time.timeScale = 1;
            _canvas.enabled = false;
            _disposables.Dispose();
        }
    }
}