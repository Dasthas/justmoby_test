using System;
using System.Collections.Generic;
using System.Linq;
using Services.Base;
using Services.Characteristics.Settings;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.Characteristics
{
    public class CharacteristicsService : Service, ICharacteristicsService
    {
        [SerializeField] private CharacteristicsTable _characteristicsTable;
        private Dictionary<CharacteristicType, int> _activePlayerUpgradeLevels = new();
        private ReactiveCommand<CharacteristicType> _onCharacteristicUpgraded = new ReactiveCommand<CharacteristicType>();

        public CharacteristicsTable Table => _characteristicsTable;

        public uint AvailablePoints => _availablePoints;

        public IObservable<CharacteristicType> OnCharacteristicUpgraded => _onCharacteristicUpgraded;

        private uint _availablePoints = 0;

        public void AddAvailablePoints(uint points)
        {
            _availablePoints = AvailablePoints + points;
        }

        public void RemoveAvailablePoints(uint points)
        {
            _availablePoints = AvailablePoints - points;
        }

        public void AddPlayerUpgrade(CharacteristicType upgradeType, int levels = 1)
        {
            if (_activePlayerUpgradeLevels.TryGetValue(upgradeType, out int level))
            {
                level += levels;
                _activePlayerUpgradeLevels[upgradeType] = level;
            }
            else
            {
                _activePlayerUpgradeLevels.Add(upgradeType, levels);
            }
            _onCharacteristicUpgraded.Execute(upgradeType);
        }

        public bool CanUpgrade(CharacteristicType upgradeType, int levels = 1, int minusPoints = 0)
        {
            return _activePlayerUpgradeLevels[upgradeType] + levels <=
                Table.GetDataForType(upgradeType).MaxLevel && AvailablePoints - minusPoints > 0;
        }

        public int GetUpgradeLevel(CharacteristicType upgradeType)
        {
            return _activePlayerUpgradeLevels[upgradeType];
        }

        public float CalculateUpgradedValue(float defaultValue, CharacteristicType upgradeType)
        {
            var upgradeValue = Table.GetDataForType(upgradeType);
            var upgradeLvl = _activePlayerUpgradeLevels[upgradeType];

            switch (upgradeValue.UpgradeValueType)
            {
                case CharacteristicValueType.DirectIncrease:
                    return defaultValue + (upgradeValue.Value * upgradeLvl);
                case CharacteristicValueType.PercentDecrease:
                    return defaultValue - ((upgradeValue.Value / 100f) * upgradeLvl * defaultValue);
                case CharacteristicValueType.PercentIncrease:
                    return defaultValue + ((upgradeValue.Value / 100f) * upgradeLvl * defaultValue);
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _activePlayerUpgradeLevels.Clear();
            _activePlayerUpgradeLevels = Enum.GetValues(typeof(CharacteristicType))
                .Cast<CharacteristicType>()
                .ToDictionary(c => c, c => 0);
        }

        public override Service RegisterAndGetInstance(IContainerBuilder builder)
        {
            var instance = Clone() as CharacteristicsService;
            builder.RegisterInstance<ICharacteristicsService>(instance)
                .As<IInitializable>()
                .As<IDisposable>();
            return instance;
        }
    }
}