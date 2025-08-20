using System;

namespace Services.Characteristics.Settings.Data.Value
{
    [Serializable]
    public struct CharacteristicValue
    {
        public CharacteristicValueType UpgradeValueType;
        public float Value;
        public uint MaxLevel;
    }
}