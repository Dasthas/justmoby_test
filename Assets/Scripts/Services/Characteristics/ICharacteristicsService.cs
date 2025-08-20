using System;
using Services.Characteristics.Settings;
using Services.Characteristics.Settings.Data;

namespace Services.Characteristics
{
    public interface ICharacteristicsService
    {
        void AddPlayerUpgrade(CharacteristicType upgradeType, int levels = 1);
        bool CanUpgrade(CharacteristicType upgradeType, int levels = 1, int minusPoints = 0);
        int GetUpgradeLevel(CharacteristicType upgradeType);
        float CalculateUpgradedValue(float defaultValue, CharacteristicType upgradeType);
        CharacteristicsTable Table { get; }
        uint AvailablePoints { get; }
        IObservable<CharacteristicType> OnCharacteristicUpgraded { get; }
        void AddAvailablePoints(uint points);
        void RemoveAvailablePoints(uint points);
    }
}